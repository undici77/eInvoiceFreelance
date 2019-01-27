using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eInvoiceFreelance
{
	public partial class InformationForm : Form
	{
		public struct RESULT
		{
			public bool     ok;
			public string   customer_name;
			public decimal  number;
			public DateTime date_time;
		};

		private RESULT Result;
		public RESULT GetResult()
		{
			return (Result);
		}

		public InformationForm()
		{
			InitializeComponent();
		}

		private void InformationForm_Load(object sender, EventArgs e)
		{
			OkButton.Enabled = false;
		}

		private void InvoceNumberTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
			{
				e.Handled = true;
			}
		}

		private void ControlsValues_Changed(object sender, EventArgs e)
		{
			try
			{
				Result.customer_name = InvoiceCustomerNameTextBox.Text;
				Result.number = decimal.Parse(InvoiceNumberTextBox.Text);
				Result.date_time = InvoiceDateTimePicker.Value;

				OkButton.Enabled = (!string.IsNullOrEmpty(Result.customer_name) && (Result.number > 0));
			}
			catch
			{
				OkButton.Enabled = false;
			}
		}

		private void OkButton_Click(object sender, EventArgs e)
		{
			Result.ok = true;

			Close();
		}

		private void CancButton_Click(object sender, EventArgs e)
		{
			Result.ok = false;

			Close();
		}

		private void Controls_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Enter)
			{
				if (ActiveControl == InvoiceCustomerNameTextBox)
				{
					ActiveControl = InvoiceNumberTextBox;
				}
				else if (ActiveControl == InvoiceNumberTextBox)
				{
					ActiveControl = InvoiceDateTimePicker;
				}
				else if (ActiveControl ==  InvoiceDateTimePicker)
				{
					ActiveControl = OkButton;
				}
			}
		}
	}
}
