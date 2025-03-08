namespace MXTools.UI
{
  partial class LicenseForm
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
      label1 = new System.Windows.Forms.Label();
      _UserBox = new System.Windows.Forms.TextBox();
      label2 = new System.Windows.Forms.Label();
      _CreatedBox = new System.Windows.Forms.TextBox();
      label3 = new System.Windows.Forms.Label();
      _ByBox = new System.Windows.Forms.TextBox();
      label4 = new System.Windows.Forms.Label();
      _ExpireBox = new System.Windows.Forms.TextBox();
      _OKButton = new System.Windows.Forms.Button();
      linkLabel1 = new System.Windows.Forms.LinkLabel();
      label5 = new System.Windows.Forms.Label();
      SuspendLayout();
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new System.Drawing.Point(12, 10);
      label1.Name = "label1";
      label1.Size = new System.Drawing.Size(35, 17);
      label1.TabIndex = 0;
      label1.Text = "User";
      // 
      // _UserBox
      // 
      _UserBox.Location = new System.Drawing.Point(112, 6);
      _UserBox.Name = "_UserBox";
      _UserBox.ReadOnly = true;
      _UserBox.Size = new System.Drawing.Size(110, 25);
      _UserBox.TabIndex = 1;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new System.Drawing.Point(12, 43);
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size(54, 17);
      label2.TabIndex = 0;
      label2.Text = "Created";
      // 
      // _CreatedBox
      // 
      _CreatedBox.Location = new System.Drawing.Point(112, 39);
      _CreatedBox.Name = "_CreatedBox";
      _CreatedBox.ReadOnly = true;
      _CreatedBox.Size = new System.Drawing.Size(110, 25);
      _CreatedBox.TabIndex = 1;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new System.Drawing.Point(12, 76);
      label3.Name = "label3";
      label3.Size = new System.Drawing.Size(21, 17);
      label3.TabIndex = 0;
      label3.Text = "By";
      // 
      // _ByBox
      // 
      _ByBox.Location = new System.Drawing.Point(112, 72);
      _ByBox.Name = "_ByBox";
      _ByBox.ReadOnly = true;
      _ByBox.Size = new System.Drawing.Size(110, 25);
      _ByBox.TabIndex = 1;
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new System.Drawing.Point(12, 109);
      label4.Name = "label4";
      label4.Size = new System.Drawing.Size(44, 17);
      label4.TabIndex = 0;
      label4.Text = "Expire";
      // 
      // _ExpireBox
      // 
      _ExpireBox.Location = new System.Drawing.Point(112, 105);
      _ExpireBox.Name = "_ExpireBox";
      _ExpireBox.ReadOnly = true;
      _ExpireBox.Size = new System.Drawing.Size(110, 25);
      _ExpireBox.TabIndex = 1;
      // 
      // _OKButton
      // 
      _OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      _OKButton.Location = new System.Drawing.Point(75, 179);
      _OKButton.Name = "_OKButton";
      _OKButton.Size = new System.Drawing.Size(83, 25);
      _OKButton.TabIndex = 2;
      _OKButton.Text = "OK";
      _OKButton.UseVisualStyleBackColor = true;
      _OKButton.Click += OnOKClicked;
      // 
      // linkLabel1
      // 
      linkLabel1.AutoSize = true;
      linkLabel1.Location = new System.Drawing.Point(95, 142);
      linkLabel1.Name = "linkLabel1";
      linkLabel1.Size = new System.Drawing.Size(127, 17);
      linkLabel1.TabIndex = 3;
      linkLabel1.TabStop = true;
      linkLabel1.Text = "hmh3ba@gmail.com";
      // 
      // label5
      // 
      label5.AutoSize = true;
      label5.Location = new System.Drawing.Point(12, 142);
      label5.Name = "label5";
      label5.Size = new System.Drawing.Size(52, 17);
      label5.TabIndex = 0;
      label5.Text = "Contact";
      // 
      // LicenseForm
      // 
      AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      ClientSize = new System.Drawing.Size(233, 216);
      Controls.Add(_OKButton);
      Controls.Add(linkLabel1);
      Controls.Add(_ExpireBox);
      Controls.Add(label5);
      Controls.Add(label4);
      Controls.Add(_ByBox);
      Controls.Add(label3);
      Controls.Add(_CreatedBox);
      Controls.Add(label2);
      Controls.Add(_UserBox);
      Controls.Add(label1);
      FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "LicenseForm";
      ShowIcon = false;
      ShowInTaskbar = false;
      StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      Text = "License";
      TopMost = true;
      Load += LicenseForm_Load;
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox _UserBox;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox _CreatedBox;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox _ByBox;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox _ExpireBox;
    private System.Windows.Forms.Button _OKButton;
    private System.Windows.Forms.LinkLabel linkLabel1;
    private System.Windows.Forms.Label label5;
  }
}