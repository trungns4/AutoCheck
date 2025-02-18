namespace AutoCheck
{
  partial class ScanPortionForm
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
      this.label1 = new System.Windows.Forms.Label();
      this._ScanButton = new System.Windows.Forms.Button();
      this._StopButton = new System.Windows.Forms.Button();
      this._AvgTIme = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this._KeyDownDelay = new System.Windows.Forms.NumericUpDown();
      this._KeyUpDelay = new System.Windows.Forms.NumericUpDown();
      this._TestButton = new System.Windows.Forms.Button();
      this._StopTestButton = new System.Windows.Forms.Button();
      this.label4 = new System.Windows.Forms.Label();
      this._OffsetBox = new System.Windows.Forms.NumericUpDown();
      this.label5 = new System.Windows.Forms.Label();
      this._AddressBox = new System.Windows.Forms.TextBox();
      this._PortionBox = new System.Windows.Forms.NumericUpDown();
      ((System.ComponentModel.ISupportInitialize)(this._KeyDownDelay)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this._KeyUpDelay)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this._OffsetBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this._PortionBox)).BeginInit();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(13, 24);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(45, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Portions";
      // 
      // _ScanButton
      // 
      this._ScanButton.Location = new System.Drawing.Point(119, 90);
      this._ScanButton.Name = "_ScanButton";
      this._ScanButton.Size = new System.Drawing.Size(75, 23);
      this._ScanButton.TabIndex = 2;
      this._ScanButton.Text = "Scan";
      this._ScanButton.UseVisualStyleBackColor = true;
      this._ScanButton.Click += new System.EventHandler(this._ScanButton_Click);
      // 
      // _StopButton
      // 
      this._StopButton.Location = new System.Drawing.Point(200, 89);
      this._StopButton.Name = "_StopButton";
      this._StopButton.Size = new System.Drawing.Size(75, 23);
      this._StopButton.TabIndex = 2;
      this._StopButton.Text = "Stop";
      this._StopButton.UseVisualStyleBackColor = true;
      // 
      // _AvgTIme
      // 
      this._AvgTIme.Location = new System.Drawing.Point(12, 210);
      this._AvgTIme.Name = "_AvgTIme";
      this._AvgTIme.ReadOnly = true;
      this._AvgTIme.Size = new System.Drawing.Size(100, 20);
      this._AvgTIme.TabIndex = 3;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 191);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(74, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Avg Time (ms)";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(13, 133);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(56, 13);
      this.label3.TabIndex = 0;
      this.label3.Text = "Delay (ms)";
      // 
      // _KeyDownDelay
      // 
      this._KeyDownDelay.Location = new System.Drawing.Point(13, 152);
      this._KeyDownDelay.Name = "_KeyDownDelay";
      this._KeyDownDelay.Size = new System.Drawing.Size(120, 20);
      this._KeyDownDelay.TabIndex = 5;
      // 
      // _KeyUpDelay
      // 
      this._KeyUpDelay.Location = new System.Drawing.Point(142, 152);
      this._KeyUpDelay.Name = "_KeyUpDelay";
      this._KeyUpDelay.Size = new System.Drawing.Size(120, 20);
      this._KeyUpDelay.TabIndex = 5;
      // 
      // _TestButton
      // 
      this._TestButton.Location = new System.Drawing.Point(10, 247);
      this._TestButton.Name = "_TestButton";
      this._TestButton.Size = new System.Drawing.Size(75, 23);
      this._TestButton.TabIndex = 2;
      this._TestButton.Text = "Test";
      this._TestButton.UseVisualStyleBackColor = true;
      // 
      // _StopTestButton
      // 
      this._StopTestButton.Location = new System.Drawing.Point(91, 247);
      this._StopTestButton.Name = "_StopTestButton";
      this._StopTestButton.Size = new System.Drawing.Size(75, 23);
      this._StopTestButton.TabIndex = 2;
      this._StopTestButton.Text = "Stop";
      this._StopTestButton.UseVisualStyleBackColor = true;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(155, 24);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(35, 13);
      this.label4.TabIndex = 0;
      this.label4.Text = "Offset";
      // 
      // _OffsetBox
      // 
      this._OffsetBox.Location = new System.Drawing.Point(155, 40);
      this._OffsetBox.Name = "_OffsetBox";
      this._OffsetBox.Size = new System.Drawing.Size(120, 20);
      this._OffsetBox.TabIndex = 5;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(13, 75);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(45, 13);
      this.label5.TabIndex = 0;
      this.label5.Text = "Address";
      // 
      // _AddressBox
      // 
      this._AddressBox.Location = new System.Drawing.Point(13, 91);
      this._AddressBox.Name = "_AddressBox";
      this._AddressBox.ReadOnly = true;
      this._AddressBox.Size = new System.Drawing.Size(100, 20);
      this._AddressBox.TabIndex = 1;
      // 
      // _PortionBox
      // 
      this._PortionBox.Location = new System.Drawing.Point(13, 40);
      this._PortionBox.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
      this._PortionBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._PortionBox.Name = "_PortionBox";
      this._PortionBox.Size = new System.Drawing.Size(120, 20);
      this._PortionBox.TabIndex = 6;
      this._PortionBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // ScanPortionForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(287, 291);
      this.Controls.Add(this._PortionBox);
      this.Controls.Add(this._OffsetBox);
      this.Controls.Add(this._KeyUpDelay);
      this.Controls.Add(this._KeyDownDelay);
      this.Controls.Add(this.label2);
      this.Controls.Add(this._AvgTIme);
      this.Controls.Add(this._StopTestButton);
      this.Controls.Add(this._TestButton);
      this.Controls.Add(this._StopButton);
      this.Controls.Add(this._ScanButton);
      this.Controls.Add(this.label3);
      this.Controls.Add(this._AddressBox);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Name = "ScanPortionForm";
      this.ShowInTaskbar = false;
      this.Text = "Scan Portions";
      this.TopMost = true;
      ((System.ComponentModel.ISupportInitialize)(this._KeyDownDelay)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this._KeyUpDelay)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this._OffsetBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this._PortionBox)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button _ScanButton;
    private System.Windows.Forms.Button _StopButton;
    private System.Windows.Forms.TextBox _AvgTIme;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.NumericUpDown _KeyDownDelay;
    private System.Windows.Forms.NumericUpDown _KeyUpDelay;
    private System.Windows.Forms.Button _TestButton;
    private System.Windows.Forms.Button _StopTestButton;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.NumericUpDown _OffsetBox;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox _AddressBox;
    private System.Windows.Forms.NumericUpDown _PortionBox;
  }
}