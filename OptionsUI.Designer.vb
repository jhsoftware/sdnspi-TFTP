<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OptionsUI
    Inherits JHSoftware.SimpleDNS.Plugin.OptionsUI

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container
    Me.Label1 = New System.Windows.Forms.Label
    Me.ddServerIP = New System.Windows.Forms.ComboBox
    Me.Label2 = New System.Windows.Forms.Label
    Me.txtFolder = New System.Windows.Forms.TextBox
    Me.btnBrowse = New System.Windows.Forms.Button
    Me.Label3 = New System.Windows.Forms.Label
    Me.Label4 = New System.Windows.Forms.Label
    Me.Label5 = New System.Windows.Forms.Label
    Me.Label6 = New System.Windows.Forms.Label
    Me.Label7 = New System.Windows.Forms.Label
    Me.Label8 = New System.Windows.Forms.Label
    Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
    Me.numConn = New System.Windows.Forms.NumericUpDown
    Me.numReTx = New System.Windows.Forms.NumericUpDown
    Me.numBlk = New System.Windows.Forms.NumericUpDown
    Me.numTimeout = New System.Windows.Forms.NumericUpDown
    Me.DlgFolder = New System.Windows.Forms.FolderBrowserDialog
    CType(Me.numConn, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.numReTx, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.numBlk, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.numTimeout, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'Label1
    '
    Me.Label1.AutoSize = True
    Me.Label1.Location = New System.Drawing.Point(-3, 3)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(94, 13)
    Me.Label1.TabIndex = 0
    Me.Label1.Text = "Server IP address:"
    '
    'ddServerIP
    '
    Me.ddServerIP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.ddServerIP.FormattingEnabled = True
    Me.ddServerIP.Location = New System.Drawing.Point(97, 0)
    Me.ddServerIP.Margin = New System.Windows.Forms.Padding(3, 3, 3, 13)
    Me.ddServerIP.Name = "ddServerIP"
    Me.ddServerIP.Size = New System.Drawing.Size(196, 21)
    Me.ddServerIP.TabIndex = 1
    '
    'Label2
    '
    Me.Label2.AutoSize = True
    Me.Label2.Location = New System.Drawing.Point(-3, 40)
    Me.Label2.Name = "Label2"
    Me.Label2.Size = New System.Drawing.Size(62, 13)
    Me.Label2.TabIndex = 2
    Me.Label2.Text = "Data folder:"
    '
    'txtFolder
    '
    Me.txtFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtFolder.Location = New System.Drawing.Point(97, 37)
    Me.txtFolder.Margin = New System.Windows.Forms.Padding(3, 3, 3, 13)
    Me.txtFolder.Name = "txtFolder"
    Me.txtFolder.Size = New System.Drawing.Size(257, 20)
    Me.txtFolder.TabIndex = 3
    '
    'btnBrowse
    '
    Me.btnBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnBrowse.Location = New System.Drawing.Point(360, 35)
    Me.btnBrowse.Name = "btnBrowse"
    Me.btnBrowse.Size = New System.Drawing.Size(27, 23)
    Me.btnBrowse.TabIndex = 4
    Me.btnBrowse.Text = "..."
    Me.ToolTip1.SetToolTip(Me.btnBrowse, "Browse...")
    Me.btnBrowse.UseVisualStyleBackColor = True
    '
    'Label3
    '
    Me.Label3.AutoSize = True
    Me.Label3.Location = New System.Drawing.Point(-2, 75)
    Me.Label3.Name = "Label3"
    Me.Label3.Size = New System.Drawing.Size(94, 13)
    Me.Label3.TabIndex = 5
    Me.Label3.Text = "Max. connections:"
    '
    'Label4
    '
    Me.Label4.AutoSize = True
    Me.Label4.Location = New System.Drawing.Point(-3, 183)
    Me.Label4.Name = "Label4"
    Me.Label4.Size = New System.Drawing.Size(86, 13)
    Me.Label4.TabIndex = 13
    Me.Label4.Text = "Max. retransmits:"
    '
    'Label5
    '
    Me.Label5.AutoSize = True
    Me.Label5.Location = New System.Drawing.Point(-3, 111)
    Me.Label5.Name = "Label5"
    Me.Label5.Size = New System.Drawing.Size(83, 13)
    Me.Label5.TabIndex = 7
    Me.Label5.Text = "Max. block size:"
    '
    'Label6
    '
    Me.Label6.AutoSize = True
    Me.Label6.Location = New System.Drawing.Point(-3, 147)
    Me.Label6.Name = "Label6"
    Me.Label6.Size = New System.Drawing.Size(81, 13)
    Me.Label6.TabIndex = 10
    Me.Label6.Text = "Default timeout:"
    '
    'Label7
    '
    Me.Label7.AutoSize = True
    Me.Label7.Location = New System.Drawing.Point(167, 147)
    Me.Label7.Name = "Label7"
    Me.Label7.Size = New System.Drawing.Size(47, 13)
    Me.Label7.TabIndex = 12
    Me.Label7.Text = "seconds"
    '
    'Label8
    '
    Me.Label8.AutoSize = True
    Me.Label8.Location = New System.Drawing.Point(168, 111)
    Me.Label8.Name = "Label8"
    Me.Label8.Size = New System.Drawing.Size(32, 13)
    Me.Label8.TabIndex = 9
    Me.Label8.Text = "bytes"
    '
    'numConn
    '
    Me.numConn.Location = New System.Drawing.Point(98, 73)
    Me.numConn.Margin = New System.Windows.Forms.Padding(3, 3, 3, 13)
    Me.numConn.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
    Me.numConn.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
    Me.numConn.Name = "numConn"
    Me.numConn.Size = New System.Drawing.Size(64, 20)
    Me.numConn.TabIndex = 6
    Me.numConn.Value = New Decimal(New Integer() {20, 0, 0, 0})
    '
    'numReTx
    '
    Me.numReTx.Location = New System.Drawing.Point(97, 181)
    Me.numReTx.Margin = New System.Windows.Forms.Padding(3, 3, 3, 13)
    Me.numReTx.Maximum = New Decimal(New Integer() {25, 0, 0, 0})
    Me.numReTx.Name = "numReTx"
    Me.numReTx.Size = New System.Drawing.Size(64, 20)
    Me.numReTx.TabIndex = 14
    Me.numReTx.Value = New Decimal(New Integer() {4, 0, 0, 0})
    '
    'numBlk
    '
    Me.numBlk.Location = New System.Drawing.Point(97, 109)
    Me.numBlk.Margin = New System.Windows.Forms.Padding(3, 3, 3, 13)
    Me.numBlk.Maximum = New Decimal(New Integer() {65464, 0, 0, 0})
    Me.numBlk.Minimum = New Decimal(New Integer() {8, 0, 0, 0})
    Me.numBlk.Name = "numBlk"
    Me.numBlk.Size = New System.Drawing.Size(64, 20)
    Me.numBlk.TabIndex = 8
    Me.numBlk.Value = New Decimal(New Integer() {65464, 0, 0, 0})
    '
    'numTimeout
    '
    Me.numTimeout.Location = New System.Drawing.Point(97, 145)
    Me.numTimeout.Margin = New System.Windows.Forms.Padding(3, 3, 3, 13)
    Me.numTimeout.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
    Me.numTimeout.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
    Me.numTimeout.Name = "numTimeout"
    Me.numTimeout.Size = New System.Drawing.Size(64, 20)
    Me.numTimeout.TabIndex = 11
    Me.numTimeout.Value = New Decimal(New Integer() {5, 0, 0, 0})
    '
    'DlgFolder
    '
    Me.DlgFolder.Description = "TFTP Server data folder"
    '
    'OptionsUI
    '
    Me.Controls.Add(Me.numTimeout)
    Me.Controls.Add(Me.numBlk)
    Me.Controls.Add(Me.numReTx)
    Me.Controls.Add(Me.numConn)
    Me.Controls.Add(Me.Label8)
    Me.Controls.Add(Me.Label7)
    Me.Controls.Add(Me.Label6)
    Me.Controls.Add(Me.Label5)
    Me.Controls.Add(Me.Label4)
    Me.Controls.Add(Me.Label3)
    Me.Controls.Add(Me.btnBrowse)
    Me.Controls.Add(Me.txtFolder)
    Me.Controls.Add(Me.Label2)
    Me.Controls.Add(Me.ddServerIP)
    Me.Controls.Add(Me.Label1)
    Me.Name = "OptionsUI"
    Me.Size = New System.Drawing.Size(387, 211)
    CType(Me.numConn, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.numReTx, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.numBlk, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.numTimeout, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents ddServerIP As System.Windows.Forms.ComboBox
  Friend WithEvents Label2 As System.Windows.Forms.Label
  Friend WithEvents txtFolder As System.Windows.Forms.TextBox
  Friend WithEvents btnBrowse As System.Windows.Forms.Button
  Friend WithEvents Label3 As System.Windows.Forms.Label
  Friend WithEvents Label4 As System.Windows.Forms.Label
  Friend WithEvents Label5 As System.Windows.Forms.Label
  Friend WithEvents Label6 As System.Windows.Forms.Label
  Friend WithEvents Label7 As System.Windows.Forms.Label
  Friend WithEvents Label8 As System.Windows.Forms.Label
  Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
  Friend WithEvents numConn As System.Windows.Forms.NumericUpDown
  Friend WithEvents numReTx As System.Windows.Forms.NumericUpDown
  Friend WithEvents numBlk As System.Windows.Forms.NumericUpDown
  Friend WithEvents numTimeout As System.Windows.Forms.NumericUpDown
  Friend WithEvents DlgFolder As System.Windows.Forms.FolderBrowserDialog

End Class
