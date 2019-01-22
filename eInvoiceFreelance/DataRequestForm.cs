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
	public partial class DataRequestForm : Form
	{
		public struct RESULT
		{
			public bool     ok;
			public string   customer_name;
			public decimal  number;
			public DateTime date_time;
			public string   file_name;
		};

		private RESULT Result;
		private string Initial_Directory;

		public RESULT GetResult()
		{
			return (Result);
		}

		public DataRequestForm(string initial_directory)
		{
			Initial_Directory = initial_directory;

			InitializeComponent();
		}

		private void DataRequestForm_Load(object sender, EventArgs e)
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
			string         file_name;
			SaveFileDialog save_file_dialog;

			file_name = Result.customer_name + "-" + Result.date_time.ToString("yyyyMMdd") + "-" + Result.number.ToString("000000");

			save_file_dialog = new SaveFileDialog();
			save_file_dialog.InitialDirectory = Initial_Directory;
			save_file_dialog.FileName = file_name;
			save_file_dialog.Filter = "xml files (*.xml) | *.xml";
			save_file_dialog.Title = "Salvataggio file generato";

			if (save_file_dialog.ShowDialog() == DialogResult.OK)
			{
				Result.file_name = save_file_dialog.FileName;
				Result.ok = true;
			}
			else
			{
				Result.ok = false;
			}

			Close();
		}

		private void CancButton_Click(object sender, EventArgs e)
		{
			Result.ok = false;

			Close();
		}
	}
}
