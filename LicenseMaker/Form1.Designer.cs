namespace MXTools
{
  partial class Form1
  {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      label1 = new Label();
      _UserBox = new TextBox();
      label2 = new Label();
      _ExpireBox = new DateTimePicker();
      _SaveButton = new Button();
      _LoadButton = new Button();
      SuspendLayout();
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(15, 16);
      label1.Name = "label1";
      label1.Size = new Size(35, 17);
      label1.TabIndex = 0;
      label1.Text = "User";
      // 
      // _UserBox
      // 
      _UserBox.Location = new Point(15, 36);
      _UserBox.Name = "_UserBox";
      _UserBox.Size = new Size(110, 25);
      _UserBox.TabIndex = 1;
      _UserBox.Text = "user";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(15, 75);
      label2.Name = "label2";
      label2.Size = new Size(44, 17);
      label2.TabIndex = 0;
      label2.Text = "Expire";
      // 
      // _ExpireBox
      // 
      _ExpireBox.Location = new Point(15, 95);
      _ExpireBox.Name = "_ExpireBox";
      _ExpireBox.Size = new Size(221, 25);
      _ExpireBox.TabIndex = 2;
      // 
      // _SaveButton
      // 
      _SaveButton.Location = new Point(15, 147);
      _SaveButton.Name = "_SaveButton";
      _SaveButton.Size = new Size(83, 25);
      _SaveButton.TabIndex = 3;
      _SaveButton.Text = "Save...";
      _SaveButton.UseVisualStyleBackColor = true;
      _SaveButton.Click += OnSaveClicked;
      // 
      // _LoadButton
      // 
      _LoadButton.Location = new Point(153, 147);
      _LoadButton.Name = "_LoadButton";
      _LoadButton.Size = new Size(83, 25);
      _LoadButton.TabIndex = 4;
      _LoadButton.Text = "Load...";
      _LoadButton.UseVisualStyleBackColor = true;
      _LoadButton.Click += OnLoadClicked;
      // 
      // Form1
      // 
      AutoScaleDimensions = new SizeF(7F, 17F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(255, 193);
      Controls.Add(_LoadButton);
      Controls.Add(_SaveButton);
      Controls.Add(_ExpireBox);
      Controls.Add(_UserBox);
      Controls.Add(label2);
      Controls.Add(label1);
      FormBorderStyle = FormBorderStyle.FixedDialog;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "Form1";
      StartPosition = FormStartPosition.CenterScreen;
      Text = "License Maker";
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Label label1;
    private TextBox _UserBox;
    private Label label2;
    private DateTimePicker _ExpireBox;
    private Button _SaveButton;
    private Button _LoadButton;
  }
}
