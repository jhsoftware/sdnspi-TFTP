Imports System.Net.Sockets

Friend Class Connection
  Friend File As System.IO.FileStream
  Friend ClientEP As System.Net.IPEndPoint
  Friend BlockSize As Integer = 512
  Friend MaxRetransmit As Integer
  Friend TimeOut As Integer
  Friend OpBlockSize As Boolean = False, OpTimeOut As Boolean = False, OpTSize As Boolean = False

  Friend AllDone As Boolean = False

  Private LastActivity As DateTime = DateTime.UtcNow
  Private BlockSendCount As Integer
  Private Sock As Socket
  Private SendBuf() As Byte
  Private SendBlockLen As Integer
  Private BlockNum As Integer
  Private RcvBuf() As Byte

  Sub Go(ByVal fileSize As Long, ByVal bindIP As System.Net.IPAddress)
    SyncLock Me
      ReDim SendBuf(Math.Max(512, BlockSize) + 4 - 1)
      ReDim RcvBuf(BlockSize + 4 - 1)

      Sock = New Socket(bindIP.AddressFamily, SocketType.Dgram, ProtocolType.Udp)
      Try
        Sock.Bind(New Net.IPEndPoint(bindIP, 0))
      Catch ex As Exception
        ShutDown() : Exit Sub
      End Try
      Try
        Sock.BeginReceive(RcvBuf, 0, RcvBuf.Length, SocketFlags.None, AddressOf ReceiveCB, Nothing)
      Catch ex As Exception
        ShutDown() : Exit Sub
      End Try

      If OpBlockSize Or OpTimeOut Or OpTSize Then
        SendBuf(1) = 6 'opcode=OACK(6)
        SendBlockLen = -2 'no block number in OACK
        If OpBlockSize Then AddString("blksize") : AddString(BlockSize.ToString)
        If OpTimeOut Then AddString("timeout") : AddString(TimeOut.ToString)
        If OpTSize Then AddString("tsize") : AddString(fileSize.ToString)
        SendCurBlock()
      Else
        PrepAndSendNextBlock()
      End If
    End SyncLock
  End Sub

  Sub AddString(ByVal s As String)
    Dim ba = System.Text.Encoding.ASCII.GetBytes(s)
    Array.Copy(ba, 0, SendBuf, SendBlockLen + 4, ba.Length)
    SendBlockLen += ba.Length
    SendBuf(SendBlockLen + 4) = 0
    SendBlockLen += 1
  End Sub

  Private Sub PrepAndSendNextBlock()
    If BlockNum > 0 AndAlso SendBlockLen < BlockSize Then ShutDown() : Exit Sub
    BlockNum += 1
    BlockSendCount = 0
    SendBuf(0) = 0 : SendBuf(1) = 3 'opcode=data(3)
    SendBuf(2) = CByte(BlockNum >> 8) : SendBuf(3) = CByte(BlockNum And 255)
    SendBlockLen = File.Read(SendBuf, 4, BlockSize)
    SendCurBlock()
  End Sub

  Private Sub SendCurBlock()
    LastActivity = DateTime.UtcNow
    BlockSendCount += 1
    Try
      Sock.BeginSendTo(SendBuf, 0, 4 + SendBlockLen, SocketFlags.None, ClientEP, Nothing, Nothing)
    Catch ex As Exception
      ShutDown()
    End Try
  End Sub

  Private Sub ReceiveCB(ByVal ar As IAsyncResult)
    SyncLock Me
      If AllDone Then Exit Sub
      Dim l As Integer
      Try
        l = Sock.EndReceive(ar)
      Catch
        If AllDone Then Exit Sub
        ShutDown()
        Exit Sub
      End Try
      If l < 4 Then GoTo markWait4Next
      Dim OpCode = RcvBuf(0) * 256 + RcvBuf(1)
      If OpCode <> 4 Then GoTo markWait4Next
      Dim bn = RcvBuf(2) * 256 + RcvBuf(3)

      If bn = BlockNum Then
        REM client received the last packet - ready for next
        PrepAndSendNextBlock()
      ElseIf bn = BlockNum - 1 Then
        REM client did not receive last packet - resend
        If BlockSendCount > MaxRetransmit Then ShutDown() : Exit Sub
        SendCurBlock()
      End If
      REM ELSE - completely out of bounds - probably from other sender 

markWait4Next:
      If AllDone Then Exit Sub
      Try
        Sock.BeginReceive(RcvBuf, 0, RcvBuf.Length, SocketFlags.None, AddressOf ReceiveCB, Nothing)
      Catch
        ShutDown() : Exit Sub
      End Try
    End SyncLock
  End Sub

  Friend Sub ShutDown()
    AllDone = True
    Try
      File.Close()
    Catch
    End Try
    Try
      Sock.Close()
    Catch
    End Try
  End Sub

  Friend Sub TimerTick()
    SyncLock Me
      If AllDone Then Exit Sub
      If DateTime.UtcNow.Subtract(LastActivity).TotalSeconds < TimeOut Then Exit Sub
      If BlockSendCount > MaxRetransmit Then ShutDown() : Exit Sub
      SendCurBlock()
    End SyncLock
  End Sub

End Class
