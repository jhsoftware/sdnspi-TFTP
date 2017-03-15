Module modPipeEncoder

  Function PipeEncode(ByVal s As String) As String
    Return s.Replace("\"c, "\\").Replace("|", "\|")
  End Function

  Function PipeDecode(ByVal s As String) As String()
    'Dim tmp As New List(Of String)
    'Dim i, j, p As Integer
    'p = 0
    'Do
    '  i = s.IndexOf("|", p)
    '  If i < 0 Then i = s.Length
    '  j = s.IndexOf("\", p)
    '  If j < 0 Then j = s.Length

    '  If i = j Then tmp.Add(s) : Return tmp.ToArray
    '  If i < j Then
    '    tmp.Add(s.Substring(0, i))
    '    s = s.Substring(i + 1)
    '    p = 0
    '  Else
    '    If j < s.Length - 1 Then s = s.Substring(0, j) & s.Substring(j + 1)
    '    p = j + 1
    '  End If
    'Loop

    Dim tmp As New Generic.List(Of String)
    If s.Length = 0 Then Return tmp.ToArray
    Dim i, j, p As Integer
    Dim x As String = ""
    p = 0
    Do
      i = s.IndexOf("|"c, p)
      If i < 0 Then i = s.Length
      j = s.IndexOf("\"c, p)
      If j < 0 Then j = s.Length

      If i <= j Then
        tmp.Add(x & s.Substring(p, i - p))
        If i = j Then Return tmp.ToArray
        x = ""
        p = i + 1
      Else
        x &= s.Substring(p, j - p) & s(j + 1)
        p = j + 2
      End If
    Loop

  End Function

End Module
