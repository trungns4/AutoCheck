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
      components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      m_ScanButton = new System.Windows.Forms.Button();
      m_StartButton = new System.Windows.Forms.Button();
      groupBox1 = new System.Windows.Forms.GroupBox();
      m_QPBar = new System.Windows.Forms.ProgressBar();
      label1 = new System.Windows.Forms.Label();
      m_VolumeCtrl = new System.Windows.Forms.TrackBar();
      m_AutoQ = new System.Windows.Forms.CheckBox();
      groupBox2 = new System.Windows.Forms.GroupBox();
      m_WPBar = new System.Windows.Forms.ProgressBar();
      m_AutoW = new System.Windows.Forms.CheckBox();
      m_ContextMenu = new System.Windows.Forms.ContextMenuStrip(components);
      m_StartMenu = new System.Windows.Forms.ToolStripMenuItem();
      toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
      m_ShowMenu = new System.Windows.Forms.ToolStripMenuItem();
      m_HideMenu = new System.Windows.Forms.ToolStripMenuItem();
      toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      m_AutoHideMenu = new System.Windows.Forms.ToolStripMenuItem();
      toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      m_CloseMenu = new System.Windows.Forms.ToolStripMenuItem();
      m_NotifyIcon = new System.Windows.Forms.NotifyIcon(components);
      m_CloseButton = new System.Windows.Forms.Button();
      groupBox3 = new System.Windows.Forms.GroupBox();
      m_KeyCount = new System.Windows.Forms.Label();
      m_EChk = new System.Windows.Forms.CheckBox();
      m_WChk = new System.Windows.Forms.CheckBox();
      m_QChk = new System.Windows.Forms.CheckBox();
      _About = new System.Windows.Forms.Label();
      _SettingsButton = new System.Windows.Forms.Button();
      m_AutoMouse = new System.Windows.Forms.CheckBox();
      label2 = new System.Windows.Forms.Label();
      groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)m_VolumeCtrl).BeginInit();
      groupBox2.SuspendLayout();
      m_ContextMenu.SuspendLayout();
      groupBox3.SuspendLayout();
      SuspendLayout();
      // 
      // m_ScanButton
      // 
      m_ScanButton.Location = new System.Drawing.Point(6, 234);
      m_ScanButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      m_ScanButton.Name = "m_ScanButton";
      m_ScanButton.Size = new System.Drawing.Size(88, 27);
      m_ScanButton.TabIndex = 0;
      m_ScanButton.Text = "Scan...";
      m_ScanButton.UseVisualStyleBackColor = true;
      m_ScanButton.Click += OnScanClicked;
      // 
      // m_StartButton
      // 
      m_StartButton.Location = new System.Drawing.Point(6, 435);
      m_StartButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      m_StartButton.Name = "m_StartButton";
      m_StartButton.Size = new System.Drawing.Size(88, 27);
      m_StartButton.TabIndex = 3;
      m_StartButton.Text = "Start";
      m_StartButton.UseVisualStyleBackColor = true;
      m_StartButton.Click += OnStartButtonClicked;
      // 
      // groupBox1
      // 
      groupBox1.Controls.Add(m_QPBar);
      groupBox1.Controls.Add(label1);
      groupBox1.Controls.Add(m_VolumeCtrl);
      groupBox1.Controls.Add(m_AutoQ);
      groupBox1.Location = new System.Drawing.Point(5, 15);
      groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      groupBox1.Name = "groupBox1";
      groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
      groupBox1.Size = new System.Drawing.Size(229, 140);
      groupBox1.TabIndex = 6;
      groupBox1.TabStop = false;
      groupBox1.Text = "HP";
      // 
      // m_QPBar
      // 
      m_QPBar.Location = new System.Drawing.Point(8, 28);
      m_QPBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      m_QPBar.Name = "m_QPBar";
      m_QPBar.Size = new System.Drawing.Size(140, 17);
      m_QPBar.TabIndex = 9;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new System.Drawing.Point(4, 59);
      label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      label1.Name = "label1";
      label1.Size = new System.Drawing.Size(95, 15);
      label1.TabIndex = 8;
      label1.Text = "Warning Volume";
      // 
      // m_VolumeCtrl
      // 
      m_VolumeCtrl.Location = new System.Drawing.Point(7, 80);
      m_VolumeCtrl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      m_VolumeCtrl.Name = "m_VolumeCtrl";
      m_VolumeCtrl.Size = new System.Drawing.Size(217, 45);
      m_VolumeCtrl.TabIndex = 7;
      m_VolumeCtrl.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
      m_VolumeCtrl.ValueChanged += OnVolumeValueChanged;
      // 
      // m_AutoQ
      // 
      m_AutoQ.AutoSize = true;
      m_AutoQ.Checked = true;
      m_AutoQ.CheckState = System.Windows.Forms.CheckState.Checked;
      m_AutoQ.Location = new System.Drawing.Point(155, 27);
      m_AutoQ.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      m_AutoQ.Name = "m_AutoQ";
      m_AutoQ.Size = new System.Drawing.Size(52, 19);
      m_AutoQ.TabIndex = 6;
      m_AutoQ.Text = "Auto";
      m_AutoQ.UseVisualStyleBackColor = true;
      m_AutoQ.CheckedChanged += OnAutoQCheckedChanged;
      // 
      // groupBox2
      // 
      groupBox2.Controls.Add(m_WPBar);
      groupBox2.Controls.Add(m_AutoW);
      groupBox2.Location = new System.Drawing.Point(6, 162);
      groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      groupBox2.Name = "groupBox2";
      groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
      groupBox2.Size = new System.Drawing.Size(227, 66);
      groupBox2.TabIndex = 6;
      groupBox2.TabStop = false;
      groupBox2.Text = "Mana";
      // 
      // m_WPBar
      // 
      m_WPBar.Location = new System.Drawing.Point(7, 28);
      m_WPBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      m_WPBar.Name = "m_WPBar";
      m_WPBar.Size = new System.Drawing.Size(140, 17);
      m_WPBar.TabIndex = 9;
      // 
      // m_AutoW
      // 
      m_AutoW.AutoSize = true;
      m_AutoW.Checked = true;
      m_AutoW.CheckState = System.Windows.Forms.CheckState.Checked;
      m_AutoW.Location = new System.Drawing.Point(155, 27);
      m_AutoW.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      m_AutoW.Name = "m_AutoW";
      m_AutoW.Size = new System.Drawing.Size(52, 19);
      m_AutoW.TabIndex = 6;
      m_AutoW.Text = "Auto";
      m_AutoW.UseVisualStyleBackColor = true;
      m_AutoW.CheckedChanged += OnAutoWCheckedChanged;
      // 
      // m_ContextMenu
      // 
      m_ContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { m_StartMenu, toolStripSeparator3, m_ShowMenu, m_HideMenu, toolStripSeparator1, m_AutoHideMenu, toolStripSeparator2, m_CloseMenu });
      m_ContextMenu.Name = "m_ContextMenu";
      m_ContextMenu.Size = new System.Drawing.Size(129, 132);
      m_ContextMenu.Opened += m_ContextMenu_Opened;
      // 
      // m_StartMenu
      // 
      m_StartMenu.Name = "m_StartMenu";
      m_StartMenu.Size = new System.Drawing.Size(128, 22);
      m_StartMenu.Text = "Start";
      // 
      // toolStripSeparator3
      // 
      toolStripSeparator3.Name = "toolStripSeparator3";
      toolStripSeparator3.Size = new System.Drawing.Size(125, 6);
      // 
      // m_ShowMenu
      // 
      m_ShowMenu.Name = "m_ShowMenu";
      m_ShowMenu.Size = new System.Drawing.Size(128, 22);
      m_ShowMenu.Text = "Show";
      // 
      // m_HideMenu
      // 
      m_HideMenu.Name = "m_HideMenu";
      m_HideMenu.Size = new System.Drawing.Size(128, 22);
      m_HideMenu.Text = "Hide";
      // 
      // toolStripSeparator1
      // 
      toolStripSeparator1.Name = "toolStripSeparator1";
      toolStripSeparator1.Size = new System.Drawing.Size(125, 6);
      // 
      // m_AutoHideMenu
      // 
      m_AutoHideMenu.Name = "m_AutoHideMenu";
      m_AutoHideMenu.Size = new System.Drawing.Size(128, 22);
      m_AutoHideMenu.Text = "Auto Hide";
      // 
      // toolStripSeparator2
      // 
      toolStripSeparator2.Name = "toolStripSeparator2";
      toolStripSeparator2.Size = new System.Drawing.Size(125, 6);
      // 
      // m_CloseMenu
      // 
      m_CloseMenu.Name = "m_CloseMenu";
      m_CloseMenu.Size = new System.Drawing.Size(128, 22);
      m_CloseMenu.Text = "Close";
      // 
      // m_NotifyIcon
      // 
      m_NotifyIcon.ContextMenuStrip = m_ContextMenu;
      m_NotifyIcon.Icon = (System.Drawing.Icon)resources.GetObject("m_NotifyIcon.Icon");
      m_NotifyIcon.Text = "AutoCheck";
      m_NotifyIcon.Visible = true;
      m_NotifyIcon.Click += m_NotifyIcon_Click;
      // 
      // m_CloseButton
      // 
      m_CloseButton.Location = new System.Drawing.Point(145, 435);
      m_CloseButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      m_CloseButton.Name = "m_CloseButton";
      m_CloseButton.Size = new System.Drawing.Size(88, 27);
      m_CloseButton.TabIndex = 3;
      m_CloseButton.Text = "Close";
      m_CloseButton.UseVisualStyleBackColor = true;
      m_CloseButton.Click += m_CloseButton_Click;
      // 
      // groupBox3
      // 
      groupBox3.Controls.Add(m_KeyCount);
      groupBox3.Controls.Add(m_EChk);
      groupBox3.Controls.Add(m_WChk);
      groupBox3.Controls.Add(m_QChk);
      groupBox3.Location = new System.Drawing.Point(6, 273);
      groupBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      groupBox3.Name = "groupBox3";
      groupBox3.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
      groupBox3.Size = new System.Drawing.Size(227, 115);
      groupBox3.TabIndex = 7;
      groupBox3.TabStop = false;
      groupBox3.Text = "Auto QWE";
      // 
      // m_KeyCount
      // 
      m_KeyCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
      m_KeyCount.Location = new System.Drawing.Point(41, 66);
      m_KeyCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      m_KeyCount.Name = "m_KeyCount";
      m_KeyCount.Size = new System.Drawing.Size(144, 34);
      m_KeyCount.TabIndex = 3;
      m_KeyCount.Text = "...";
      m_KeyCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // m_EChk
      // 
      m_EChk.AutoSize = true;
      m_EChk.Location = new System.Drawing.Point(173, 33);
      m_EChk.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      m_EChk.Name = "m_EChk";
      m_EChk.Size = new System.Drawing.Size(32, 19);
      m_EChk.TabIndex = 1;
      m_EChk.Text = "E";
      m_EChk.UseVisualStyleBackColor = true;
      m_EChk.CheckStateChanged += OnEChkChanged;
      // 
      // m_WChk
      // 
      m_WChk.AutoSize = true;
      m_WChk.Location = new System.Drawing.Point(89, 33);
      m_WChk.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      m_WChk.Name = "m_WChk";
      m_WChk.Size = new System.Drawing.Size(37, 19);
      m_WChk.TabIndex = 1;
      m_WChk.Text = "W";
      m_WChk.UseVisualStyleBackColor = true;
      m_WChk.CheckedChanged += OnWChkChanged;
      // 
      // m_QChk
      // 
      m_QChk.AutoSize = true;
      m_QChk.Checked = true;
      m_QChk.CheckState = System.Windows.Forms.CheckState.Checked;
      m_QChk.Location = new System.Drawing.Point(7, 33);
      m_QChk.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      m_QChk.Name = "m_QChk";
      m_QChk.Size = new System.Drawing.Size(35, 19);
      m_QChk.TabIndex = 1;
      m_QChk.Text = "Q";
      m_QChk.UseVisualStyleBackColor = true;
      m_QChk.CheckedChanged += OnQChkChanged;
      // 
      // _About
      // 
      _About.AutoSize = true;
      _About.ForeColor = System.Drawing.SystemColors.ControlDark;
      _About.Location = new System.Drawing.Point(14, 467);
      _About.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      _About.Name = "_About";
      _About.Size = new System.Drawing.Size(13, 15);
      _About.TabIndex = 8;
      _About.Text = "..";
      // 
      // _SettingsButton
      // 
      _SettingsButton.Location = new System.Drawing.Point(145, 234);
      _SettingsButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      _SettingsButton.Name = "_SettingsButton";
      _SettingsButton.Size = new System.Drawing.Size(88, 27);
      _SettingsButton.TabIndex = 9;
      _SettingsButton.Text = "Settings...";
      _SettingsButton.UseVisualStyleBackColor = true;
      _SettingsButton.Click += _SettingsButton_Click_1;
      // 
      // m_AutoMouse
      // 
      m_AutoMouse.AutoSize = true;
      m_AutoMouse.Checked = true;
      m_AutoMouse.CheckState = System.Windows.Forms.CheckState.Checked;
      m_AutoMouse.Location = new System.Drawing.Point(6, 396);
      m_AutoMouse.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      m_AutoMouse.Name = "m_AutoMouse";
      m_AutoMouse.Size = new System.Drawing.Size(136, 19);
      m_AutoMouse.TabIndex = 10;
      m_AutoMouse.Text = "Right Mouse (Ctrl-A)";
      m_AutoMouse.UseVisualStyleBackColor = true;
      m_AutoMouse.CheckedChanged += m_AutoMouse_CheckedChanged;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new System.Drawing.Point(100, 441);
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size(37, 15);
      label2.TabIndex = 4;
      label2.Text = "Ctrl-0";
      // 
      // Form1
      // 
      AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      ClientSize = new System.Drawing.Size(247, 493);
      Controls.Add(label2);
      Controls.Add(m_AutoMouse);
      Controls.Add(_SettingsButton);
      Controls.Add(_About);
      Controls.Add(m_ScanButton);
      Controls.Add(groupBox3);
      Controls.Add(groupBox2);
      Controls.Add(groupBox1);
      Controls.Add(m_CloseButton);
      Controls.Add(m_StartButton);
      FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
      Location = new System.Drawing.Point(2, 250);
      Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      MaximizeBox = false;
      Name = "Form1";
      StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      Text = "AutoCheck";
      TopMost = true;
      FormClosing += OnFormClosing;
      FormClosed += OnFormClosed;
      Load += OnFormLoad;
      groupBox1.ResumeLayout(false);
      groupBox1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)m_VolumeCtrl).EndInit();
      groupBox2.ResumeLayout(false);
      groupBox2.PerformLayout();
      m_ContextMenu.ResumeLayout(false);
      groupBox3.ResumeLayout(false);
      groupBox3.PerformLayout();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Button m_ScanButton;
    private System.Windows.Forms.Button m_StartButton;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.CheckBox m_AutoQ;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.CheckBox m_AutoW;
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
    private System.Windows.Forms.Label m_KeyCount;
    private System.Windows.Forms.Label _About;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ProgressBar m_QPBar;
    private System.Windows.Forms.ProgressBar m_WPBar;
    private System.Windows.Forms.Button _SettingsButton;
    private System.Windows.Forms.CheckBox m_AutoMouse;
    private System.Windows.Forms.Label label2;
  }
}

