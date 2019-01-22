namespace eInvoiceFreelance
{
	partial class DataRequestForm
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
			this.InvoiceCustomerNameTextBox = new CueTextBox();
			this.InvoiceNumberTextBox = new CueTextBox();
			this.SuspendLayout();
			// 
			// InvoiceDateTimePicker
			// 
			this.InvoiceDateTimePicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.InvoiceDateTimePicker.Location = new System.Drawing.Point(18, 100);
			this.InvoiceDateTimePicker.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.InvoiceDateTimePicker.Name = "InvoiceDateTimePicker";
			this.InvoiceDateTimePicker.Size = new System.Drawing.Size(284, 24);
			this.InvoiceDateTimePicker.TabIndex = 3;
			this.InvoiceDateTimePicker.ValueChanged += new System.EventHandler(this.ControlsValues_Changed);
			// 
			// OkButton
			// 
			this.OkButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.OkButton.Location = new System.Drawing.Point(18, 141);
			this.OkButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.OkButton.Name = "OkButton";
			this.OkButton.Size = new System.Drawing.Size(112, 37);
			this.OkButton.TabIndex = 4;
			this.OkButton.Text = "Ok";
			this.OkButton.UseVisualStyleBackColor = true;
			this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
			// 
			// CancButton
			// 
			this.CancButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CancButton.Location = new System.Drawing.Point(192, 141);
			this.CancButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.CancButton.Name = "CancButton";
			this.CancButton.Size = new System.Drawing.Size(112, 37);
			this.CancButton.TabIndex = 5;
			this.CancButton.Text = "Anulla";
			this.CancButton.UseVisualStyleBackColor = true;
			this.CancButton.Click += new System.EventHandler(this.CancButton_Click);
			// 
			// InvoiceCustomerNameTextBox
			// 
			this.InvoiceCustomerNameTextBox.CueText = "Nome Cliente";
			this.InvoiceCustomerNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.InvoiceCustomerNameTextBox.Location = new System.Drawing.Point(18, 17);
			this.InvoiceCustomerNameTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.InvoiceCustomerNameTextBox.Name = "InvoiceCustomerNameTextBox";
			this.InvoiceCustomerNameTextBox.Size = new System.Drawing.Size(284, 24);
			this.InvoiceCustomerNameTextBox.TabIndex = 1;
			this.InvoiceCustomerNameTextBox.TextChanged += new System.EventHandler(this.ControlsValues_Changed);
			// 
			// InvoiceNumberTextBox
			// 
			this.InvoiceNumberTextBox.CueText = "Numero Fattura";
			this.InvoiceNumberTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.InvoiceNumberTextBox.Location = new System.Drawing.Point(18, 58);
			this.InvoiceNumberTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.InvoiceNumberTextBox.Name = "InvoiceNumberTextBox";
			this.InvoiceNumberTextBox.Size = new System.Drawing.Size(284, 24);
			this.InvoiceNumberTextBox.TabIndex = 2;
			this.InvoiceNumberTextBox.TextChanged += new System.EventHandler(this.ControlsValues_Changed);
			this.InvoiceNumberTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.InvoceNumberTextBox_KeyPress);
			// 
			// DataRequestForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(322, 197);
			this.Controls.Add(this.CancButton);
			this.Controls.Add(this.InvoiceCustomerNameTextBox);
			this.Controls.Add(this.OkButton);
			this.Controls.Add(this.InvoiceDateTimePicker);
			this.Controls.Add(this.InvoiceNumberTextBox);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.MaximumSize = new System.Drawing.Size(338, 236);
			this.MinimumSize = new System.Drawing.Size(338, 236);
			this.Name = "DataRequestForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Informazioni";
			this.Load += new System.EventHandler(this.DataRequestForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private CueTextBox InvoiceNumberTextBox;
		private System.Windows.Forms.DateTimePicker InvoiceDateTimePicker;
		private System.Windows.Forms.Button OkButton;
		private CueTextBox InvoiceCustomerNameTextBox;
		private System.Windows.Forms.Button CancButton;
	}
}