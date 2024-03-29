﻿Friend Class MyConfig
  Friend ListenIP As SdnsIP
  Friend Folder As String = ""
  Friend MaxConns As Integer = 10
  Friend MaxRetransmit As Integer = 4
  Friend MaxBlockSize As Integer = 4096
  Friend DefTimeOut As Integer = 5

  Friend Function Save() As String
    Return "1|" & _
           ListenIP.ToString & "|" & _
           PipeEncode(Folder) & "|" & _
           MaxConns & "|" & _
           MaxRetransmit & "|" & _
           MaxBlockSize & "|" & _
           DefTimeOut
  End Function

  Friend Shared Function Load(ByVal s As String) As MyConfig
    Dim v = PipeDecode(s)
    If v(0) <> "1" Then Throw New Exception("Unknown config version")
    Return New MyConfig With {.ListenIP = SdnsIP.Parse(v(1)),
                              .Folder = v(2),
                              .MaxConns = Integer.Parse(v(3)),
                              .MaxRetransmit = Integer.Parse(v(4)),
                              .MaxBlockSize = Integer.Parse(v(5)),
                              .DefTimeOut = Integer.Parse(v(6))}
  End Function

End Class
