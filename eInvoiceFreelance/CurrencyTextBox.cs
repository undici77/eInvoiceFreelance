using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace eInvoiceFreelance
{
	class CurrencyTextBox : TextBox
	{
		private char _Currency_Char;

		public char CurrencyChar
		{
			get
			{
				return (_Currency_Char);
			}
		}

		public string Currency
		{
			get
			{
				return (Format(Text));
			}

			set
			{
				Text = 	Format(value);				
			}
		}

		public decimal Decimal
		{
			get
			{
				try
				{
					return (decimal.Parse(Text.Replace("" + _Currency_Char, "")));
				}
				catch
				{
					return(0);
				}
			}
		}

		public CurrencyTextBox(char currency_char)
		{
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CurrencyTextBoxKeyPress);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CurrencyTextBoxKeyDown);
			this.Leave += new System.EventHandler(this.CurrencyTextBoxKeyLeave);

			_Currency_Char = currency_char;
		}

		public CurrencyTextBox()
		{
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CurrencyTextBoxKeyPress);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CurrencyTextBoxKeyDown);
			this.Leave += new System.EventHandler(this.CurrencyTextBoxKeyLeave);

			_Currency_Char = '€';
		}

		private void CurrencyTextBoxKeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
			    (e.KeyChar != '.'))
			{
				e.Handled = true;
			}

			// only allow one decimal point
			if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
			{
				e.Handled = true;
			}
		}

		private string Format(string value)
		{
			Double numeric_value;
			if (Double.TryParse(value.Trim(_Currency_Char), out numeric_value))
			{
				return( String.Format("{0} {1:F2}", _Currency_Char, numeric_value));
			}
			else
			{
				return(String.Empty);
			}
		}

		private void CurrencyTextBoxKeyLeave(object sender, EventArgs e)
		{
			Text = Format(Text);
		}

		private void CurrencyTextBoxKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Enter)
			{
				Text = Format(Text);
			}
		}


	}
}
