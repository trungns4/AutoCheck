namespace AutoCheck
{
  partial class Form1
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      this.m_ScanButton = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.m_CurrentAddrBoxQ = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.m_MaxAddrBoxQ = new System.Windows.Forms.TextBox();
      this.m_StartButton = new System.Windows.Forms.Button();
      this.m_MaxValueBoxQ = new System.Windows.Forms.TextBox();
      this.m_CurrentValueBoxQ = new System.Windows.Forms.TextBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.m_VolumeCtrl = new System.Windows.Forms.TrackBar();
      this.m_AutoQ = new System.Windows.Forms.CheckBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.m_AutoW = new System.Windows.Forms.CheckBox();
      this.m_CurrentAddrBoxW = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.m_CurrentValueBoxW = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.m_MaxValueBoxW = new System.Windows.Forms.TextBox();
      this.m_MaxAddrBoxW = new System.Windows.Forms.TextBox();
      this.m_ContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.m_StartMenu = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
      this.m_ShowMenu = new System.Windows.Forms.ToolStripMenuItem();
      this.m_HideMenu = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.m_AutoHideMenu = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.m_CloseMenu = new System.Windows.Forms.ToolStripMenuItem();
      this.m_NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
      this.m_CloseButton = new System.Windows.Forms.Button();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.m_KeyCount = new System.Windows.Forms.Label();
      this.m_EChk = new System.Windows.Forms.CheckBox();
      this.m_WChk = new System.Windows.Forms.CheckBox();
      this.m_QChk = new System.Windows.Forms.CheckBox();
      this.m_KeyThreadDelay = new System.Windows.Forms.TextBox();
      this.m_KeyDelay = new System.Windows.Forms.TextBox();
      this.groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.m_VolumeCtrl)).BeginInit();
      this.groupBox2.SuspendLayout();
      this.m_ContextMenu.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.SuspendLayout();
      // 
      // m_ScanButtonQ
      // 
      this.m_ScanButton.Location = new System.Drawing.Point(148, 122);
      this.m_ScanButton.Name = "m_ScanButtonQ";
      this.m_ScanButton.Size = new System.Drawing.Size(75, 23);
      this.m_ScanButton.TabIndex = 0;
      this.m_ScanButton.Text = "Scan...";
      this.m_ScanButton.UseVisualStyleBackColor = true;
      this.m_ScanButton.Click += new System.EventHandler(this.OnScanQClicked);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 18);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(41, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Current";
      // 
      // m_CurrentAddrBoxQ
      // 
      this.m_CurrentAddrBoxQ.Location = new System.Drawing.Point(6, 35);
      this.m_CurrentAddrBoxQ.Name = "m_CurrentAddrBoxQ";
      this.m_CurrentAddrBoxQ.ReadOnly = true;
      this.m_CurrentAddrBoxQ.Size = new System.Drawing.Size(100, 20);
      this.m_CurrentAddrBoxQ.TabIndex = 2;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(6, 73);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(27, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Max";
      // 
      // m_MaxAddrBoxQ
      // 
      this.m_MaxAddrBoxQ.Location = new System.Drawing.Point(6, 90);
      this.m_MaxAddrBoxQ.Name = "m_MaxAddrBoxQ";
      this.m_MaxAddrBoxQ.ReadOnly = true;
      this.m_MaxAddrBoxQ.Size = new System.Drawing.Size(100, 20);
      this.m_MaxAddrBoxQ.TabIndex = 2;
      // 
      // m_StartButton
      // 
      this.m_StartButton.Location = new System.Drawing.Point(13, 530);
      this.m_StartButton.Name = "m_StartButton";
      this.m_StartButton.Size = new System.Drawing.Size(75, 23);
      this.m_StartButton.TabIndex = 3;
      this.m_StartButton.Text = "Start";
      this.m_StartButton.UseVisualStyleBackColor = true;
      this.m_StartButton.Click += new System.EventHandler(this.OnStartButtonClicked);
      // 
      // m_MaxValueBoxQ
      // 
      this.m_MaxValueBoxQ.Location = new System.Drawing.Point(125, 90);
      this.m_MaxValueBoxQ.Name = "m_MaxValueBoxQ";
      this.m_MaxValueBoxQ.ReadOnly = true;
      this.m_MaxValueBoxQ.Size = new System.Drawing.Size(100, 20);
      this.m_MaxValueBoxQ.TabIndex = 4;
      // 
      // m_CurrentValueBoxQ
      // 
      this.m_CurrentValueBoxQ.Location = new System.Drawing.Point(125, 35);
      this.m_CurrentValueBoxQ.Name = "m_CurrentValueBoxQ";
      this.m_CurrentValueBoxQ.ReadOnly = true;
      this.m_CurrentValueBoxQ.Size = new System.Drawing.Size(100, 20);
      this.m_CurrentValueBoxQ.TabIndex = 4;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.m_VolumeCtrl);
      this.groupBox1.Controls.Add(this.m_AutoQ);
      this.groupBox1.Controls.Add(this.m_CurrentAddrBoxQ);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.m_CurrentValueBoxQ);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.m_MaxValueBoxQ);
      this.groupBox1.Controls.Add(this.m_MaxAddrBoxQ);
      this.groupBox1.Location = new System.Drawing.Point(4, 13);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(234, 190);
      this.groupBox1.TabIndex = 6;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Q";
      // 
      // m_VolumeCtrl
      // 
      this.m_VolumeCtrl.Location = new System.Drawing.Point(87, 137);
      this.m_VolumeCtrl.Name = "m_VolumeCtrl";
      this.m_VolumeCtrl.Size = new System.Drawing.Size(138, 45);
      this.m_VolumeCtrl.TabIndex = 7;
      this.m_VolumeCtrl.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
      this.m_VolumeCtrl.ValueChanged += new System.EventHandler(this.OnVolumeValueChanged);
      // 
      // m_AutoQ
      // 
      this.m_AutoQ.AutoSize = true;
      this.m_AutoQ.Checked = true;
      this.m_AutoQ.CheckState = System.Windows.Forms.CheckState.Checked;
      this.m_AutoQ.Location = new System.Drawing.Point(9, 151);
      this.m_AutoQ.Name = "m_AutoQ";
      this.m_AutoQ.Size = new System.Drawing.Size(48, 17);
      this.m_AutoQ.TabIndex = 6;
      this.m_AutoQ.Text = "Auto";
      this.m_AutoQ.UseVisualStyleBackColor = true;
      this.m_AutoQ.CheckedChanged += new System.EventHandler(this.OnAutoQCheckedChanged);
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.m_AutoW);
      this.groupBox2.Controls.Add(this.m_CurrentAddrBoxW);
      this.groupBox2.Controls.Add(this.label3);
      this.groupBox2.Controls.Add(this.m_ScanButton);
      this.groupBox2.Controls.Add(this.m_CurrentValueBoxW);
      this.groupBox2.Controls.Add(this.label4);
      this.groupBox2.Controls.Add(this.m_MaxValueBoxW);
      this.groupBox2.Controls.Add(this.m_MaxAddrBoxW);
      this.groupBox2.Location = new System.Drawing.Point(4, 212);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(234, 161);
      this.groupBox2.TabIndex = 6;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "W";
      // 
      // m_AutoW
      // 
      this.m_AutoW.AutoSize = true;
      this.m_AutoW.Checked = true;
      this.m_AutoW.CheckState = System.Windows.Forms.CheckState.Checked;
      this.m_AutoW.Location = new System.Drawing.Point(9, 126);
      this.m_AutoW.Name = "m_AutoW";
      this.m_AutoW.Size = new System.Drawing.Size(48, 17);
      this.m_AutoW.TabIndex = 6;
      this.m_AutoW.Text = "Auto";
      this.m_AutoW.UseVisualStyleBackColor = true;
      this.m_AutoW.CheckedChanged += new System.EventHandler(this.OnAutoWCheckedChanged);
      // 
      // m_CurrentAddrBoxW
      // 
      this.m_CurrentAddrBoxW.Location = new System.Drawing.Point(6, 35);
      this.m_CurrentAddrBoxW.Name = "m_CurrentAddrBoxW";
      this.m_CurrentAddrBoxW.ReadOnly = true;
      this.m_CurrentAddrBoxW.Size = new System.Drawing.Size(100, 20);
      this.m_CurrentAddrBoxW.TabIndex = 2;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(6, 18);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(41, 13);
      this.label3.TabIndex = 1;
      this.label3.Text = "Current";
      // 
      // m_CurrentValueBoxW
      // 
      this.m_CurrentValueBoxW.Location = new System.Drawing.Point(125, 35);
      this.m_CurrentValueBoxW.Name = "m_CurrentValueBoxW";
      this.m_CurrentValueBoxW.ReadOnly = true;
      this.m_CurrentValueBoxW.Size = new System.Drawing.Size(100, 20);
      this.m_CurrentValueBoxW.TabIndex = 4;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(6, 73);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(27, 13);
      this.label4.TabIndex = 1;
      this.label4.Text = "Max";
      // 
      // m_MaxValueBoxW
      // 
      this.m_MaxValueBoxW.Location = new System.Drawing.Point(125, 90);
      this.m_MaxValueBoxW.Name = "m_MaxValueBoxW";
      this.m_MaxValueBoxW.ReadOnly = true;
      this.m_MaxValueBoxW.Size = new System.Drawing.Size(100, 20);
      this.m_MaxValueBoxW.TabIndex = 4;
      // 
      // m_MaxAddrBoxW
      // 
      this.m_MaxAddrBoxW.Location = new System.Drawing.Point(6, 90);
      this.m_MaxAddrBoxW.Name = "m_MaxAddrBoxW";
      this.m_MaxAddrBoxW.ReadOnly = true;
      this.m_MaxAddrBoxW.Size = new System.Drawing.Size(100, 20);
      this.m_MaxAddrBoxW.TabIndex = 2;
      // 
      // m_ContextMenu
      // 
      this.m_ContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_StartMenu,
            this.toolStripSeparator3,
            this.m_ShowMenu,
            this.m_HideMenu,
            this.toolStripSeparator1,
            this.m_AutoHideMenu,
            this.toolStripSeparator2,
            this.m_CloseMenu});
      this.m_ContextMenu.Name = "m_ContextMenu";
      this.m_ContextMenu.Size = new System.Drawing.Size(141, 132);
      this.m_ContextMenu.Opened += new System.EventHandler(this.m_ContextMenu_Opened);
      // 
      // m_StartMenu
      // 
      this.m_StartMenu.Name = "m_StartMenu";
      this.m_StartMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
      this.m_StartMenu.Size = new System.Drawing.Size(140, 22);
      this.m_StartMenu.Text = "Start";
      // 
      // toolStripSeparator3
      // 
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new System.Drawing.Size(137, 6);
      // 
      // m_ShowMenu
      // 
      this.m_ShowMenu.Name = "m_ShowMenu";
      this.m_ShowMenu.Size = new System.Drawing.Size(140, 22);
      this.m_ShowMenu.Text = "Show";
      // 
      // m_HideMenu
      // 
      this.m_HideMenu.Name = "m_HideMenu";
      this.m_HideMenu.Size = new System.Drawing.Size(140, 22);
      this.m_HideMenu.Text = "Hide";
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(137, 6);
      // 
      // m_AutoHideMenu
      // 
      this.m_AutoHideMenu.Name = "m_AutoHideMenu";
      this.m_AutoHideMenu.Size = new System.Drawing.Size(140, 22);
      this.m_AutoHideMenu.Text = "Auto Hide";
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(137, 6);
      // 
      // m_CloseMenu
      // 
      this.m_CloseMenu.Name = "m_CloseMenu";
      this.m_CloseMenu.Size = new System.Drawing.Size(140, 22);
      this.m_CloseMenu.Text = "Close";
      // 
      // m_NotifyIcon
      // 
      this.m_NotifyIcon.ContextMenuStrip = this.m_ContextMenu;
      this.m_NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("m_NotifyIcon.Icon")));
      this.m_NotifyIcon.Text = "AutoCheck";
      this.m_NotifyIcon.Visible = true;
      this.m_NotifyIcon.Click += new System.EventHandler(this.m_NotifyIcon_Click);
      // 
      // m_CloseButton
      // 
      this.m_CloseButton.Location = new System.Drawing.Point(152, 530);
      this.m_CloseButton.Name = "m_CloseButton";
      this.m_CloseButton.Size = new System.Drawing.Size(75, 23);
      this.m_CloseButton.TabIndex = 3;
      this.m_CloseButton.Text = "Close";
      this.m_CloseButton.UseVisualStyleBackColor = true;
      this.m_CloseButton.Click += new System.EventHandler(this.m_CloseButton_Click);
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.m_KeyCount);
      this.groupBox3.Controls.Add(this.m_EChk);
      this.groupBox3.Controls.Add(this.m_WChk);
      this.groupBox3.Controls.Add(this.m_QChk);
      this.groupBox3.Controls.Add(this.m_KeyThreadDelay);
      this.groupBox3.Controls.Add(this.m_KeyDelay);
      this.groupBox3.Location = new System.Drawing.Point(4, 379);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(234, 144);
      this.groupBox3.TabIndex = 7;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Auto QWE";
      // 
      // m_KeyCount
      // 
      this.m_KeyCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.m_KeyCount.Location = new System.Drawing.Point(62, 92);
      this.m_KeyCount.Name = "m_KeyCount";
      this.m_KeyCount.Size = new System.Drawing.Size(86, 44);
      this.m_KeyCount.TabIndex = 3;
      this.m_KeyCount.Text = "...";
      this.m_KeyCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // m_EChk
      // 
      this.m_EChk.AutoSize = true;
      this.m_EChk.Location = new System.Drawing.Point(190, 29);
      this.m_EChk.Name = "m_EChk";
      this.m_EChk.Size = new System.Drawing.Size(33, 17);
      this.m_EChk.TabIndex = 1;
      this.m_EChk.Text = "E";
      this.m_EChk.UseVisualStyleBackColor = true;
      // 
      // m_WChk
      // 
      this.m_WChk.AutoSize = true;
      this.m_WChk.Location = new System.Drawing.Point(98, 29);
      this.m_WChk.Name = "m_WChk";
      this.m_WChk.Size = new System.Drawing.Size(37, 17);
      this.m_WChk.TabIndex = 1;
      this.m_WChk.Text = "W";
      this.m_WChk.UseVisualStyleBackColor = true;
      // 
      // m_QChk
      // 
      this.m_QChk.AutoSize = true;
      this.m_QChk.Checked = true;
      this.m_QChk.CheckState = System.Windows.Forms.CheckState.Checked;
      this.m_QChk.Location = new System.Drawing.Point(6, 29);
      this.m_QChk.Name = "m_QChk";
      this.m_QChk.Size = new System.Drawing.Size(34, 17);
      this.m_QChk.TabIndex = 1;
      this.m_QChk.Text = "Q";
      this.m_QChk.UseVisualStyleBackColor = true;
      // 
      // m_KeyThreadDelay
      // 
      this.m_KeyThreadDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.m_KeyThreadDelay.Location = new System.Drawing.Point(125, 64);
      this.m_KeyThreadDelay.Name = "m_KeyThreadDelay";
      this.m_KeyThreadDelay.Size = new System.Drawing.Size(100, 20);
      this.m_KeyThreadDelay.TabIndex = 0;
      // 
      // m_KeyDelay
      // 
      this.m_KeyDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.m_KeyDelay.Location = new System.Drawing.Point(6, 64);
      this.m_KeyDelay.Name = "m_KeyDelay";
      this.m_KeyDelay.Size = new System.Drawing.Size(100, 20);
      this.m_KeyDelay.TabIndex = 0;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(244, 565);
      this.Controls.Add(this.groupBox3);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.m_CloseButton);
      this.Controls.Add(this.m_StartButton);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Location = new System.Drawing.Point(2, 250);
      this.MaximizeBox = false;
      this.Name = "Form1";
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "AutoCheck";
      this.TopMost = true;
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
      this.Load += new System.EventHandler(this.OnFormLoad);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.m_VolumeCtrl)).EndInit();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.m_ContextMenu.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button m_ScanButton;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox m_CurrentAddrBoxQ;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox m_MaxAddrBoxQ;
    private System.Windows.Forms.Button m_StartButton;
    private System.Windows.Forms.TextBox m_MaxValueBoxQ;
    private System.Windows.Forms.TextBox m_CurrentValueBoxQ;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.CheckBox m_AutoQ;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.CheckBox m_AutoW;
    private System.Windows.Forms.TextBox m_CurrentAddrBoxW;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox m_CurrentValueBoxW;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox m_MaxValueBoxW;
    private System.Windows.Forms.TextBox m_MaxAddrBoxW;
    private System.Windows.Forms.TrackBar m_VolumeCtrl;
    private System.Windows.Forms.ContextMenuStrip m_ContextMenu;
    private System.Windows.Forms.ToolStripMenuItem m_AutoHideMenu;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripMenuItem m_ShowMenu;
    private System.Windows.Forms.ToolStripMenuItem m_HideMenu;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem m_CloseMenu;
    private System.Windows.Forms.NotifyIcon m_NotifyIcon;
    private System.Windows.Forms.ToolStripMenuItem m_StartMenu;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.Button m_CloseButton;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.CheckBox m_EChk;
    private System.Windows.Forms.CheckBox m_WChk;
    private System.Windows.Forms.CheckBox m_QChk;
    private System.Windows.Forms.TextBox m_KeyDelay;
    private System.Windows.Forms.Label m_KeyCount;
    private System.Windows.Forms.TextBox m_KeyThreadDelay;
  }
}

