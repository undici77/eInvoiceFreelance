namespace eInvoiceFreelance
{
	partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.InvoiceGridView = new System.Windows.Forms.DataGridView();
			this.InvoiceDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.InvoiceQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.InvoiceUnitPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.InvoiceTotalPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.InvoiceReimbursement = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.InvoiceVat = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.StripMenu = new System.Windows.Forms.MenuStrip();
			this.ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.OpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.CloseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.GenerateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.SummariesGridView = new System.Windows.Forms.DataGridView();
			this.SummaryData = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SummaryCurrency = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SummaryValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.InvoiceGridView)).BeginInit();
			this.StripMenu.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.SummariesGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// InvoiceGridView
			// 
			this.InvoiceGridView.AllowUserToAddRows = false;
			this.InvoiceGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.InvoiceGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.InvoiceGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.InvoiceDescription,
            this.InvoiceQuantity,
            this.InvoiceUnitPrice,
            this.InvoiceTotalPrice,
            this.InvoiceReimbursement,
            this.InvoiceVat});
			this.InvoiceGridView.EnableHeadersVisualStyles = false;
			this.InvoiceGridView.Location = new System.Drawing.Point(12, 27);
			this.InvoiceGridView.Name = "InvoiceGridView";
			this.InvoiceGridView.RowHeadersVisible = false;
			this.InvoiceGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.InvoiceGridView.Size = new System.Drawing.Size(860, 244);
			this.InvoiceGridView.TabIndex = 0;
			this.InvoiceGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.InvoiceGridViewCellContentClick);
			this.InvoiceGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.InvoiceGridViewCellEndEdit);
			this.InvoiceGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.InvoiceGridViewCellValueChanged);
			// 
			// InvoiceDescription
			// 
			this.InvoiceDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.InvoiceDescription.FillWeight = 30F;
			this.InvoiceDescription.HeaderText = "Description";
			this.InvoiceDescription.MinimumWidth = 300;
			this.InvoiceDescription.Name = "InvoiceDescription";
			// 
			// InvoiceQuantity
			// 
			this.InvoiceQuantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.InvoiceQuantity.FillWeight = 10F;
			this.InvoiceQuantity.HeaderText = "Quantity";
			this.InvoiceQuantity.MinimumWidth = 30;
			this.InvoiceQuantity.Name = "InvoiceQuantity";
			// 
			// InvoiceUnitPrice
			// 
			this.InvoiceUnitPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.InvoiceUnitPrice.FillWeight = 20F;
			this.InvoiceUnitPrice.HeaderText = "Unit price";
			this.InvoiceUnitPrice.MinimumWidth = 100;
			this.InvoiceUnitPrice.Name = "InvoiceUnitPrice";
			// 
			// InvoiceTotalPrice
			// 
			this.InvoiceTotalPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.InvoiceTotalPrice.FillWeight = 20F;
			this.InvoiceTotalPrice.HeaderText = "Total price";
			this.InvoiceTotalPrice.MinimumWidth = 100;
			this.InvoiceTotalPrice.Name = "InvoiceTotalPrice";
			// 
			// InvoiceReimbursement
			// 
			this.InvoiceReimbursement.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.InvoiceReimbursement.FillWeight = 10F;
			this.InvoiceReimbursement.HeaderText = "Reimb.";
			this.InvoiceReimbursement.MinimumWidth = 60;
			this.InvoiceReimbursement.Name = "InvoiceReimbursement";
			// 
			// InvoiceVat
			// 
			this.InvoiceVat.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.InvoiceVat.FillWeight = 10F;
			this.InvoiceVat.HeaderText = "Vat";
			this.InvoiceVat.MinimumWidth = 60;
			this.InvoiceVat.Name = "InvoiceVat";
			// 
			// StripMenu
			// 
			this.StripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem,
            this.GenerateToolStripMenuItem});
			this.StripMenu.Location = new System.Drawing.Point(0, 0);
			this.StripMenu.Name = "StripMenu";
			this.StripMenu.Size = new System.Drawing.Size(884, 24);
			this.StripMenu.TabIndex = 1;
			this.StripMenu.Text = "StripMenu";
			// 
			// ToolStripMenuItem
			// 
			this.ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenToolStripMenuItem,
            this.CloseToolStripMenuItem});
			this.ToolStripMenuItem.Name = "ToolStripMenuItem";
			this.ToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.ToolStripMenuItem.Text = "File";
			// 
			// OpenToolStripMenuItem
			// 
			this.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem";
			this.OpenToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.OpenToolStripMenuItem.Text = "Open";
			this.OpenToolStripMenuItem.Click += new System.EventHandler(this.StripMenuFileOpenClick);
			// 
			// CloseToolStripMenuItem
			// 
			this.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem";
			this.CloseToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.CloseToolStripMenuItem.Text = "Close";
			this.CloseToolStripMenuItem.Click += new System.EventHandler(this.StripMenuFileCloseClick);
			// 
			// GenerateToolStripMenuItem
			// 
			this.GenerateToolStripMenuItem.Name = "GenerateToolStripMenuItem";
			this.GenerateToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
			this.GenerateToolStripMenuItem.Text = "Generate";
			this.GenerateToolStripMenuItem.Click += new System.EventHandler(this.StripMenuGenerateClick);
			// 
			// SummariesGridView
			// 
			this.SummariesGridView.AllowUserToAddRows = false;
			this.SummariesGridView.AllowUserToDeleteRows = false;
			this.SummariesGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.SummariesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.SummariesGridView.ColumnHeadersVisible = false;
			this.SummariesGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SummaryData,
            this.SummaryCurrency,
            this.SummaryValue});
			this.SummariesGridView.Location = new System.Drawing.Point(13, 277);
			this.SummariesGridView.Name = "SummariesGridView";
			this.SummariesGridView.ReadOnly = true;
			this.SummariesGridView.RowHeadersVisible = false;
			this.SummariesGridView.Size = new System.Drawing.Size(859, 172);
			this.SummariesGridView.TabIndex = 2;
			// 
			// SummaryData
			// 
			this.SummaryData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.SummaryData.FillWeight = 60F;
			this.SummaryData.HeaderText = "SummaryData";
			this.SummaryData.Name = "SummaryData";
			this.SummaryData.ReadOnly = true;
			// 
			// SummaryCurrency
			// 
			this.SummaryCurrency.FillWeight = 10F;
			this.SummaryCurrency.HeaderText = "SummaryCurrency";
			this.SummaryCurrency.MinimumWidth = 30;
			this.SummaryCurrency.Name = "SummaryCurrency";
			this.SummaryCurrency.ReadOnly = true;
			this.SummaryCurrency.Width = 30;
			// 
			// SummaryValue
			// 
			this.SummaryValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.SummaryValue.FillWeight = 40F;
			this.SummaryValue.HeaderText = "SummaryValue";
			this.SummaryValue.Name = "SummaryValue";
			this.SummaryValue.ReadOnly = true;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(884, 461);
			this.Controls.Add(this.SummariesGridView);
			this.Controls.Add(this.InvoiceGridView);
			this.Controls.Add(this.StripMenu);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.StripMenu;
			this.MinimumSize = new System.Drawing.Size(900, 500);
			this.Name = "MainForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
			this.Load += new System.EventHandler(this.MainFormLoad);
			((System.ComponentModel.ISupportInitialize)(this.InvoiceGridView)).EndInit();
			this.StripMenu.ResumeLayout(false);
			this.StripMenu.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.SummariesGridView)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView InvoiceGridView;
		private System.Windows.Forms.MenuStrip StripMenu;
		private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem OpenToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem CloseToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem GenerateToolStripMenuItem;
		private System.Windows.Forms.DataGridViewTextBoxColumn InvoiceDescription;
		private System.Windows.Forms.DataGridViewTextBoxColumn InvoiceQuantity;
		private System.Windows.Forms.DataGridViewCheckBoxColumn InvoiceReimbursement;
		private System.Windows.Forms.DataGridViewTextBoxColumn InvoiceVat;
		private System.Windows.Forms.DataGridView SummariesGridView;
		private System.Windows.Forms.DataGridViewTextBoxColumn SummaryData;
		private System.Windows.Forms.DataGridViewTextBoxColumn SummaryCurrency;
		private System.Windows.Forms.DataGridViewTextBoxColumn SummaryValue;
		private System.Windows.Forms.DataGridViewTextBoxColumn InvoiceUnitPrice;
		private System.Windows.Forms.DataGridViewTextBoxColumn InvoiceTotalPrice;
	}
}

