namespace AutoCheck
{
  partial class ScanForm
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
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.panel1 = new System.Windows.Forms.Panel();
      this.button3 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.button1 = new System.Windows.Forms.Button();
      this.m_ReScanButton = new System.Windows.Forms.Button();
      this.m_ScanButton = new System.Windows.Forms.Button();
      this.m_NumberBox = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.m_Grid = new System.Windows.Forms.DataGridView();
      this.m_AddressCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.m_ValueCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.m_CheckCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
      this.panel2 = new System.Windows.Forms.Panel();
      this.m_CancelButton = new System.Windows.Forms.Button();
      this.m_OKButton = new System.Windows.Forms.Button();
      this.tableLayoutPanel1.SuspendLayout();
      this.panel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.m_Grid)).BeginInit();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 1;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.m_Grid, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 2);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 3;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(604, 564);
      this.tableLayoutPanel1.TabIndex = 0;
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.button3);
      this.panel1.Controls.Add(this.button2);
      this.panel1.Controls.Add(this.button1);
      this.panel1.Controls.Add(this.m_ReScanButton);
      this.panel1.Controls.Add(this.m_ScanButton);
      this.panel1.Controls.Add(this.m_NumberBox);
      this.panel1.Controls.Add(this.label1);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point(3, 3);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(598, 54);
      this.panel1.TabIndex = 0;
      // 
      // button3
      // 
      this.button3.Location = new System.Drawing.Point(514, 9);
      this.button3.Name = "button3";
      this.button3.Size = new System.Drawing.Size(75, 23);
      this.button3.TabIndex = 3;
      this.button3.Text = "Auto";
      this.button3.UseVisualStyleBackColor = true;
      this.button3.Click += new System.EventHandler(this.OnAuto);
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(404, 9);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(75, 23);
      this.button2.TabIndex = 2;
      this.button2.Text = "Un-Filter";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(323, 9);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 2;
      this.button1.Text = "Filter";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // m_ReScanButton
      // 
      this.m_ReScanButton.Location = new System.Drawing.Point(242, 9);
      this.m_ReScanButton.Name = "m_ReScanButton";
      this.m_ReScanButton.Size = new System.Drawing.Size(75, 23);
      this.m_ReScanButton.TabIndex = 2;
      this.m_ReScanButton.Text = "Re-Scan";
      this.m_ReScanButton.UseVisualStyleBackColor = true;
      this.m_ReScanButton.Click += new System.EventHandler(this.m_ReScanButton_Click);
      // 
      // m_ScanButton
      // 
      this.m_ScanButton.Location = new System.Drawing.Point(161, 9);
      this.m_ScanButton.Name = "m_ScanButton";
      this.m_ScanButton.Size = new System.Drawing.Size(75, 23);
      this.m_ScanButton.TabIndex = 2;
      this.m_ScanButton.Text = "Scan";
      this.m_ScanButton.UseVisualStyleBackColor = true;
      this.m_ScanButton.Click += new System.EventHandler(this.m_ScanButton_Click);
      // 
      // m_NumberBox
      // 
      this.m_NumberBox.Location = new System.Drawing.Point(52, 10);
      this.m_NumberBox.Name = "m_NumberBox";
      this.m_NumberBox.Size = new System.Drawing.Size(100, 20);
      this.m_NumberBox.TabIndex = 1;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 14);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(44, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Number";
      // 
      // m_Grid
      // 
      this.m_Grid.AllowUserToAddRows = false;
      this.m_Grid.AllowUserToDeleteRows = false;
      this.m_Grid.AllowUserToOrderColumns = true;
      this.m_Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.m_Grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_AddressCol,
            this.m_ValueCol,
            this.m_CheckCol});
      this.m_Grid.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_Grid.Location = new System.Drawing.Point(3, 63);
      this.m_Grid.Name = "m_Grid";
      this.m_Grid.Size = new System.Drawing.Size(598, 438);
      this.m_Grid.TabIndex = 1;
      // 
      // m_AddressCol
      // 
      this.m_AddressCol.HeaderText = "Address";
      this.m_AddressCol.Name = "m_AddressCol";
      this.m_AddressCol.ReadOnly = true;
      // 
      // m_ValueCol
      // 
      this.m_ValueCol.HeaderText = "Value";
      this.m_ValueCol.Name = "m_ValueCol";
      this.m_ValueCol.ReadOnly = true;
      // 
      // m_CheckCol
      // 
      this.m_CheckCol.HeaderText = "Select";
      this.m_CheckCol.Name = "m_CheckCol";
      // 
      // panel2
      // 
      this.panel2.Controls.Add(this.m_CancelButton);
      this.panel2.Controls.Add(this.m_OKButton);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel2.Location = new System.Drawing.Point(3, 507);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(598, 54);
      this.panel2.TabIndex = 2;
      // 
      // m_CancelButton
      // 
      this.m_CancelButton.Location = new System.Drawing.Point(303, 18);
      this.m_CancelButton.Name = "m_CancelButton";
      this.m_CancelButton.Size = new System.Drawing.Size(75, 23);
      this.m_CancelButton.TabIndex = 1;
      this.m_CancelButton.Text = "Cancel";
      this.m_CancelButton.UseVisualStyleBackColor = true;
      this.m_CancelButton.Click += new System.EventHandler(this.m_CancelButton_Click);
      // 
      // m_OKButton
      // 
      this.m_OKButton.Location = new System.Drawing.Point(221, 18);
      this.m_OKButton.Name = "m_OKButton";
      this.m_OKButton.Size = new System.Drawing.Size(75, 23);
      this.m_OKButton.TabIndex = 0;
      this.m_OKButton.Text = "OK";
      this.m_OKButton.UseVisualStyleBackColor = true;
      this.m_OKButton.Click += new System.EventHandler(this.m_OKButton_Click);
      // 
      // ScanForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(604, 564);
      this.Controls.Add(this.tableLayoutPanel1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.Name = "ScanForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "Scan";
      this.TopMost = true;
      this.Load += new System.EventHandler(this.ScanForm_Load);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.m_Grid)).EndInit();
      this.panel2.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button m_ScanButton;
    private System.Windows.Forms.TextBox m_NumberBox;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.DataGridView m_Grid;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Button m_CancelButton;
    private System.Windows.Forms.Button m_OKButton;
    private System.Windows.Forms.Button m_ReScanButton;
    private System.Windows.Forms.DataGridViewTextBoxColumn m_AddressCol;
    private System.Windows.Forms.DataGridViewTextBoxColumn m_ValueCol;
    private System.Windows.Forms.DataGridViewCheckBoxColumn m_CheckCol;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Button button3;
  }
}