namespace AutoCheck
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
      this._OKButton = new System.Windows.Forms.Button();
      this._CancelButton = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this._InputBox = new System.Windows.Forms.TextBox();
      this._ProgBar = new System.Windows.Forms.ProgressBar();
      this._ScanButton = new System.Windows.Forms.Button();
      this._AdrBox = new System.Windows.Forms.TextBox();
      this._StopButton = new System.Windows.Forms.Button();
      this.m_OffsetBox = new System.Windows.Forms.NumericUpDown();
      this.label2 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.m_OffsetBox)).BeginInit();
      this.SuspendLayout();
      // 
      // _OKButton
      // 
      this._OKButton.Location = new System.Drawing.Point(183, 77);
      this._OKButton.Name = "_OKButton";
      this._OKButton.Size = new System.Drawing.Size(75, 23);
      this._OKButton.TabIndex = 0;
      this._OKButton.Text = "OK";
      this._OKButton.UseVisualStyleBackColor = true;
      this._OKButton.Click += new System.EventHandler(this._OKButton_Click);
      // 
      // _CancelButton
      // 
      this._CancelButton.Location = new System.Drawing.Point(264, 77);
      this._CancelButton.Name = "_CancelButton";
      this._CancelButton.Size = new System.Drawing.Size(75, 23);
      this._CancelButton.TabIndex = 1;
      this._CancelButton.Text = "Cancel";
      this._CancelButton.UseVisualStyleBackColor = true;
      this._CancelButton.Click += new System.EventHandler(this._CancelButton_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(21, 7);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(22, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "HP";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // _InputBox
      // 
      this._InputBox.Location = new System.Drawing.Point(21, 23);
      this._InputBox.Name = "_InputBox";
      this._InputBox.Size = new System.Drawing.Size(98, 20);
      this._InputBox.TabIndex = 3;
      // 
      // _ProgBar
      // 
      this._ProgBar.Location = new System.Drawing.Point(21, 49);
      this._ProgBar.Name = "_ProgBar";
      this._ProgBar.Size = new System.Drawing.Size(320, 13);
      this._ProgBar.TabIndex = 4;
      this._ProgBar.Visible = false;
      // 
      // _ScanButton
      // 
      this._ScanButton.Location = new System.Drawing.Point(21, 77);
      this._ScanButton.Name = "_ScanButton";
      this._ScanButton.Size = new System.Drawing.Size(75, 23);
      this._ScanButton.TabIndex = 5;
      this._ScanButton.Text = "Scan";
      this._ScanButton.UseVisualStyleBackColor = true;
      this._ScanButton.Click += new System.EventHandler(this._Scan_Click);
      // 
      // _AdrBox
      // 
      this._AdrBox.Location = new System.Drawing.Point(243, 23);
      this._AdrBox.Name = "_AdrBox";
      this._AdrBox.ReadOnly = true;
      this._AdrBox.Size = new System.Drawing.Size(98, 20);
      this._AdrBox.TabIndex = 6;
      // 
      // _StopButton
      // 
      this._StopButton.Location = new System.Drawing.Point(102, 77);
      this._StopButton.Name = "_StopButton";
      this._StopButton.Size = new System.Drawing.Size(75, 23);
      this._StopButton.TabIndex = 7;
      this._StopButton.Text = "Stop";
      this._StopButton.UseVisualStyleBackColor = true;
      // 
      // m_OffsetBox
      // 
      this.m_OffsetBox.Location = new System.Drawing.Point(132, 23);
      this.m_OffsetBox.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
      this.m_OffsetBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.m_OffsetBox.Name = "m_OffsetBox";
      this.m_OffsetBox.Size = new System.Drawing.Size(98, 20);
      this.m_OffsetBox.TabIndex = 8;
      this.m_OffsetBox.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(132, 7);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(52, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Offset (%)";
      this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // ScanHPForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(365, 117);
      this.Controls.Add(this.m_OffsetBox);
      this.Controls.Add(this.label1);
      this.Controls.Add(this._CancelButton);
      this.Controls.Add(this._StopButton);
      this.Controls.Add(this._OKButton);
      this.Controls.Add(this.label2);
      this.Controls.Add(this._InputBox);
      this.Controls.Add(this._AdrBox);
      this.Controls.Add(this._ProgBar);
      this.Controls.Add(this._ScanButton);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Location = new System.Drawing.Point(2, 250);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ScanHPForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "Scan HP";
      this.TopMost = true;
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ScanHPForm_FormClosing);
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ScanHPForm_FormClosed);
      this.Load += new System.EventHandler(this.ScanHPForm_Load);
      ((System.ComponentModel.ISupportInitialize)(this.m_OffsetBox)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

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