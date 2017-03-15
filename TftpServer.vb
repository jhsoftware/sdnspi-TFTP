Imports System.Net.Sockets

'opcode  operation
'1     Read request (RRQ)
'2     Write request (WRQ)
'3     Data (DATA)
'4     Acknowledgment (ACK)
'5     Error (ERROR)
'6     OACK

'Error Codes
' 0         Not defined, see error message (if any).
' 1         File not found.
' 2         Access violation.
' 3         Disk full or allocation exceeded.
' 4         Illegal TFTP operation.
' 5         Unknown transfer ID.
' 6         File already exists.
' 7         No such user.

Public Class TftpServer
  Implements JHSoftware.SimpleDNS.Plugin.IPlugInBase

  Private Cfg As MyConfig

  Private LSock As Socket
  Private LBuf(599) As Byte
  Private LEndPoint As Net.EndPoint = New Net.IPEndPoint(0, 0)

  Private MyTimer As System.Threading.Timer
  Private Conns As New List(Of Connection)

  Private ShuttingDown As Boolean = False

  Public Event LogLine(ByVal text As String) Implements JHSoftware.SimpleDNS.Plugin.IPlugInBase.LogLine
  Public Event AsyncError(ByVal ex As System.Exception) Implements JHSoftware.SimpleDNS.Plugin.IPlugInBase.AsyncError

  Public Function GetPlugInTypeInfo() As JHSoftware.SimpleDNS.Plugin.IPlugInBase.PlugInTypeInfo Implements JHSoftware.SimpleDNS.Plugin.IPlugInBase.GetPlugInTypeInfo
    Dim rv As JHSoftware.SimpleDNS.Plugin.IPlugInBase.PlugInTypeInfo
    rv.Name = "TFTP Server"
    rv.Description = "Read-only TFTP Server"
    rv.InfoURL = "http://www.simpledns.com/kb.aspx?kbid=1302"
    rv.ConfigFile = False
    rv.MultiThreaded = False
    Return rv
  End Function

  Public Function InstanceConflict(ByVal config1 As String, ByVal config2 As String, ByRef errorMsg As String) As Boolean Implements JHSoftware.SimpleDNS.Plugin.IPlugInBase.InstanceConflict
    Dim c1 = MyConfig.Load(config1)
    Dim c2 = MyConfig.Load(config2)
    If c1.ListenIP = c2.ListenIP Then errorMsg = "IP address conflicts with another TFTP Server plug-in" : Return True
    Return False

  End Function

  Public Sub LoadConfig(ByVal config As String, ByVal instanceID As System.Guid, ByVal dataPath As String, ByRef maxThreads As Integer) Implements JHSoftware.SimpleDNS.Plugin.IPlugInBase.LoadConfig
    Cfg = MyConfig.Load(config)
  End Sub

  Public Function GetOptionsUI(ByVal instanceID As System.Guid, ByVal dataPath As String) As JHSoftware.SimpleDNS.Plugin.OptionsUI Implements JHSoftware.SimpleDNS.Plugin.IPlugInBase.GetOptionsUI
    Return New OptionsUI
  End Function

  '............................................................................................................

  Public Sub StartService() Implements JHSoftware.SimpleDNS.Plugin.IPlugInBase.StartService
    SyncLock Me
      LSock = New Socket(Cfg.ListenIP.ToNetIPAddress.AddressFamily, SocketType.Dgram, ProtocolType.Udp)
      LEndPoint = New Net.IPEndPoint(Cfg.ListenIP.ToNetIPAddress, 69)
      Try
        LSock.Bind(LEndPoint)
      Catch ex As Exception
        Throw New Exception("Error binding to " & Cfg.ListenIP.ToString & ":69 UDP", ex)
      End Try

      Try
        LSock.BeginReceiveFrom(LBuf, 0, LBuf.Length, SocketFlags.None, LEndPoint, AddressOf ListenCB, Nothing)
      Catch ex As Exception
        If ShuttingDown Then Exit Sub
        Throw New Exception("Error listening for incoming requests", ex)
      End Try

      MyTimer = New System.Threading.Timer(AddressOf TimerCB, Nothing, 0, 1000)

      RaiseEvent LogLine("Listening for TFTP requests on " & Cfg.ListenIP.ToString & " UDP port 69")
    End SyncLock
  End Sub

  Public Sub StopService() Implements JHSoftware.SimpleDNS.Plugin.IPlugInBase.StopService
    SyncLock Me
      ShuttingDown = True
      MyTimer.Dispose()
      Try
        LSock.Close()
      Catch
      End Try
      For Each c In Conns
        c.ShutDown()
      Next
      Conns.Clear()
    End SyncLock
  End Sub

  Private Sub TimerCB(ByVal dummy As Object)
    SyncLock Me
      For Each cn In Conns
        cn.TimerTick()
      Next
      PurgeConns()
    End SyncLock
  End Sub

  Private Sub PurgeConns()
    Dim i = 0
    While i < Conns.Count
      If Conns(i).AllDone Then Conns.RemoveAt(i) Else i += 1
    End While
  End Sub

  Private Sub ListenCB(ByVal ar As IAsyncResult)
    SyncLock Me
      Dim l As Integer
      Try
        l = LSock.EndReceiveFrom(ar, LEndPoint)
      Catch ex As SocketException
        If ex.ErrorCode = 10054 Then GoTo markWait4Next
        If ShuttingDown Then Exit Sub
        RaiseEvent AsyncError(ex)
      Catch ex As Exception
        If ShuttingDown Then Exit Sub
        RaiseEvent AsyncError(ex)
      End Try

      Dim LL = "TFTP request from " & DirectCast(LEndPoint, System.Net.IPEndPoint).Address.ToString

      If l < 4 Then GoTo markWait4Next
      Dim OpCode = LBuf(0) * 256 + LBuf(1)
      If OpCode = 2 Then RaiseEvent LogLine(LL & " - Error 2: Write not allowed") : SendError(2, "Write not allowed") : GoTo markWait4Next
      If OpCode <> 1 Then GoTo markWait4Next 'ignore opcodes other than read
      Dim p = 2
      Dim FileName = GetNullTermString(LBuf, l, p)
      If String.IsNullOrEmpty(FileName) Then RaiseEvent LogLine(LL & " - Error 4: File name is required") : SendError(4, "File name is required") : GoTo markWait4Next
      LL &= " for """ & FileName & """"
      Dim x = GetNullTermString(LBuf, l, p).ToLower
      If x <> "netascii" And x <> "octet" Then RaiseEvent LogLine(LL & " - Error 4: Unknown Mode (" & x & ")") : SendError(4, "Unknown Mode") : GoTo markWait4Next

      PurgeConns()
      If Conns.Count >= Cfg.MaxConns Then RaiseEvent LogLine(LL & " Error 0: Too many connections") : SendError(0, "Too many connections - try again later") : GoTo markWait4Next

      Dim Cn = New Connection With {.ClientEP = DirectCast(LEndPoint, System.Net.IPEndPoint), _
                                    .MaxRetransmit = Cfg.MaxRetransmit, _
                                    .TimeOut = Cfg.DefTimeOut}

      Dim y As String, i As Integer
      Dim LLO = ""
      While p < l
        x = GetNullTermString(LBuf, l, p).ToLower
        If p >= l Then Exit While
        y = GetNullTermString(LBuf, l, p)
        Select Case x
          Case "blksize" 'RFC2348
            If Not Integer.TryParse(y, i) OrElse i < 8 OrElse i > 65464 Then Continue While
            Cn.BlockSize = Math.Min(i, Cfg.MaxBlockSize)
            Cn.OpBlockSize = True
            LLO &= If(LLO.Length > 0, ",", "") & "BlockSize=" & Cn.BlockSize
          Case "timeout" 'RFC2349
            If Not Integer.TryParse(y, i) OrElse i < 1 OrElse i > 255 Then Continue While
            Cn.TimeOut = i
            Cn.OpTimeOut = True
            LLO &= If(LLO.Length > 0, ",", "") & "Timeout=" & Cn.TimeOut
          Case "tsize" 'RFC2349
            Cn.OpTSize = True
            LLO &= If(LLO.Length > 0, ",", "") & "TSize"
        End Select
      End While
      If LLO.Length > 0 Then LL &= " [" & LLO & "]"

      FileName = FileName.Replace("..", "").Replace("/", "\")
      Dim fi As System.IO.FileInfo
      Try
        fi = New System.IO.FileInfo(Cfg.Folder & "\" & FileName)
      Catch ex As Exception
        SendError(2, ex.Message) : GoTo markWait4Next
      End Try

      If Not fi.Exists Then RaiseEvent LogLine(LL & " - Error 1: File not found") : SendError(1, "File not found") : GoTo markWait4Next

      Try
        Cn.File = fi.OpenRead
      Catch ex As Exception
        RaiseEvent LogLine(LL & " - Error 0: " & ex.Message)
        SendError(2, ex.Message) : GoTo markWait4Next
      End Try

      RaiseEvent LogLine(LL & " - Sending " & fi.Length & " bytes.")
      Conns.Add(Cn)
      Cn.Go(fi.Length, Cfg.ListenIP.ToNetIPAddress)

markWait4Next:
      Try
        LSock.BeginReceiveFrom(LBuf, 0, LBuf.Length, SocketFlags.None, LEndPoint, AddressOf ListenCB, Nothing)
      Catch ex As SocketException
        If ShuttingDown Then Exit Sub
        If ex.ErrorCode = 10054 Then GoTo markWait4Next
        RaiseEvent AsyncError(ex)
      Catch ex As Exception
        If ShuttingDown Then Exit Sub
        RaiseEvent AsyncError(ex)
      End Try
    End SyncLock
  End Sub

  Private Sub SendError(ByVal errCode As Byte, ByVal errMsg As String)
    Dim ba = System.Text.Encoding.ASCII.GetBytes(errMsg)
    Dim ba2(ba.Length + 5 - 1) As Byte
    ba2(1) = 5 'opcode=error(5)
    ba2(3) = errCode
    ba.CopyTo(ba2, 4)
    Try
      LSock.BeginSendTo(ba2, 0, ba2.Length, SocketFlags.None, LEndPoint, Nothing, Nothing)
    Catch
      REM ignore
    End Try
  End Sub

  Private Function GetNullTermString(ByVal buf As Byte(), ByVal bufLen As Integer, ByRef pos As Integer) As String
    If pos >= bufLen Then Return ""
    Dim rv As String
    For i = pos To bufLen - 1
      If buf(i) = 0 Then
        rv = System.Text.Encoding.ASCII.GetString(buf, pos, i - pos)
        pos = i + 1
        Return rv
      End If
    Next
    rv = System.Text.Encoding.ASCII.GetString(buf, pos, bufLen - pos)
    pos = bufLen
    Return rv
  End Function

#Region "not implemented"
  Public Event SaveConfig(ByVal config As String) Implements JHSoftware.SimpleDNS.Plugin.IPlugInBase.SaveConfig
  Public Sub LoadState(ByVal state As String) Implements JHSoftware.SimpleDNS.Plugin.IPlugInBase.LoadState
    REM nothing 
  End Sub

  Public Function SaveState() As String Implements JHSoftware.SimpleDNS.Plugin.IPlugInBase.SaveState
    Return ""
  End Function

#End Region
End Class
