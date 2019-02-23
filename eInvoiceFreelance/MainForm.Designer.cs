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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
			this.StripMenu = new System.Windows.Forms.MenuStrip();
			this.OpenStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.AddStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.RemoveStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.GenerateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.XmlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.XmlInvoiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.PdfToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.PdfInvoiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.PdfProformaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.SummariesGridView = new System.Windows.Forms.DataGridView();
			this.SummaryData = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SummaryValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DonatePictureBox = new System.Windows.Forms.PictureBox();
			this.MainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.InvoiceGridView = new DataGridViewEx();
			this.InvoiceDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.InvoiceQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.InvoiceUnitPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.InvoiceTotalPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.InvoiceReimbursement = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.InvoiceVat = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.StripMenu.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.SummariesGridView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.DonatePictureBox)).BeginInit();
			this.MainTableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.InvoiceGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// StripMenu
			// 
			this.StripMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.StripMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.StripMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.StripMenu.Dock = System.Windows.Forms.DockStyle.None;
			this.StripMenu.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.StripMenu.GripMargin = new System.Windows.Forms.Padding(0, 0, -4, 0);
			this.StripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenStripMenuItem,
            this.AddStripMenuItem,
            this.RemoveStripMenuItem,
            this.GenerateToolStripMenuItem,
            this.AboutToolStripMenuItem});
			this.StripMenu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.StripMenu.Location = new System.Drawing.Point(12, 4);
			this.StripMenu.Name = "StripMenu";
			this.StripMenu.Padding = new System.Windows.Forms.Padding(0);
			this.StripMenu.Size = new System.Drawing.Size(470, 24);
			this.StripMenu.TabIndex = 1;
			this.StripMenu.Text = "StripMenu";
			// 
			// OpenStripMenuItem
			// 
			this.OpenStripMenuItem.Name = "OpenStripMenuItem";
			this.OpenStripMenuItem.Size = new System.Drawing.Size(49, 24);
			this.OpenStripMenuItem.Text = "Apri";
			this.OpenStripMenuItem.Click += new System.EventHandler(this.OpenStripMenuItem_Click);
			// 
			// AddStripMenuItem
			// 
			this.AddStripMenuItem.Name = "AddStripMenuItem";
			this.AddStripMenuItem.Size = new System.Drawing.Size(82, 24);
			this.AddStripMenuItem.Text = "Aggiungi";
			this.AddStripMenuItem.Click += new System.EventHandler(this.AddStripMenuItem_Click);
			// 
			// RemoveStripMenuItem
			// 
			this.RemoveStripMenuItem.Name = "RemoveStripMenuItem";
			this.RemoveStripMenuItem.Size = new System.Drawing.Size(75, 24);
			this.RemoveStripMenuItem.Text = "Rimuovi";
			this.RemoveStripMenuItem.Click += new System.EventHandler(this.RemoveStripMenuItem_Click);
			// 
			// GenerateToolStripMenuItem
			// 
			this.GenerateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.XmlToolStripMenuItem,
            this.PdfToolStripMenuItem});
			this.GenerateToolStripMenuItem.Name = "GenerateToolStripMenuItem";
			this.GenerateToolStripMenuItem.Size = new System.Drawing.Size(68, 24);
			this.GenerateToolStripMenuItem.Text = "Genera";
			// 
			// XmlToolStripMenuItem
			// 
			this.XmlToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.XmlInvoiceToolStripMenuItem});
			this.XmlToolStripMenuItem.Name = "XmlToolStripMenuItem";
			this.XmlToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
			this.XmlToolStripMenuItem.Text = "Xml";
			// 
			// XmlInvoiceToolStripMenuItem
			// 
			this.XmlInvoiceToolStripMenuItem.Name = "XmlInvoiceToolStripMenuItem";
			this.XmlInvoiceToolStripMenuItem.Size = new System.Drawing.Size(123, 24);
			this.XmlInvoiceToolStripMenuItem.Text = "Fattura";
			this.XmlInvoiceToolStripMenuItem.Click += new System.EventHandler(this.XmlInvoiceToolStripMenuItem_Click);
			// 
			// PdfToolStripMenuItem
			// 
			this.PdfToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PdfInvoiceToolStripMenuItem,
            this.PdfProformaToolStripMenuItem});
			this.PdfToolStripMenuItem.Name = "PdfToolStripMenuItem";
			this.PdfToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
			this.PdfToolStripMenuItem.Text = "Pdf";
			// 
			// PdfInvoiceToolStripMenuItem
			// 
			this.PdfInvoiceToolStripMenuItem.Name = "PdfInvoiceToolStripMenuItem";
			this.PdfInvoiceToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
			this.PdfInvoiceToolStripMenuItem.Text = "Fattua";
			this.PdfInvoiceToolStripMenuItem.Click += new System.EventHandler(this.PdfInvoiceToolStripMenuItem_Click);
			// 
			// PdfProformaToolStripMenuItem
			// 
			this.PdfProformaToolStripMenuItem.Name = "PdfProformaToolStripMenuItem";
			this.PdfProformaToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
			this.PdfProformaToolStripMenuItem.Text = "Proforma";
			this.PdfProformaToolStripMenuItem.Click += new System.EventHandler(this.PdfProformaToolStripMenuItem_Click);
			// 
			// AboutToolStripMenuItem
			// 
			this.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
			this.AboutToolStripMenuItem.Size = new System.Drawing.Size(105, 24);
			this.AboutToolStripMenuItem.Text = "Informazioni";
			this.AboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
			// 
			// SummariesGridView
			// 
			this.SummariesGridView.AllowUserToAddRows = false;
			this.SummariesGridView.AllowUserToDeleteRows = false;
			this.SummariesGridView.AllowUserToResizeColumns = false;
			this.SummariesGridView.AllowUserToResizeRows = false;
			this.SummariesGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.SummariesGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle12;
			this.SummariesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.SummariesGridView.ColumnHeadersVisible = false;
			this.SummariesGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SummaryData,
            this.SummaryValue});
			dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.SummariesGridView.DefaultCellStyle = dataGridViewCellStyle14;
			this.SummariesGridView.Location = new System.Drawing.Point(3, 249);
			this.SummariesGridView.MultiSelect = false;
			this.SummariesGridView.Name = "SummariesGridView";
			this.SummariesGridView.ReadOnly = true;
			dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle15.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.SummariesGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle15;
			this.SummariesGridView.RowHeadersVisible = false;
			this.SummariesGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.SummariesGridView.ShowCellErrors = false;
			this.SummariesGridView.ShowCellToolTips = false;
			this.SummariesGridView.ShowEditingIcon = false;
			this.SummariesGridView.ShowRowErrors = false;
			this.SummariesGridView.Size = new System.Drawing.Size(706, 166);
			this.SummariesGridView.TabIndex = 2;
			this.SummariesGridView.SelectionChanged += new System.EventHandler(this.SummariesGridView_SelectionChanged);
			// 
			// SummaryData
			// 
			this.SummaryData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.SummaryData.FillWeight = 60F;
			this.SummaryData.HeaderText = "SummaryData";
			this.SummaryData.Name = "SummaryData";
			this.SummaryData.ReadOnly = true;
			// 
			// SummaryValue
			// 
			this.SummaryValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle13.Format = "€ 0.00";
			dataGridViewCellStyle13.NullValue = "€ 0.00";
			this.SummaryValue.DefaultCellStyle = dataGridViewCellStyle13;
			this.SummaryValue.FillWeight = 40F;
			this.SummaryValue.HeaderText = "SummaryValue";
			this.SummaryValue.Name = "SummaryValue";
			this.SummaryValue.ReadOnly = true;
			// 
			// DonatePictureBox
			// 
			this.DonatePictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.DonatePictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DonatePictureBox.Image = ((System.Drawing.Image)(resources.GetObject("DonatePictureBox.Image")));
			this.DonatePictureBox.InitialImage = null;
			this.DonatePictureBox.Location = new System.Drawing.Point(715, 249);
			this.DonatePictureBox.Name = "DonatePictureBox";
			this.DonatePictureBox.Size = new System.Drawing.Size(342, 166);
			this.DonatePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.DonatePictureBox.TabIndex = 3;
			this.DonatePictureBox.TabStop = false;
			this.DonatePictureBox.Click += new System.EventHandler(this.DonatePictureBox_Click);
			// 
			// MainTableLayoutPanel
			// 
			this.MainTableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.MainTableLayoutPanel.ColumnCount = 2;
			this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 348F));
			this.MainTableLayoutPanel.Controls.Add(this.DonatePictureBox, 1, 1);
			this.MainTableLayoutPanel.Controls.Add(this.InvoiceGridView, 0, 0);
			this.MainTableLayoutPanel.Controls.Add(this.SummariesGridView, 0, 1);
			this.MainTableLayoutPanel.Location = new System.Drawing.Point(12, 31);
			this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
			this.MainTableLayoutPanel.RowCount = 2;
			this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 172F));
			this.MainTableLayoutPanel.Size = new System.Drawing.Size(1060, 418);
			this.MainTableLayoutPanel.TabIndex = 4;
			// 
			// InvoiceGridView
			// 
			this.InvoiceGridView.AllowUserToAddRows = false;
			this.InvoiceGridView.AllowUserToResizeRows = false;
			this.InvoiceGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle16.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.InvoiceGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle16;
			this.InvoiceGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.InvoiceGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.InvoiceDescription,
            this.InvoiceQuantity,
            this.InvoiceUnitPrice,
            this.InvoiceTotalPrice,
            this.InvoiceReimbursement,
            this.InvoiceVat});
			this.MainTableLayoutPanel.SetColumnSpan(this.InvoiceGridView, 2);
			dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle21.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle21.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle21.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle21.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle21.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.InvoiceGridView.DefaultCellStyle = dataGridViewCellStyle21;
			this.InvoiceGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.InvoiceGridView.EnableHeadersVisualStyles = false;
			this.InvoiceGridView.EnterTabPressed = false;
			this.InvoiceGridView.Location = new System.Drawing.Point(3, 3);
			this.InvoiceGridView.Name = "InvoiceGridView";
			dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle22.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle22.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle22.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle22.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle22.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle22.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.InvoiceGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle22;
			this.InvoiceGridView.RowHeadersVisible = false;
			this.InvoiceGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.InvoiceGridView.ShowCellErrors = false;
			this.InvoiceGridView.ShowCellToolTips = false;
			this.InvoiceGridView.ShowEditingIcon = false;
			this.InvoiceGridView.ShowRowErrors = false;
			this.InvoiceGridView.Size = new System.Drawing.Size(1054, 240);
			this.InvoiceGridView.TabIndex = 0;
			this.InvoiceGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.InvoiceGridView_CellContentClick);
			this.InvoiceGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.InvoiceGridView_CellDoubleClick);
			this.InvoiceGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.InvoiceGridView_CellEndEdit);
			this.InvoiceGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.InvoiceGridView_CellFormatting);
			this.InvoiceGridView.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.InvoiceGridView_CellMouseClick);
			this.InvoiceGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.InvoiceGridView_CellValueChanged);
			this.InvoiceGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.InvoiceGridView_CurrentCellDirtyStateChanged);
			this.InvoiceGridView.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.InvoiceGridView_EditingControlShowing);
			this.InvoiceGridView.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.InvoiceGridView_RowsAdded);
			this.InvoiceGridView.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.InvoiceGridView_RowsRemoved);
			this.InvoiceGridView.Leave += new System.EventHandler(this.InvoiceGridView_Leave);
			// 
			// InvoiceDescription
			// 
			this.InvoiceDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.InvoiceDescription.FillWeight = 30F;
			this.InvoiceDescription.HeaderText = "Descrizione";
			this.InvoiceDescription.MinimumWidth = 300;
			this.InvoiceDescription.Name = "InvoiceDescription";
			// 
			// InvoiceQuantity
			// 
			this.InvoiceQuantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle17.Format = "0.00";
			dataGridViewCellStyle17.NullValue = "0.00";
			this.InvoiceQuantity.DefaultCellStyle = dataGridViewCellStyle17;
			this.InvoiceQuantity.FillWeight = 10F;
			this.InvoiceQuantity.HeaderText = "Quantità";
			this.InvoiceQuantity.MinimumWidth = 30;
			this.InvoiceQuantity.Name = "InvoiceQuantity";
			// 
			// InvoiceUnitPrice
			// 
			this.InvoiceUnitPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle18.Format = "€ 0.00";
			dataGridViewCellStyle18.NullValue = "€ 0.00";
			this.InvoiceUnitPrice.DefaultCellStyle = dataGridViewCellStyle18;
			this.InvoiceUnitPrice.FillWeight = 20F;
			this.InvoiceUnitPrice.HeaderText = "Prezzo unitario";
			this.InvoiceUnitPrice.MinimumWidth = 100;
			this.InvoiceUnitPrice.Name = "InvoiceUnitPrice";
			// 
			// InvoiceTotalPrice
			// 
			this.InvoiceTotalPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle19.Format = "€ 0.00";
			dataGridViewCellStyle19.NullValue = "€ 0.00";
			this.InvoiceTotalPrice.DefaultCellStyle = dataGridViewCellStyle19;
			this.InvoiceTotalPrice.FillWeight = 20F;
			this.InvoiceTotalPrice.HeaderText = "Prezzo totale";
			this.InvoiceTotalPrice.MinimumWidth = 100;
			this.InvoiceTotalPrice.Name = "InvoiceTotalPrice";
			// 
			// InvoiceReimbursement
			// 
			this.InvoiceReimbursement.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.InvoiceReimbursement.FillWeight = 15F;
			this.InvoiceReimbursement.HeaderText = "Cont. Prev.";
			this.InvoiceReimbursement.MinimumWidth = 60;
			this.InvoiceReimbursement.Name = "InvoiceReimbursement";
			// 
			// InvoiceVat
			// 
			this.InvoiceVat.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle20.Format = "0.00 \\%";
			this.InvoiceVat.DefaultCellStyle = dataGridViewCellStyle20;
			this.InvoiceVat.FillWeight = 10F;
			this.InvoiceVat.HeaderText = "IVA";
			this.InvoiceVat.MinimumWidth = 60;
			this.InvoiceVat.Name = "InvoiceVat";
			this.InvoiceVat.ReadOnly = true;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.ClientSize = new System.Drawing.Size(1084, 461);
			this.Controls.Add(this.MainTableLayoutPanel);
			this.Controls.Add(this.StripMenu);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.StripMenu;
			this.MinimumSize = new System.Drawing.Size(1100, 500);
			this.Name = "MainForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.StripMenu.ResumeLayout(false);
			this.StripMenu.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.SummariesGridView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.DonatePictureBox)).EndInit();
			this.MainTableLayoutPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.InvoiceGridView)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private DataGridViewEx InvoiceGridView;
		private System.Windows.Forms.MenuStrip StripMenu;
		private System.Windows.Forms.ToolStripMenuItem GenerateToolStripMenuItem;
		private System.Windows.Forms.DataGridView SummariesGridView;
		private System.Windows.Forms.ToolStripMenuItem OpenStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem AddStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem RemoveStripMenuItem;
		private System.Windows.Forms.DataGridViewTextBoxColumn SummaryData;
		private System.Windows.Forms.DataGridViewTextBoxColumn SummaryValue;
		private System.Windows.Forms.ToolStripMenuItem AboutToolStripMenuItem;
		private System.Windows.Forms.PictureBox DonatePictureBox;
		private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
		private System.Windows.Forms.DataGridViewTextBoxColumn InvoiceDescription;
		private System.Windows.Forms.DataGridViewTextBoxColumn InvoiceQuantity;
		private System.Windows.Forms.DataGridViewTextBoxColumn InvoiceUnitPrice;
		private System.Windows.Forms.DataGridViewTextBoxColumn InvoiceTotalPrice;
		private System.Windows.Forms.DataGridViewCheckBoxColumn InvoiceReimbursement;
		private System.Windows.Forms.DataGridViewTextBoxColumn InvoiceVat;
		private System.Windows.Forms.ToolStripMenuItem XmlToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem XmlInvoiceToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem PdfToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem PdfInvoiceToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem PdfProformaToolStripMenuItem;
	}
}

