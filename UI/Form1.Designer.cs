namespace MXTools
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
      m_AutoQ = new System.Windows.Forms.CheckBox();
      m_AutoW = new System.Windows.Forms.CheckBox();
      m_ContextMenu = new System.Windows.Forms.ContextMenuStrip(components);
      m_StartMenu = new System.Windows.Forms.ToolStripMenuItem();
      toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
      m_ShowMenu = new System.Windows.Forms.ToolStripMenuItem();
      m_HideMenu = new System.Windows.Forms.ToolStripMenuItem();
      toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      m_CloseMenu = new System.Windows.Forms.ToolStripMenuItem();
      m_NotifyIcon = new System.Windows.Forms.NotifyIcon(components);
      m_CloseButton = new System.Windows.Forms.Button();
      m_KeyCount = new System.Windows.Forms.Label();
      m_EChk = new System.Windows.Forms.CheckBox();
      m_WChk = new System.Windows.Forms.CheckBox();
      m_QChk = new System.Windows.Forms.CheckBox();
      _About = new System.Windows.Forms.Label();
      _SettingsButton = new System.Windows.Forms.Button();
      m_AutoMouse = new System.Windows.Forms.CheckBox();
      _WarnTime = new System.Windows.Forms.Label();
      _HPBar = new System.Windows.Forms.ProgressBar();
      _ManaBar = new System.Windows.Forms.ProgressBar();
      m_ContextMenu.SuspendLayout();
      SuspendLayout();
      // 
      // m_ScanButton
      // 
      m_ScanButton.Image = Properties.Resources.scan_16;
      m_ScanButton.Location = new System.Drawing.Point(5, 236);
      m_ScanButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      m_ScanButton.Name = "m_ScanButton";
      m_ScanButton.Size = new System.Drawing.Size(67, 31);
      m_ScanButton.TabIndex = 0;
      m_ScanButton.UseVisualStyleBackColor = true;
      m_ScanButton.Click += OnScanClicked;
      // 
      // m_StartButton
      // 
      m_StartButton.Image = Properties.Resources.play_16;
      m_StartButton.Location = new System.Drawing.Point(74, 236);
      m_StartButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      m_StartButton.Name = "m_StartButton";
      m_StartButton.Size = new System.Drawing.Size(67, 31);
      m_StartButton.TabIndex = 3;
      m_StartButton.UseVisualStyleBackColor = true;
      m_StartButton.Click += OnStartButtonClicked;
      // 
      // m_AutoQ
      // 
      m_AutoQ.AutoSize = true;
      m_AutoQ.Checked = true;
      m_AutoQ.CheckState = System.Windows.Forms.CheckState.Checked;
      m_AutoQ.Location = new System.Drawing.Point(5, 12);
      m_AutoQ.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      m_AutoQ.Name = "m_AutoQ";
      m_AutoQ.Size = new System.Drawing.Size(112, 21);
      m_AutoQ.TabIndex = 6;
      m_AutoQ.Text = "Auto Check HP";
      m_AutoQ.UseVisualStyleBackColor = true;
      m_AutoQ.CheckedChanged += OnAutoQCheckedChanged;
      // 
      // m_AutoW
      // 
      m_AutoW.AutoSize = true;
      m_AutoW.Checked = true;
      m_AutoW.CheckState = System.Windows.Forms.CheckState.Checked;
      m_AutoW.Location = new System.Drawing.Point(5, 48);
      m_AutoW.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      m_AutoW.Name = "m_AutoW";
      m_AutoW.Size = new System.Drawing.Size(129, 21);
      m_AutoW.TabIndex = 6;
      m_AutoW.Text = "Auto Check Mana";
      m_AutoW.UseVisualStyleBackColor = true;
      m_AutoW.CheckedChanged += OnAutoWCheckedChanged;
      // 
      // m_ContextMenu
      // 
      m_ContextMenu.ImageScalingSize = new System.Drawing.Size(18, 18);
      m_ContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { m_StartMenu, toolStripSeparator3, m_ShowMenu, m_HideMenu, toolStripSeparator2, m_CloseMenu });
      m_ContextMenu.Name = "m_ContextMenu";
      m_ContextMenu.Size = new System.Drawing.Size(108, 104);
      m_ContextMenu.Opened += m_ContextMenu_Opened;
      // 
      // m_StartMenu
      // 
      m_StartMenu.Name = "m_StartMenu";
      m_StartMenu.Size = new System.Drawing.Size(107, 22);
      m_StartMenu.Text = "Start";
      // 
      // toolStripSeparator3
      // 
      toolStripSeparator3.Name = "toolStripSeparator3";
      toolStripSeparator3.Size = new System.Drawing.Size(104, 6);
      // 
      // m_ShowMenu
      // 
      m_ShowMenu.Name = "m_ShowMenu";
      m_ShowMenu.Size = new System.Drawing.Size(107, 22);
      m_ShowMenu.Text = "Show";
      // 
      // m_HideMenu
      // 
      m_HideMenu.Name = "m_HideMenu";
      m_HideMenu.Size = new System.Drawing.Size(107, 22);
      m_HideMenu.Text = "Hide";
      // 
      // toolStripSeparator2
      // 
      toolStripSeparator2.Name = "toolStripSeparator2";
      toolStripSeparator2.Size = new System.Drawing.Size(104, 6);
      // 
      // m_CloseMenu
      // 
      m_CloseMenu.Name = "m_CloseMenu";
      m_CloseMenu.Size = new System.Drawing.Size(107, 22);
      m_CloseMenu.Text = "Quit";
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
      m_CloseButton.Image = Properties.Resources.quit_16;
      m_CloseButton.Location = new System.Drawing.Point(74, 273);
      m_CloseButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      m_CloseButton.Name = "m_CloseButton";
      m_CloseButton.Size = new System.Drawing.Size(67, 31);
      m_CloseButton.TabIndex = 3;
      m_CloseButton.UseVisualStyleBackColor = true;
      m_CloseButton.Click += m_CloseButton_Click;
      // 
      // m_KeyCount
      // 
      m_KeyCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
      m_KeyCount.Location = new System.Drawing.Point(5, 117);
      m_KeyCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      m_KeyCount.Name = "m_KeyCount";
      m_KeyCount.Size = new System.Drawing.Size(136, 39);
      m_KeyCount.TabIndex = 3;
      m_KeyCount.Text = "...";
      m_KeyCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // m_EChk
      // 
      m_EChk.AutoSize = true;
      m_EChk.Location = new System.Drawing.Point(107, 90);
      m_EChk.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      m_EChk.Name = "m_EChk";
      m_EChk.Size = new System.Drawing.Size(34, 21);
      m_EChk.TabIndex = 1;
      m_EChk.Text = "E";
      m_EChk.UseVisualStyleBackColor = true;
      m_EChk.CheckStateChanged += OnEChkChanged;
      // 
      // m_WChk
      // 
      m_WChk.AutoSize = true;
      m_WChk.Location = new System.Drawing.Point(55, 90);
      m_WChk.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      m_WChk.Name = "m_WChk";
      m_WChk.Size = new System.Drawing.Size(39, 21);
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
      m_QChk.Location = new System.Drawing.Point(5, 90);
      m_QChk.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      m_QChk.Name = "m_QChk";
      m_QChk.Size = new System.Drawing.Size(37, 21);
      m_QChk.TabIndex = 1;
      m_QChk.Text = "Q";
      m_QChk.UseVisualStyleBackColor = true;
      m_QChk.CheckedChanged += OnQChkChanged;
      // 
      // _About
      // 
      _About.AutoSize = true;
      _About.ForeColor = System.Drawing.SystemColors.ControlDark;
      _About.Location = new System.Drawing.Point(5, 313);
      _About.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      _About.Name = "_About";
      _About.Size = new System.Drawing.Size(14, 17);
      _About.TabIndex = 8;
      _About.Text = "..";
      // 
      // _SettingsButton
      // 
      _SettingsButton.Image = Properties.Resources.settings_16;
      _SettingsButton.Location = new System.Drawing.Point(5, 273);
      _SettingsButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      _SettingsButton.Name = "_SettingsButton";
      _SettingsButton.Size = new System.Drawing.Size(67, 31);
      _SettingsButton.TabIndex = 9;
      _SettingsButton.UseVisualStyleBackColor = true;
      _SettingsButton.Click += _SettingsButton_Click_1;
      // 
      // m_AutoMouse
      // 
      m_AutoMouse.AutoSize = true;
      m_AutoMouse.Checked = true;
      m_AutoMouse.CheckState = System.Windows.Forms.CheckState.Checked;
      m_AutoMouse.Location = new System.Drawing.Point(5, 166);
      m_AutoMouse.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      m_AutoMouse.Name = "m_AutoMouse";
      m_AutoMouse.Size = new System.Drawing.Size(132, 21);
      m_AutoMouse.TabIndex = 10;
      m_AutoMouse.Text = "Right Click (Ctrl-A)";
      m_AutoMouse.UseVisualStyleBackColor = true;
      m_AutoMouse.CheckedChanged += m_AutoMouse_CheckedChanged;
      // 
      // _WarnTime
      // 
      _WarnTime.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
      _WarnTime.ForeColor = System.Drawing.Color.Red;
      _WarnTime.Location = new System.Drawing.Point(5, 187);
      _WarnTime.Name = "_WarnTime";
      _WarnTime.Size = new System.Drawing.Size(136, 39);
      _WarnTime.TabIndex = 11;
      _WarnTime.Text = "...";
      _WarnTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // _HPBar
      // 
      _HPBar.Location = new System.Drawing.Point(5, 31);
      _HPBar.Name = "_HPBar";
      _HPBar.Size = new System.Drawing.Size(136, 8);
      _HPBar.TabIndex = 12;
      // 
      // _ManaBar
      // 
      _ManaBar.Location = new System.Drawing.Point(5, 67);
      _ManaBar.Name = "_ManaBar";
      _ManaBar.Size = new System.Drawing.Size(136, 8);
      _ManaBar.TabIndex = 12;
      // 
      // Form1
      // 
      AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      ClientSize = new System.Drawing.Size(148, 348);
      Controls.Add(_ManaBar);
      Controls.Add(_HPBar);
      Controls.Add(m_KeyCount);
      Controls.Add(m_AutoQ);
      Controls.Add(m_EChk);
      Controls.Add(m_AutoW);
      Controls.Add(m_WChk);
      Controls.Add(m_QChk);
      Controls.Add(_WarnTime);
      Controls.Add(m_AutoMouse);
      Controls.Add(_SettingsButton);
      Controls.Add(_About);
      Controls.Add(m_ScanButton);
      Controls.Add(m_CloseButton);
      Controls.Add(m_StartButton);
      FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
      Location = new System.Drawing.Point(2, 320);
      Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      MaximizeBox = false;
      Name = "Form1";
      SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      Text = "MXTools";
      TopMost = true;
      FormClosing += OnFormClosing;
      FormClosed += OnFormClosed;
      Load += OnFormLoad;
      m_ContextMenu.ResumeLayout(false);
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Button m_ScanButton;
    private System.Windows.Forms.Button m_StartButton;
    private System.Windows.Forms.CheckBox m_AutoQ;
    private System.Windows.Forms.CheckBox m_AutoW;
    private System.Windows.Forms.ContextMenuStrip m_ContextMenu;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripMenuItem m_ShowMenu;
    private System.Windows.Forms.ToolStripMenuItem m_HideMenu;
    private System.Windows.Forms.ToolStripMenuItem m_CloseMenu;
    private System.Windows.Forms.NotifyIcon m_NotifyIcon;
    private System.Windows.Forms.ToolStripMenuItem m_StartMenu;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.Button m_CloseButton;
    private System.Windows.Forms.CheckBox m_EChk;
    private System.Windows.Forms.CheckBox m_WChk;
    private System.Windows.Forms.CheckBox m_QChk;
    private System.Windows.Forms.Label m_KeyCount;
    private System.Windows.Forms.Label _About;
    private System.Windows.Forms.Button _SettingsButton;
    private System.Windows.Forms.CheckBox m_AutoMouse;
    private System.Windows.Forms.Label _WarnTime;
    private System.Windows.Forms.ProgressBar _HPBar;
    private System.Windows.Forms.ProgressBar _ManaBar;
  }
}

