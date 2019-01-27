namespace eInvoiceFreelance
{
	partial class InformationForm
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
			this.InvoiceDateTimePicker = new System.Windows.Forms.DateTimePicker();
			this.OkButton = new System.Windows.Forms.Button();
			this.CancButton = new System.Windows.Forms.Button();
			this.InvoiceCustomerNameTextBox = new System.Windows.Forms.TextBox();
			this.InvoiceNumberTextBox = new System.Windows.Forms.TextBox();
			this.DataRequestTabelLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.NamelLabel = new System.Windows.Forms.Label();
			this.NumberLabel = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.DataRequestTabelLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			//
			// InvoiceDateTimePicker
			//
			this.InvoiceDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.DataRequestTabelLayoutPanel.SetColumnSpan(this.InvoiceDateTimePicker, 3);
			this.InvoiceDateTimePicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.InvoiceDateTimePicker.Location = new System.Drawing.Point(126, 85);
			this.InvoiceDateTimePicker.Margin = new System.Windows.Forms.Padding(4);
			this.InvoiceDateTimePicker.Name = "InvoiceDateTimePicker";
			this.InvoiceDateTimePicker.Size = new System.Drawing.Size(177, 24);
			this.InvoiceDateTimePicker.TabIndex = 3;
			this.InvoiceDateTimePicker.ValueChanged += new System.EventHandler(this.ControlsValues_Changed);
			this.InvoiceDateTimePicker.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Controls_KeyDown);
			//
			// OkButton
			//
			this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.DataRequestTabelLayoutPanel.SetColumnSpan(this.OkButton, 2);
			this.OkButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.OkButton.Location = new System.Drawing.Point(4, 146);
			this.OkButton.Margin = new System.Windows.Forms.Padding(4);
			this.OkButton.Name = "OkButton";
			this.OkButton.Size = new System.Drawing.Size(114, 27);
			this.OkButton.TabIndex = 4;
			this.OkButton.Text = "Ok";
			this.OkButton.UseVisualStyleBackColor = true;
			this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
			//
			// CancButton
			//
			this.CancButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.DataRequestTabelLayoutPanel.SetColumnSpan(this.CancButton, 2);
			this.CancButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CancButton.Location = new System.Drawing.Point(187, 146);
			this.CancButton.Margin = new System.Windows.Forms.Padding(4);
			this.CancButton.Name = "CancButton";
			this.CancButton.Size = new System.Drawing.Size(116, 27);
			this.CancButton.TabIndex = 5;
			this.CancButton.Text = "Anulla";
			this.CancButton.UseVisualStyleBackColor = true;
			this.CancButton.Click += new System.EventHandler(this.CancButton_Click);
			//
			// InvoiceCustomerNameTextBox
			//
			this.InvoiceCustomerNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.DataRequestTabelLayoutPanel.SetColumnSpan(this.InvoiceCustomerNameTextBox, 3);
			this.InvoiceCustomerNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.InvoiceCustomerNameTextBox.Location = new System.Drawing.Point(126, 7);
			this.InvoiceCustomerNameTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.InvoiceCustomerNameTextBox.Name = "InvoiceCustomerNameTextBox";
			this.InvoiceCustomerNameTextBox.Size = new System.Drawing.Size(177, 24);
			this.InvoiceCustomerNameTextBox.TabIndex = 1;
			this.InvoiceCustomerNameTextBox.TextChanged += new System.EventHandler(this.ControlsValues_Changed);
			this.InvoiceCustomerNameTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Controls_KeyDown);
			//
			// InvoiceNumberTextBox
			//
			this.InvoiceNumberTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.DataRequestTabelLayoutPanel.SetColumnSpan(this.InvoiceNumberTextBox, 3);
			this.InvoiceNumberTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.InvoiceNumberTextBox.Location = new System.Drawing.Point(126, 46);
			this.InvoiceNumberTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.InvoiceNumberTextBox.Name = "InvoiceNumberTextBox";
			this.InvoiceNumberTextBox.Size = new System.Drawing.Size(177, 24);
			this.InvoiceNumberTextBox.TabIndex = 2;
			this.InvoiceNumberTextBox.TextChanged += new System.EventHandler(this.ControlsValues_Changed);
			this.InvoiceNumberTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Controls_KeyDown);
			this.InvoiceNumberTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.InvoceNumberTextBox_KeyPress);
			//
			// DataRequestTabelLayoutPanel
			//
			this.DataRequestTabelLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			                                                                                 | System.Windows.Forms.AnchorStyles.Left)
			                                                                                | System.Windows.Forms.AnchorStyles.Right)));
			this.DataRequestTabelLayoutPanel.ColumnCount = 5;
			this.DataRequestTabelLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.DataRequestTabelLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.DataRequestTabelLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.DataRequestTabelLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.DataRequestTabelLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.DataRequestTabelLayoutPanel.Controls.Add(this.InvoiceCustomerNameTextBox, 2, 0);
			this.DataRequestTabelLayoutPanel.Controls.Add(this.OkButton, 0, 4);
			this.DataRequestTabelLayoutPanel.Controls.Add(this.CancButton, 3, 4);
			this.DataRequestTabelLayoutPanel.Controls.Add(this.InvoiceNumberTextBox, 2, 1);
			this.DataRequestTabelLayoutPanel.Controls.Add(this.InvoiceDateTimePicker, 2, 2);
			this.DataRequestTabelLayoutPanel.Controls.Add(this.NamelLabel, 0, 0);
			this.DataRequestTabelLayoutPanel.Controls.Add(this.NumberLabel, 0, 1);
			this.DataRequestTabelLayoutPanel.Controls.Add(this.label2, 0, 2);
			this.DataRequestTabelLayoutPanel.Location = new System.Drawing.Point(12, 12);
			this.DataRequestTabelLayoutPanel.Name = "DataRequestTabelLayoutPanel";
			this.DataRequestTabelLayoutPanel.RowCount = 5;
			this.DataRequestTabelLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22F));
			this.DataRequestTabelLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22F));
			this.DataRequestTabelLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22F));
			this.DataRequestTabelLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14F));
			this.DataRequestTabelLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.DataRequestTabelLayoutPanel.Size = new System.Drawing.Size(307, 178);
			this.DataRequestTabelLayoutPanel.TabIndex = 6;
			//
			// NamelLabel
			//
			this.NamelLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.NamelLabel.AutoSize = true;
			this.DataRequestTabelLayoutPanel.SetColumnSpan(this.NamelLabel, 2);
			this.NamelLabel.Location = new System.Drawing.Point(3, 10);
			this.NamelLabel.Name = "NamelLabel";
			this.NamelLabel.Size = new System.Drawing.Size(116, 18);
			this.NamelLabel.TabIndex = 6;
			this.NamelLabel.Text = "Nome cliente";
			//
			// NumberLabel
			//
			this.NumberLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.NumberLabel.AutoSize = true;
			this.DataRequestTabelLayoutPanel.SetColumnSpan(this.NumberLabel, 2);
			this.NumberLabel.Location = new System.Drawing.Point(3, 49);
			this.NumberLabel.Name = "NumberLabel";
			this.NumberLabel.Size = new System.Drawing.Size(116, 18);
			this.NumberLabel.TabIndex = 7;
			this.NumberLabel.Text = "Numero fattura";
			//
			// label2
			//
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.DataRequestTabelLayoutPanel.SetColumnSpan(this.label2, 2);
			this.label2.Location = new System.Drawing.Point(3, 88);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(116, 18);
			this.label2.TabIndex = 8;
			this.label2.Text = "Data fattura";
			//
			// InformationForm
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(331, 202);
			this.Controls.Add(this.DataRequestTabelLayoutPanel);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MinimumSize = new System.Drawing.Size(347, 241);
			this.Name = "InformationForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Informazioni";
			this.Load += new System.EventHandler(this.InformationForm_Load);
			this.DataRequestTabelLayoutPanel.ResumeLayout(false);
			this.DataRequestTabelLayoutPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.TextBox InvoiceNumberTextBox;
		private System.Windows.Forms.DateTimePicker InvoiceDateTimePicker;
		private System.Windows.Forms.Button OkButton;
		private System.Windows.Forms.TextBox InvoiceCustomerNameTextBox;
		private System.Windows.Forms.Button CancButton;
		private System.Windows.Forms.TableLayoutPanel DataRequestTabelLayoutPanel;
		private System.Windows.Forms.Label NamelLabel;
		private System.Windows.Forms.Label NumberLabel;
		private System.Windows.Forms.Label label2;
	}
}