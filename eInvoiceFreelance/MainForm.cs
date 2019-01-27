using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using eInvoiceFreelance.Serializer;

namespace eInvoiceFreelance
{
	public partial class MainForm : Form
	{
		private struct ACTIVITY_FIELD
		{
			public string  description;
			public decimal quantity;
			public decimal unit_price;
			public decimal total_price;
			public bool    reimbursement;
		}

		private enum INVOICE_GRID_VIEW_COLUMN_ID
		{
			DESCRIPTION_ID,
			QUANTITY_ID,
			UNIT_PRICE_ID,
			TOTAL_PRICE_ID,
			REIMBOURSE_ID,
			VAT_ID
		};

		private enum SUMMARY_GRID_VIEW_COLUMN_ID
		{
			DATA_ID,
			VALUE_ID,
		};

		private enum SUMMARY_GRID_VIEW_ROW_ID
		{
			TAXABLE_ID,
			REIMBURSMENT_ID,
			TOTAL_TAXABLE_ID,
			TOTAL_VAT_ID,
			TOTAL_ID,
			WITHHOLDING_TAX_ID,
			TO_PAY_ID
		};

		private struct SUMMARY
		{
			public decimal taxable;
			public decimal reimbursment;
			public decimal total_taxable;
			public decimal total_vat;
			public decimal total;
			public decimal withholding_tax;
			public decimal to_pay;
		}

		private struct BANK_ACCOUNT
		{
			public string beneficiary;
			public string financial_institute;
			public string iban;
			public string abi;
			public string cab;
			public string bic;
		};

		private BANK_ACCOUNT Bank_Account;
		private decimal Vat;
		private decimal Reimbursment;
		private string Reimbursment_Description;
		private string Reimbursment_String;
		private decimal Withholding_Tax;
		private string Save_Directory;

		private List<ACTIVITY_FIELD> Activity_List;
		private SUMMARY Summary;
		private object[] Invoice_Grid_View_Default_Value;
		private IniFile Ini_File;
		private string Template_File_Name;

		public MainForm()
		{
			InitializeComponent();

			this.Text = App.Name + " " + App.Version;

			Activity_List = new List<ACTIVITY_FIELD>();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			string description;
			decimal quantity;
			decimal unit_price;
			decimal total_price;
			OpenFileDialog open_file_dialog;
			AboutBox about;
			string disclaimer_signature;
			bool disclaimer_accepted;

			Ini_File = new IniFile();
			Ini_File.Load(App.Name + ".ini");

			description = Ini_File.GetKeyValue("Init", "Description");
			try
			{
				quantity = decimal.Parse(Ini_File.GetKeyValue("Init", "Quantity"));
			}
			catch
			{
				quantity = 1;
			}
			try
			{
				unit_price = decimal.Parse(Ini_File.GetKeyValue("Init", "UnitPrice"));
			}
			catch
			{
				unit_price = 0;
			}
			try
			{
				Vat = decimal.Parse(Ini_File.GetKeyValue("Init", "Vat"));
			}
			catch
			{
				Vat = 22;
			}

			total_price = quantity * unit_price;

			Invoice_Grid_View_Default_Value = new object[] { description, quantity.ToString(), unit_price.ToString(), total_price.ToString(), true, Vat };

			Ini_File.SetKeyValue("Init", "Description", description);
			Ini_File.SetKeyValue("Init", "Quantity", quantity.ToString());
			Ini_File.SetKeyValue("Init", "UnitPrice", unit_price.ToString());
			Ini_File.SetKeyValue("Init", "Vat", Vat.ToString());

			try
			{
				Reimbursment = decimal.Parse(Ini_File.GetKeyValue("Conf", "Reimbursment"));
			}
			catch
			{
				Reimbursment = 4;
				Ini_File.SetKeyValue("Conf", "Reimbursment", Reimbursment.ToString());
			}


			Reimbursment_Description = Ini_File.GetKeyValue("Conf", "ReimbursmentDescrition");
			if (string.IsNullOrEmpty(Reimbursment_Description))
			{
				Reimbursment_Description = "Cont. Prev.";
			}
			Ini_File.SetKeyValue("Conf", "ReimbursmentDescrition", Reimbursment_Description);

			Reimbursment_String = " (" + Reimbursment_Description + " " + Reimbursment.ToString() + "%)";

			try
			{
				Withholding_Tax = decimal.Parse(Ini_File.GetKeyValue("Conf", "WithholdingTax"));
			}
			catch
			{
				Withholding_Tax = 20;
				Ini_File.SetKeyValue("Conf", "WithholdingTax", Withholding_Tax.ToString());
			}

			Template_File_Name = Ini_File.GetKeyValue("Conf", "Template");
			if (string.IsNullOrEmpty(Template_File_Name))
			{
				Template_File_Name = "Template.xml";
			}

			if (!File.Exists(Template_File_Name))
			{
				open_file_dialog = new OpenFileDialog();
				open_file_dialog.InitialDirectory = App.Path;
				open_file_dialog.Filter = "xml files (*.xml)|*.xml";
				open_file_dialog.RestoreDirectory = true;

				if (open_file_dialog.ShowDialog() == DialogResult.OK)
				{
					Template_File_Name = open_file_dialog.FileName;
				}
				else
				{
					Application.Exit();
					return;
				}
			}

			Save_Directory = Ini_File.GetKeyValue("Conf", "SaveDirectory");

			Ini_File.SetKeyValue("Conf", "Template", Template_File_Name);

			InvoiceGridView.Columns[(int)INVOICE_GRID_VIEW_COLUMN_ID.REIMBOURSE_ID].HeaderText += " " + Reimbursment.ToString("0.00") + "%";

			InvoiceGridView.Columns[(int)INVOICE_GRID_VIEW_COLUMN_ID.DESCRIPTION_ID].ValueType = typeof(string);
			InvoiceGridView.Columns[(int)INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID].ValueType    = typeof(string);
			InvoiceGridView.Columns[(int)INVOICE_GRID_VIEW_COLUMN_ID.UNIT_PRICE_ID].ValueType  = typeof(string);
			InvoiceGridView.Columns[(int)INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID].ValueType = typeof(string);
			InvoiceGridView.Columns[(int)INVOICE_GRID_VIEW_COLUMN_ID.REIMBOURSE_ID].ValueType  = typeof(bool);
			if (App.IsUnix)
			{
				InvoiceGridView.Columns[(int)INVOICE_GRID_VIEW_COLUMN_ID.REIMBOURSE_ID].ReadOnly = true;
			}
			InvoiceGridView.Columns[(int)INVOICE_GRID_VIEW_COLUMN_ID.VAT_ID].ValueType         = typeof(decimal);

			SummaryValue.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

			SummariesGridView.Columns[(int)SUMMARY_GRID_VIEW_COLUMN_ID.DATA_ID].ValueType  = typeof(string);
			SummariesGridView.Columns[(int)SUMMARY_GRID_VIEW_COLUMN_ID.VALUE_ID].ValueType = typeof(decimal);

			SummariesGridView.Rows.Add("Importo", 0);
			SummariesGridView.Rows.Add("Contributo Previdenziale " + Reimbursment.ToString("0.00") + "%", 0);
			SummariesGridView.Rows.Add("Totale Imponibile", 0);
			SummariesGridView.Rows.Add("Totale IVA", 0);
			SummariesGridView.Rows.Add("Totale", 0);
			SummariesGridView.Rows.Add("Ritenuta d'acconto " + Withholding_Tax.ToString("0.00") + "%", 0);
			SummariesGridView.Rows.Add("Totale", 0);

			Bank_Account.beneficiary = Ini_File.GetKeyValue("BankAccount", "Beneficiary");
			Bank_Account.financial_institute = Ini_File.GetKeyValue("BankAccount", "FinancialInstitute");
			Bank_Account.iban = Ini_File.GetKeyValue("BankAccount", "IBAN");
			Bank_Account.abi = Ini_File.GetKeyValue("BankAccount", "ABI");
			Bank_Account.cab = Ini_File.GetKeyValue("BankAccount", "CAB");
			Bank_Account.bic = Ini_File.GetKeyValue("BankAccount", "BIC");

			if (string.IsNullOrEmpty(Bank_Account.beneficiary))
			{
				Bank_Account.beneficiary = string.Empty;
			}
			if (string.IsNullOrEmpty(Bank_Account.financial_institute))
			{
				Bank_Account.beneficiary = string.Empty;
			}
			if (string.IsNullOrEmpty(Bank_Account.iban))
			{
				Bank_Account.iban = string.Empty;
			}
			if (string.IsNullOrEmpty(Bank_Account.abi))
			{
				Bank_Account.abi = string.Empty;
			}
			if (string.IsNullOrEmpty(Bank_Account.cab))
			{
				Bank_Account.cab = string.Empty;
			}
			if (string.IsNullOrEmpty(Bank_Account.bic))
			{
				Bank_Account.bic = string.Empty;
			}

			disclaimer_signature = (App.Name + " " + App.Version);
			disclaimer_accepted = (Ini_File.GetKeyValue("Disclaimer", "Accepted") ==  disclaimer_signature);
			Ini_File.SetKeyValue("Disclaimer", "Accepted", disclaimer_signature);

			if (!disclaimer_accepted)
			{
				about = new AboutBox(false);
				about.ShowDialog(this);
				if (!about.Result)
				{
					Application.Exit();
					return;
				}

				System.Diagnostics.Process.Start("http://einvoicefreelance.altervista.org");
			}

			Ini_File.Save(App.Name + ".ini");

			UpdateSummaryDataGridView();
			UpdateSaveButton();
		}


		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			InvoiceGridView.CancelEdit();
			InvoiceGridView.Rows.Clear();
		}

		private void SummariesGridView_SelectionChanged(object sender, EventArgs e)
		{
			SummariesGridView.ClearSelection();
		}

		private void InvoiceDataGridViewRowAdd(object[] row)
		{
			ACTIVITY_FIELD field;

			field.description   = (string)row[(int)INVOICE_GRID_VIEW_COLUMN_ID.DESCRIPTION_ID];
			field.quantity      = decimal.Parse((string)row[(int)INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID]);
			field.unit_price    = decimal.Parse(((string)row[(int)INVOICE_GRID_VIEW_COLUMN_ID.UNIT_PRICE_ID]).Replace("€ ", ""));
			field.total_price   = decimal.Parse(((string)row[(int)INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID]).Replace("€ ", ""));
			field.reimbursement = (bool)row[(int)INVOICE_GRID_VIEW_COLUMN_ID.REIMBOURSE_ID];

			Activity_List.Add(field);
			InvoiceGridView.Rows.Add(row);
		}

		private void InvoiceDataGridViewRowAdd(string description, decimal quantity, decimal unit_price, decimal total_price, bool reimbursement)
		{
			object[]       row;
			ACTIVITY_FIELD field;

			row = new object[] { description, quantity.ToString(), unit_price.ToString(), total_price.ToString(), reimbursement, Vat };

			field.description   = description;
			field.quantity      = quantity;
			field.unit_price    = unit_price;
			field.total_price   = total_price;
			field.reimbursement = reimbursement;

			Activity_List.Add(field);
			InvoiceGridView.Rows.Add(row);
		}

		private bool ValidateDescriptionCell(DataGridViewRow row)
		{
			string description;

			try
			{
				description = row.Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.DESCRIPTION_ID].Value.ToString();
				return (!string.IsNullOrEmpty(description));
			}
			catch
			{
			}

			return (false);
		}

		private bool ValidateQuantityCell(DataGridViewRow row)
		{
			decimal quantity;

			try
			{
				quantity = decimal.Parse(row.Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID].Value.ToString());
				return ((quantity > 0));
			}
			catch
			{
			}

			return (false);
		}

		private bool ValidateUnitPriceCell(DataGridViewRow row)
		{
			decimal unit_price;

			try
			{
				unit_price = decimal.Parse(row.Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.UNIT_PRICE_ID].Value.ToString().Replace("€ ", ""));
				return ((unit_price != 0));
			}
			catch
			{
			}

			return (false);
		}

		private bool ValidateTotalPriceCell(DataGridViewRow row)
		{
			decimal total_price;

			try
			{
				total_price = decimal.Parse(row.Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID].Value.ToString().Replace("€ ", ""));
				return ((total_price != 0));
			}
			catch
			{
			}

			return (false);
		}

		private bool ValidateVatCell(DataGridViewRow row)
		{
			decimal vat;

			try
			{
				vat = decimal.Parse(row.Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.VAT_ID].Value.ToString());
				return ((vat > 0));
			}
			catch
			{
			}

			return (false);
		}

		private void UpdateSaveButton()
		{
			DataGridViewRow row;
			int row_id;
			bool enable;

			row_id = 0;
			enable = (InvoiceGridView.RowCount > 0);

			while (enable && (row_id < InvoiceGridView.RowCount))
			{
				row = InvoiceGridView.Rows[row_id];
				if (!ValidateDescriptionCell(row))
				{
					enable = false;
				}
				else if (!ValidateQuantityCell(row))
				{
					enable = false;
				}
				else if (!ValidateUnitPriceCell(row))
				{
					enable = false;
				}
				else if (!ValidateTotalPriceCell(row))
				{
					enable = false;
				}
				else if (!ValidateVatCell(row))
				{
					enable = false;
				}

				row_id++;
			}

			GenerateToolStripMenuItem.Enabled = enable;
		}

		private void UpdateValueBackColor(INVOICE_GRID_VIEW_COLUMN_ID col_id, DataGridViewRow row)
		{
			switch (col_id)
			{
				case INVOICE_GRID_VIEW_COLUMN_ID.DESCRIPTION_ID:
					if (ValidateDescriptionCell(row))
					{
						row.Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.DESCRIPTION_ID].Style.BackColor = Color.White;
					}
					else
					{
						row.Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.DESCRIPTION_ID].Style.BackColor = Color.Red;
					}
					break;

				case INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID:
					if (ValidateQuantityCell(row))
					{
						row.Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID].Style.BackColor = Color.White;
					}
					else
					{
						row.Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID].Style.BackColor = Color.Red;
					}
					break;

				case INVOICE_GRID_VIEW_COLUMN_ID.UNIT_PRICE_ID:
					if (ValidateUnitPriceCell(row))
					{
						row.Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.UNIT_PRICE_ID].Style.BackColor = Color.White;
					}
					else
					{
						row.Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.UNIT_PRICE_ID].Style.BackColor = Color.Red;
					}
					break;

				case INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID:
					if (ValidateTotalPriceCell(row))
					{
						row.Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID].Style.BackColor = Color.White;
					}
					else
					{
						row.Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID].Style.BackColor = Color.Red;
					}
					break;

				case INVOICE_GRID_VIEW_COLUMN_ID.VAT_ID:
					if (ValidateVatCell(row))
					{
						row.Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.VAT_ID].Style.BackColor = Color.White;
					}
					else
					{
						row.Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.VAT_ID].Style.BackColor = Color.Red;
					}
					break;
			}
		}

		private void UpdateSummaryDataGridView()
		{
			decimal reimbursment;
			decimal vat;

			Summary.taxable = 0;
			Summary.reimbursment = 0;
			Summary.total_taxable = 0;
			Summary.total_vat = 0;
			Summary.total = 0;
			Summary.withholding_tax = 0;
			Summary.to_pay = 0;

			try
			{
				foreach (ACTIVITY_FIELD field in Activity_List)
				{
					Summary.taxable += field.total_price;

					reimbursment = 0;
					if (field.reimbursement)
					{
						reimbursment = ((field.total_price * Reimbursment) / 100);
					}

					Summary.reimbursment += reimbursment;

					vat = (((field.total_price + reimbursment) * Vat) / 100);

					Summary.total_vat += vat;
				}

				Summary.total_taxable = Summary.taxable + Summary.reimbursment;
				Summary.total = Summary.total_taxable + Summary.total_vat;
				Summary.withholding_tax = ((Summary.total_taxable * Withholding_Tax) / 100);
				Summary.to_pay = Summary.total - Summary.withholding_tax;
			}
			catch
			{
			}

			try
			{
				SummariesGridView.Rows[(int)SUMMARY_GRID_VIEW_ROW_ID.TAXABLE_ID].Cells[(int)SUMMARY_GRID_VIEW_COLUMN_ID.VALUE_ID].Value = Summary.taxable;
				SummariesGridView.Rows[(int)SUMMARY_GRID_VIEW_ROW_ID.REIMBURSMENT_ID].Cells[(int)SUMMARY_GRID_VIEW_COLUMN_ID.VALUE_ID].Value = Summary.reimbursment;
				SummariesGridView.Rows[(int)SUMMARY_GRID_VIEW_ROW_ID.TOTAL_TAXABLE_ID].Cells[(int)SUMMARY_GRID_VIEW_COLUMN_ID.VALUE_ID].Value = Summary.total_taxable;
				SummariesGridView.Rows[(int)SUMMARY_GRID_VIEW_ROW_ID.TOTAL_VAT_ID].Cells[(int)SUMMARY_GRID_VIEW_COLUMN_ID.VALUE_ID].Value = Summary.total_vat;
				SummariesGridView.Rows[(int)SUMMARY_GRID_VIEW_ROW_ID.TOTAL_ID].Cells[(int)SUMMARY_GRID_VIEW_COLUMN_ID.VALUE_ID].Value = Summary.total;
				SummariesGridView.Rows[(int)SUMMARY_GRID_VIEW_ROW_ID.WITHHOLDING_TAX_ID].Cells[(int)SUMMARY_GRID_VIEW_COLUMN_ID.VALUE_ID].Value = Summary.withholding_tax;
				SummariesGridView.Rows[(int)SUMMARY_GRID_VIEW_ROW_ID.TO_PAY_ID].Cells[(int)SUMMARY_GRID_VIEW_COLUMN_ID.VALUE_ID].Value = Summary.to_pay;
			}
			catch
			{
			}
		}

		private void GenerateBillList(out List<DettaglioLineeType> bill_list)
		{
			DettaglioLineeType bill_filed;

			int line_number;

			bill_list = new List<DettaglioLineeType>();

			line_number = 1;
			foreach (ACTIVITY_FIELD activity_field in Activity_List)
			{
				bill_filed = new DettaglioLineeType();
				bill_filed.NumeroLinea       = line_number.ToString();
				bill_filed.Descrizione       = activity_field.description;
				bill_filed.QuantitaSpecified = true;
				bill_filed.Quantita          = activity_field.quantity;
				bill_filed.PrezzoUnitario    = activity_field.unit_price;
				bill_filed.PrezzoTotale      = activity_field.total_price;
				bill_filed.AliquotaIVA       = Vat;

				bill_list.Add(bill_filed);
				line_number++;

				if (activity_field.reimbursement)
				{
					bill_filed = new DettaglioLineeType();
					bill_filed.NumeroLinea       = line_number.ToString();
					bill_filed.Descrizione       = activity_field.description + Reimbursment_String;
					bill_filed.QuantitaSpecified = true;
					bill_filed.Quantita          = 1;
					bill_filed.PrezzoUnitario    = (activity_field.total_price * Reimbursment) / 100;
					bill_filed.PrezzoTotale      = (activity_field.total_price * Reimbursment) / 100;
					bill_filed.AliquotaIVA       = Vat;

					bill_list.Add(bill_filed);
					line_number++;
				}
			}
		}

		private void InvoiceGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
		{
			try
			{
				UpdateSummaryDataGridView();
				UpdateSaveButton();
			}
			catch
			{
			}
		}

		private void InvoiceGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
		{
			try
			{
				Activity_List.RemoveAt(e.RowIndex);
				UpdateSummaryDataGridView();
				UpdateSaveButton();
			}
			catch
			{
			}
		}

		private void InvoiceGridView_Leave(object sender, EventArgs e)
		{
			try
			{
				InvoiceGridView.EndEdit();
			}
			catch
			{
			}
		}

		private void InvoiceGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (App.IsWindows)
				{
					if (e.ColumnIndex != (int)INVOICE_GRID_VIEW_COLUMN_ID.REIMBOURSE_ID)
					{
						InvoiceGridView.Update();
						InvoiceGridView.BeginEdit(true);
					}
				}
				else if (App.IsUnix)
				{
				}
			}
			catch
			{
			}
		}

		private void InvoiceGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			try
			{
				if (App.IsWindows)
				{
					if (e.ColumnIndex == (int)INVOICE_GRID_VIEW_COLUMN_ID.REIMBOURSE_ID)
					{
						InvoiceGridView.Update();
						InvoiceGridView.BeginEdit(true);
					}
				}
				else if (App.IsUnix)
				{
				}
			}
			catch
			{
			}
		}

		private void InvoiceGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewCheckBoxCell check_box;

			if (App.IsWindows)
			{
			}
			else if (App.IsUnix)
			{
				if (e.ColumnIndex == (int)INVOICE_GRID_VIEW_COLUMN_ID.REIMBOURSE_ID)
				{
					check_box = (DataGridViewCheckBoxCell)InvoiceGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
					check_box.Value = !((bool)check_box.Value);

					InvoiceGridView.Update();
				}
			}
		}

		private void InvoiceGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			if (InvoiceGridView.IsCurrentCellDirty)
			{
				InvoiceGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
			}
		}

		private void InvoiceGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			decimal                     quantity;
			decimal                     unit_price;
			decimal                     total_price;

			int                         row_id;
			INVOICE_GRID_VIEW_COLUMN_ID col_id;
			DataGridViewRow             row;
			bool                        go_to_news_cell;

			row_id = e.RowIndex;
			col_id = (INVOICE_GRID_VIEW_COLUMN_ID)e.ColumnIndex;
			row    = InvoiceGridView.Rows[row_id];

			quantity    = 0;
			unit_price  = 0;
			total_price = 0;

			try
			{
				quantity    = decimal.Parse(InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID].Value.ToString());
				unit_price  = decimal.Parse(InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.UNIT_PRICE_ID].Value.ToString().Replace("€ ", ""));
				total_price = decimal.Parse(InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID].Value.ToString().Replace("€ ", ""));
			}
			catch
			{
			}

			UpdateValueBackColor(col_id, row);

			if ((col_id == INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID) && (unit_price != 0))
			{
				total_price = (quantity * unit_price);
				InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID].Value = total_price.ToString();
				UpdateValueBackColor(INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID, row);
			}
			else if ((col_id == INVOICE_GRID_VIEW_COLUMN_ID.UNIT_PRICE_ID) && (unit_price != 0))
			{
				total_price = (quantity * unit_price);
				InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID].Value = total_price.ToString();
				UpdateValueBackColor(INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID, row);
			}
			else if ((col_id == INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID) && (unit_price != 0) && (total_price != 0) && (quantity == 0))
			{
				quantity = (total_price / unit_price);
				InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID].Value = quantity.ToString();
				UpdateValueBackColor(INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID, row);
			}
			else if ((col_id == INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID) && (quantity > 0) && (total_price != 0))
			{
				unit_price  = (total_price / quantity);
				InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.UNIT_PRICE_ID].Value = unit_price.ToString();
				UpdateValueBackColor(INVOICE_GRID_VIEW_COLUMN_ID.UNIT_PRICE_ID, row);
			}
			else if ((col_id == INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID) && (unit_price != 0) && (total_price != 0))
			{
				quantity = (total_price / unit_price);
				InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID].Value = quantity.ToString();
				UpdateValueBackColor(INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID, row);
			}

			InvoiceGridView.Update();

			try
			{
				if (InvoiceGridView.EnterTabPressed)
				{
					go_to_news_cell = true;

					switch (col_id)
					{
						case INVOICE_GRID_VIEW_COLUMN_ID.DESCRIPTION_ID:
							col_id = INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID;
							break;

						case INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID:
							col_id = INVOICE_GRID_VIEW_COLUMN_ID.UNIT_PRICE_ID;
							break;

						case INVOICE_GRID_VIEW_COLUMN_ID.UNIT_PRICE_ID:
							if (ValidateTotalPriceCell(row))
							{
								row_id++;
								col_id = INVOICE_GRID_VIEW_COLUMN_ID.DESCRIPTION_ID;
							}
							else
							{
								col_id = INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID;
							}
							break;

						case INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID:
							row_id++;
							col_id = INVOICE_GRID_VIEW_COLUMN_ID.DESCRIPTION_ID;
							break;

						default:
							go_to_news_cell = false;
							break;

					}

					if (go_to_news_cell && (row_id < InvoiceGridView.RowCount))
					{
						InvoiceGridView.ClearSelection();
						InvoiceGridView.Rows[row_id].Selected = true;
						InvoiceGridView.CurrentCell = InvoiceGridView.Rows[row_id].Cells[(int)col_id];

						InvoiceGridView.BeginEdit(true);
					}
				}
			}
			catch
			{
			}
		}

		private void InvoiceGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			ACTIVITY_FIELD              field;
			DataGridViewRow             row;
			int                         row_id;
			INVOICE_GRID_VIEW_COLUMN_ID col_id;

			try
			{
				row_id = e.RowIndex;
				col_id = (INVOICE_GRID_VIEW_COLUMN_ID)e.ColumnIndex;
				row = InvoiceGridView.Rows[row_id];

				field.description   = "";
				field.quantity      = 0;
				field.unit_price    = 0;
				field.total_price   = 0;
				field.reimbursement = false;

				field.description   = InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.DESCRIPTION_ID].Value.ToString();
				field.quantity      = decimal.Parse(InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID].Value.ToString());
				field.unit_price    = decimal.Parse(InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.UNIT_PRICE_ID].Value.ToString().Replace("€ ", ""));
				field.total_price   = decimal.Parse(InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID].Value.ToString().Replace("€ ", ""));
				field.reimbursement = bool.Parse(InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.REIMBOURSE_ID].Value.ToString());

				Activity_List[row_id] = field;

				UpdateValueBackColor(col_id, row);
			}
			catch
			{
			}

			UpdateSummaryDataGridView();
			UpdateSaveButton();
		}

		private void InvoiceGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
		{
			INVOICE_GRID_VIEW_COLUMN_ID col_id;
			TextBox text_box;

			e.Control.KeyPress -= new KeyPressEventHandler(InvoiceGridView_DecimalSignedColumnKeyPress);
			e.Control.KeyPress -= new KeyPressEventHandler(InvoiceGridView_DecimalUnsignedColumnKeyPress);

			col_id = (INVOICE_GRID_VIEW_COLUMN_ID)InvoiceGridView.CurrentCell.ColumnIndex;
			switch (col_id)
			{
				case INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID:
					text_box = e.Control as TextBox;
					if (text_box != null)
					{
						text_box.KeyPress += new KeyPressEventHandler(InvoiceGridView_DecimalUnsignedColumnKeyPress);
					}
					break;

				case INVOICE_GRID_VIEW_COLUMN_ID.UNIT_PRICE_ID:
				case INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID:
				case INVOICE_GRID_VIEW_COLUMN_ID.VAT_ID:
					text_box = e.Control as TextBox;
					if (text_box != null)
					{
						text_box.KeyPress += new KeyPressEventHandler(InvoiceGridView_DecimalSignedColumnKeyPress);
					}
					break;
			}
		}

		private void InvoiceGridView_DecimalUnsignedColumnKeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '.')
			{
				if (InvoiceGridView.CurrentCell.Value.ToString().Contains("."))
				{
					e.Handled = true;
				}
			}
			else if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
			{
				e.Handled = true;
			}
		}

		private void InvoiceGridView_DecimalSignedColumnKeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '.')
			{
				if (InvoiceGridView.CurrentCell.Value.ToString().Contains("."))
				{
					e.Handled = true;
				}
			}
			else if (e.KeyChar == '+' || e.KeyChar == '-')
			{
				if (InvoiceGridView.CurrentCell.Value.ToString().StartsWith(String.Format("{0}", e.KeyChar)))
				{
					e.Handled = true;
				}
			}
			else if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
			{
				e.Handled = true;
			}
		}

		private void InvoiceGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			int row_id;
			INVOICE_GRID_VIEW_COLUMN_ID col_id;
			decimal value;

			row_id = e.RowIndex;
			col_id = (INVOICE_GRID_VIEW_COLUMN_ID)e.ColumnIndex;

			try
			{
				switch (col_id)
				{
					case INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID:
					case INVOICE_GRID_VIEW_COLUMN_ID.UNIT_PRICE_ID:
					case INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID:
					case INVOICE_GRID_VIEW_COLUMN_ID.VAT_ID:
						value = decimal.Parse(e.Value.ToString().Replace("€ ", ""));
						e.Value = value.ToString(InvoiceGridView.Columns[(int)col_id].DefaultCellStyle.Format);
						e.FormattingApplied = true;
						break;

					default:
						break;
				}
			}
			catch
			{
			}
		}

		private void OpenStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog         open_file_dialog;
			string                 file_name;
			FatturaElettronicaType invoice;
			XmlSerializer          serializer;
			StreamReader           reader;
			int                    lines_number;
			int                    id;
			DettaglioLineeType     line;
			DettaglioLineeType     field;

			open_file_dialog = new OpenFileDialog();
			open_file_dialog.InitialDirectory = Save_Directory;
			open_file_dialog.Filter = "xml files (*.xml)|*.xml";
			open_file_dialog.RestoreDirectory = true;

			if (open_file_dialog.ShowDialog() == DialogResult.OK)
			{
				file_name = open_file_dialog.FileName;
			}
			else
			{
				return;
			}

			try
			{
				invoice = new FatturaElettronicaType();
				serializer = new XmlSerializer(typeof(FatturaElettronicaType));
				reader = new StreamReader(file_name);
				invoice = (FatturaElettronicaType)serializer.Deserialize(reader);
				reader.Close();
			}
			catch
			{
				MessageBox.Show("Impossibile caricare e deserializzare" + file_name, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			InvoiceGridView.Rows.Clear();
			Activity_List.Clear();

			Vat = invoice.FatturaElettronicaBody[0].DatiBeniServizi.DatiRiepilogo[0].AliquotaIVA;

			lines_number = invoice.FatturaElettronicaBody[0].DatiBeniServizi.DettaglioLinee.Length;

			id = 0;
			line = null;
			field = null;
			while (id < lines_number)
			{
				if (line == null)
				{
					line = invoice.FatturaElettronicaBody[0].DatiBeniServizi.DettaglioLinee[id];
					id++;
				}

				if (line.Descrizione.Contains(Reimbursment_String) && (field != null))
				{
					InvoiceDataGridViewRowAdd(field.Descrizione, field.Quantita, field.PrezzoUnitario, field.PrezzoTotale, true);
					field = null;
					line = null;
				}
				else if (field == null)
				{
					field = line;
					line = null;
				}
				else
				{
					InvoiceDataGridViewRowAdd(field.Descrizione, field.Quantita, field.PrezzoUnitario, field.PrezzoTotale, false);
					field = null;
				}
			}

			if (field != null)
			{
				InvoiceDataGridViewRowAdd(field.Descrizione, field.Quantita, field.PrezzoUnitario, field.PrezzoTotale, false);
				field = null;
				line = null;
			}
		}

		private void AddStripMenuItem_Click(object sender, EventArgs e)
		{
			InvoiceDataGridViewRowAdd(Invoice_Grid_View_Default_Value);

			if (!InvoiceGridView.IsCurrentCellInEditMode)
			{
				try
				{
					InvoiceGridView.CurrentCell = InvoiceGridView.Rows[InvoiceGridView.RowCount - 1].Cells[0];
					InvoiceGridView.BeginEdit(true);
				}
				catch
				{
				}
			}
		}

		private void RemoveStripMenuItem_Click(object sender, EventArgs e)
		{
			int number;

			number = InvoiceGridView.Rows.GetRowCount(DataGridViewElementStates.Selected);
			if (number > 0)
			{
				for (int i = 0; i < number; i++)
				{
					InvoiceGridView.Rows.RemoveAt(InvoiceGridView.SelectedRows[0].Index);
				}
			}
		}

		private void GenerateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FatturaElettronicaType               invoice;
			XmlSerializer                        template_serializer;
			XmlSerializer                        invoice_serializer;
			StreamReader                         reader;
			XmlSerializerNamespaces              name_space;
			List<DettaglioLineeType>             bill_list;
			InformationForm                      info_form;
			InformationForm.RESULT               result;
			StreamWriter                         writer;
			string                               file_name;
			SaveFileDialog                       save_file_dialog;

			try
			{
				invoice = new FatturaElettronicaType();
				template_serializer = new XmlSerializer(typeof(FatturaElettronicaType));
			}
			catch
			{
				MessageBox.Show("Impossibile creare il prototipo di fattura elettronica", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			try
			{
				reader = new StreamReader(Template_File_Name);
				invoice = (FatturaElettronicaType)template_serializer.Deserialize(reader);
				reader.Close();
			}
			catch
			{
				MessageBox.Show("Impossibile caricare e deserializzare" + Template_File_Name, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			info_form = new InformationForm();
			info_form.ShowDialog(this);
			result = info_form.GetResult();

			if (!result.ok)
			{
				return;
			}

			file_name = result.number.ToString("000000") + "-" + result.customer_name + "-" + result.date_time.ToString("yyyyMMdd");

			save_file_dialog = new SaveFileDialog();
			save_file_dialog.InitialDirectory = Save_Directory;
			save_file_dialog.FileName = file_name;
			save_file_dialog.Filter = "xml files (*.xml) | *.xml";
			save_file_dialog.Title = "Salvataggio file generato";

			if (save_file_dialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			file_name = save_file_dialog.FileName;

			Save_Directory = Path.GetDirectoryName(file_name);
			Ini_File.SetKeyValue("Conf", "SaveDirectory", Save_Directory);

			Ini_File.Save(App.Name + ".ini");

			GenerateBillList(out bill_list);
			invoice.FatturaElettronicaBody[0].DatiBeniServizi.DettaglioLinee = bill_list.ToArray();

			invoice.FatturaElettronicaBody[0].DatiBeniServizi.DatiRiepilogo[0].AliquotaIVA = Vat;
			invoice.FatturaElettronicaBody[0].DatiBeniServizi.DatiRiepilogo[0].ImponibileImporto = Summary.total_taxable;
			invoice.FatturaElettronicaBody[0].DatiBeniServizi.DatiRiepilogo[0].Imposta = Summary.total_vat;

			invoice.FatturaElettronicaBody[0].DatiGenerali.DatiGeneraliDocumento.DatiRitenuta.AliquotaRitenuta = Withholding_Tax;
			invoice.FatturaElettronicaBody[0].DatiGenerali.DatiGeneraliDocumento.DatiRitenuta.ImportoRitenuta  = Summary.withholding_tax;

			invoice.FatturaElettronicaBody[0].DatiGenerali.DatiGeneraliDocumento.Data = result.date_time;
			invoice.FatturaElettronicaBody[0].DatiGenerali.DatiGeneraliDocumento.Numero = result.number.ToString();
			invoice.FatturaElettronicaBody[0].DatiGenerali.DatiGeneraliDocumento.ImportoTotaleDocumento = Summary.total;
			invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].ImportoPagamento = Summary.to_pay;
			invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].DataRiferimentoTerminiPagamento = result.date_time;
			invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].DataScadenzaPagamentoSpecified = true;
			invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].DataScadenzaPagamento = result.date_time.AddDays(1);

			invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].Beneficiario = Bank_Account.beneficiary;
			invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].IstitutoFinanziario = Bank_Account.financial_institute;
			invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].IBAN = Bank_Account.iban;
			invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].ABI = Bank_Account.abi;
			invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].CAB = Bank_Account.cab;
			invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].BIC = Bank_Account.bic;

			invoice_serializer = new XmlSerializer(typeof(FatturaElettronicaType));

			writer = new StreamWriter(file_name);

			name_space = new XmlSerializerNamespaces();
			name_space.Add("ns2", "http://ivaservizi.agenziaentrate.gov.it/docs/xsd/fatture/v1.2");

			invoice_serializer.SerializeWithDecimalFormatting(writer, invoice, name_space);

			writer.Close();
		}

		private void DonatePictureBox_Click(object sender, EventArgs e)
		{
			string url = "";

			string business = "PBUGWQJTH5MGC";
			string description = "Donation";
			string country = "IT";
			string currency = "EUR";

			url += "https://www.paypal.com/cgi-bin/webscr" +
			       "?cmd=" + "_donations" +
			       "&business=" + business +
			       "&lc=" + country +
			       "&item_name=" + description +
			       "&currency_code=" + currency +
			       "&bn=" + "PP%2dDonationsBF";

			System.Diagnostics.Process.Start(url);
		}

		private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AboutBox about;

			about = new AboutBox(true);

			about.ShowDialog(this);
		}
	}
}

