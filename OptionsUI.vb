Public Class OptionsUI

  Public Overrides Sub LoadData(ByVal config As String)
    For Each ipsn In GetServerIPs()
      ddServerIP.Items.Add(ipsn.IPAddr)
    Next
    If ddServerIP.Items.Count > 0 Then ddServerIP.SelectedIndex = 0
    btnBrowse.Enabled = Not RemoteGUI
    If config Is Nothing Then Exit Sub 'new instance

    Dim cfg = MyConfig.Load(config)
    ddServerIP.SelectedIndex = -1
    For i = 0 To ddServerIP.Items.Count - 1
      If DirectCast(ddServerIP.Items(i), SdnsIP) = cfg.ListenIP Then ddServerIP.SelectedIndex = i : Exit For
    Next
    txtFolder.Text = cfg.Folder
    numConn.Value = cfg.MaxConns
    numReTx.Value = cfg.MaxRetransmit
    numBlk.Value = cfg.MaxBlockSize
    numTimeout.Value = cfg.DefTimeOut
  End Sub

  Public Overrides Function ValidateData() As Boolean
    If ddServerIP.SelectedIndex < 0 Then
      MessageBox.Show("No server IP address selected", "TFTP Server Plug-In", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Return False
    End If
    CleanFolder()
    If txtFolder.Text.Length = 0 Then
      MessageBox.Show("Data folder is required", "TFTP Server Plug-In", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Return False
    End If

    If Not RemoteGUI Then
      Dim fi As System.IO.DirectoryInfo
      Try
        fi = New System.IO.DirectoryInfo(txtFolder.Text)
      Catch ex As Exception
        MessageBox.Show("Invalid data folder location", "TFTP Server Plug-In", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Return False
      End Try
      If Not fi.Exists Then
        MessageBox.Show("Data folder location does not exist", "TFTP Server Plug-In", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Return False
      End If
    End If

    Return True
  End Function

  Public Overrides Function SaveData() As String
    Dim cfg As New MyConfig
    cfg.ListenIP = DirectCast(ddServerIP.SelectedItem, SdnsIP)
    cfg.Folder = txtFolder.Text
    cfg.MaxConns = CInt(numConn.Value)
    cfg.MaxRetransmit = CInt(numReTx.Value)
    cfg.MaxBlockSize = CInt(numBlk.Value)
    cfg.DefTimeOut = CInt(numTimeout.Value)
    Return cfg.Save
  End Function

  Private Sub CleanFolder()
    Dim x = txtFolder.Text.Trim.Replace("/", "\")
    While x.StartsWith("\")
      x = x.Substring(1).Trim
    End While
    While x.EndsWith("\")
      x = x.Substring(0, x.Length - 1).Trim
    End While
    txtFolder.Text = x
  End Sub

  Private Sub txtFolder_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFolder.LostFocus
    CleanFolder()
  End Sub

  Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
    CleanFolder()
    Try
      DlgFolder.SelectedPath = txtFolder.Text
    Catch
    End Try
    If DlgFolder.ShowDialog <> DialogResult.OK Then Exit Sub
    txtFolder.Text = DlgFolder.SelectedPath
  End Sub
End Class
