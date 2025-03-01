namespace MXTools
{
  partial class ScanHPForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      _OKButton = new System.Windows.Forms.Button();
      _CancelButton = new System.Windows.Forms.Button();
      label1 = new System.Windows.Forms.Label();
      _InputBox = new System.Windows.Forms.TextBox();
      _ProgBar = new System.Windows.Forms.ProgressBar();
      _ScanButton = new System.Windows.Forms.Button();
      _AdrBox = new System.Windows.Forms.TextBox();
      _StopButton = new System.Windows.Forms.Button();
      m_OffsetBox = new System.Windows.Forms.NumericUpDown();
      label2 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)m_OffsetBox).BeginInit();
      SuspendLayout();
      // 
      // _OKButton
      // 
      _OKButton.Location = new System.Drawing.Point(287, 89);
      _OKButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      _OKButton.Name = "_OKButton";
      _OKButton.Size = new System.Drawing.Size(88, 31);
      _OKButton.TabIndex = 0;
      _OKButton.Text = "OK";
      _OKButton.UseVisualStyleBackColor = true;
      _OKButton.Click += _OKButton_Click;
      // 
      // _CancelButton
      // 
      _CancelButton.Location = new System.Drawing.Point(381, 89);
      _CancelButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      _CancelButton.Name = "_CancelButton";
      _CancelButton.Size = new System.Drawing.Size(88, 31);
      _CancelButton.TabIndex = 1;
      _CancelButton.Text = "Cancel";
      _CancelButton.UseVisualStyleBackColor = true;
      _CancelButton.Click += _CancelButton_Click;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new System.Drawing.Point(9, 9);
      label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      label1.Name = "label1";
      label1.Size = new System.Drawing.Size(24, 17);
      label1.TabIndex = 2;
      label1.Text = "HP";
      label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // _InputBox
      // 
      _InputBox.Location = new System.Drawing.Point(9, 31);
      _InputBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      _InputBox.Name = "_InputBox";
      _InputBox.Size = new System.Drawing.Size(114, 25);
      _InputBox.TabIndex = 3;
      // 
      // _ProgBar
      // 
      _ProgBar.Location = new System.Drawing.Point(9, 65);
      _ProgBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      _ProgBar.Name = "_ProgBar";
      _ProgBar.Size = new System.Drawing.Size(460, 18);
      _ProgBar.TabIndex = 4;
      _ProgBar.Visible = false;
      // 
      // _ScanButton
      // 
      _ScanButton.Location = new System.Drawing.Point(9, 90);
      _ScanButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      _ScanButton.Name = "_ScanButton";
      _ScanButton.Size = new System.Drawing.Size(88, 31);
      _ScanButton.TabIndex = 5;
      _ScanButton.Text = "Scan";
      _ScanButton.UseVisualStyleBackColor = true;
      _ScanButton.Click += _Scan_Click;
      // 
      // _AdrBox
      // 
      _AdrBox.Location = new System.Drawing.Point(269, 31);
      _AdrBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      _AdrBox.Name = "_AdrBox";
      _AdrBox.ReadOnly = true;
      _AdrBox.Size = new System.Drawing.Size(200, 25);
      _AdrBox.TabIndex = 6;
      // 
      // _StopButton
      // 
      _StopButton.Location = new System.Drawing.Point(104, 90);
      _StopButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      _StopButton.Name = "_StopButton";
      _StopButton.Size = new System.Drawing.Size(88, 31);
      _StopButton.TabIndex = 7;
      _StopButton.Text = "Stop";
      _StopButton.UseVisualStyleBackColor = true;
      // 
      // m_OffsetBox
      // 
      m_OffsetBox.Location = new System.Drawing.Point(139, 31);
      m_OffsetBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      m_OffsetBox.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
      m_OffsetBox.Name = "m_OffsetBox";
      m_OffsetBox.Size = new System.Drawing.Size(114, 25);
      m_OffsetBox.TabIndex = 8;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new System.Drawing.Point(139, 9);
      label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size(43, 17);
      label2.TabIndex = 2;
      label2.Text = "Offset";
      label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // ScanHPForm
      // 
      AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      ClientSize = new System.Drawing.Size(484, 129);
      Controls.Add(m_OffsetBox);
      Controls.Add(label1);
      Controls.Add(_CancelButton);
      Controls.Add(_StopButton);
      Controls.Add(_OKButton);
      Controls.Add(label2);
      Controls.Add(_InputBox);
      Controls.Add(_AdrBox);
      Controls.Add(_ProgBar);
      Controls.Add(_ScanButton);
      FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      Location = new System.Drawing.Point(2, 250);
      Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "ScanHPForm";
      ShowInTaskbar = false;
      StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      Text = "Scan HP";
      TopMost = true;
      FormClosing += ScanHPForm_FormClosing;
      FormClosed += ScanHPForm_FormClosed;
      Load += ScanHPForm_Load;
      ((System.ComponentModel.ISupportInitialize)m_OffsetBox).EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Button _OKButton;
    private System.Windows.Forms.Button _CancelButton;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox _InputBox;
    private System.Windows.Forms.ProgressBar _ProgBar;
    private System.Windows.Forms.Button _ScanButton;
    private System.Windows.Forms.TextBox _AdrBox;
    private System.Windows.Forms.Button _StopButton;
    private System.Windows.Forms.NumericUpDown m_OffsetBox;
    private System.Windows.Forms.Label label2;
  }
}