namespace QCheck
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
      this._Grid = new System.Windows.Forms.DataGridView();
      this._TextCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this._PlayCol = new System.Windows.Forms.DataGridViewButtonColumn();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.panel1 = new System.Windows.Forms.Panel();
      this.m_NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
      this.m_ContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.m_AutoHideMenu = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.m_ShowMenu = new System.Windows.Forms.ToolStripMenuItem();
      this.m_HideMenu = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.m_CloseMenu = new System.Windows.Forms.ToolStripMenuItem();
      ((System.ComponentModel.ISupportInitialize)(this._Grid)).BeginInit();
      this.tableLayoutPanel1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.m_ContextMenu.SuspendLayout();
      this.SuspendLayout();
      // 
      // _Grid
      // 
      this._Grid.AllowUserToAddRows = false;
      this._Grid.AllowUserToDeleteRows = false;
      this._Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this._Grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._TextCol,
            this._PlayCol});
      this._Grid.Dock = System.Windows.Forms.DockStyle.Fill;
      this._Grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
      this._Grid.Location = new System.Drawing.Point(0, 0);
      this._Grid.MultiSelect = false;
      this._Grid.Name = "_Grid";
      this._Grid.ReadOnly = true;
      this._Grid.RowHeadersVisible = false;
      this._Grid.RowHeadersWidth = 45;
      this._Grid.RowTemplate.Height = 40;
      this._Grid.RowTemplate.ReadOnly = true;
      this._Grid.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      this._Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this._Grid.Size = new System.Drawing.Size(197, 440);
      this._Grid.TabIndex = 0;
      this._Grid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Grid_CellContentClick);
      // 
      // _TextCol
      // 
      this._TextCol.HeaderText = "File";
      this._TextCol.MinimumWidth = 6;
      this._TextCol.Name = "_TextCol";
      this._TextCol.ReadOnly = true;
      this._TextCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      this._TextCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this._TextCol.Width = 110;
      // 
      // _PlayCol
      // 
      this._PlayCol.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this._PlayCol.HeaderText = ">";
      this._PlayCol.MinimumWidth = 6;
      this._PlayCol.Name = "_PlayCol";
      this._PlayCol.ReadOnly = true;
      this._PlayCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      this._PlayCol.Width = 80;
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 1;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 1;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 857F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(203, 446);
      this.tableLayoutPanel1.TabIndex = 2;
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this._Grid);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point(3, 3);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(197, 440);
      this.panel1.TabIndex = 3;
      // 
      // m_NotifyIcon
      // 
      this.m_NotifyIcon.ContextMenuStrip = this.m_ContextMenu;
      this.m_NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("m_NotifyIcon.Icon")));
      this.m_NotifyIcon.Text = "QCheck";
      this.m_NotifyIcon.Visible = true;
      // 
      // m_ContextMenu
      // 
      this.m_ContextMenu.ImageScalingSize = new System.Drawing.Size(18, 18);
      this.m_ContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_AutoHideMenu,
            this.toolStripSeparator2,
            this.m_ShowMenu,
            this.m_HideMenu,
            this.toolStripSeparator1,
            this.m_CloseMenu});
      this.m_ContextMenu.Name = "m_ContextMenu";
      this.m_ContextMenu.Size = new System.Drawing.Size(135, 104);
      this.m_ContextMenu.Opened += new System.EventHandler(this.OnOpenedContextMenu);
      // 
      // m_AutoHideMenu
      // 
      this.m_AutoHideMenu.Name = "m_AutoHideMenu";
      this.m_AutoHideMenu.Size = new System.Drawing.Size(134, 22);
      this.m_AutoHideMenu.Text = "Auto Hide";
      this.m_AutoHideMenu.Click += new System.EventHandler(this.OnAutoHideClicked);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(131, 6);
      // 
      // m_ShowMenu
      // 
      this.m_ShowMenu.Name = "m_ShowMenu";
      this.m_ShowMenu.Size = new System.Drawing.Size(134, 22);
      this.m_ShowMenu.Text = "Show";
      // 
      // m_HideMenu
      // 
      this.m_HideMenu.Name = "m_HideMenu";
      this.m_HideMenu.Size = new System.Drawing.Size(134, 22);
      this.m_HideMenu.Text = "Hide";
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(131, 6);
      // 
      // m_CloseMenu
      // 
      this.m_CloseMenu.Name = "m_CloseMenu";
      this.m_CloseMenu.Size = new System.Drawing.Size(134, 22);
      this.m_CloseMenu.Text = "Close";
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(203, 446);
      this.Controls.Add(this.tableLayoutPanel1);
      this.MaximizeBox = false;
      this.Name = "Form1";
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "QCheck";
      this.TopMost = true;
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnClosed);
      this.Load += new System.EventHandler(this.OnLoad);
      ((System.ComponentModel.ISupportInitialize)(this._Grid)).EndInit();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.m_ContextMenu.ResumeLayout(false);
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView _Grid;
    private System.Windows.Forms.DataGridViewTextBoxColumn _TextCol;
    private System.Windows.Forms.DataGridViewButtonColumn _PlayCol;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.NotifyIcon m_NotifyIcon;
    private System.Windows.Forms.ContextMenuStrip m_ContextMenu;
    private System.Windows.Forms.ToolStripMenuItem m_ShowMenu;
    private System.Windows.Forms.ToolStripMenuItem m_HideMenu;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem m_CloseMenu;
    private System.Windows.Forms.ToolStripMenuItem m_AutoHideMenu;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
  }
}

