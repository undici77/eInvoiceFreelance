using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace eInvoiceFreelance
{
	public partial class MainForm : Form
	{
		private enum INVOICE_GRID_VIEW_COLUMN_ID
		{
			DESCRIPTION_ID,
			QUANTITY_ID,
			UNIT_PRICE_ID,
			TOTAL_PRICE_ID,
			REIMBOURSE_ID,
			VAT_ID,
			VAT_ENABLE_ID
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

		struct Init
		{
			public Reimbursment  reimbursment;
			public RevenueStamp  revenue_stamp;
			public ActivityField init_activity;
			public BankAccount   bank_account;
			public Tax           tax;
			public object[]      default_value;
			public string        template_file_name;
			public string        save_directory;
		};

		private IniFile _Ini_File;
		private Init    _Init;
		private Invoice _Invoice;
		private bool    _Init_Invoice_Data_Grid_View;

		public MainForm()
		{
			InitializeComponent();

			this.Text = App.Name + " " + App.Version;

			_Init_Invoice_Data_Grid_View = false;
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			AboutBox about;
			string disclaimer_signature;
			bool disclaimer_accepted;

			_Ini_File = new IniFile();
			_Ini_File.Load(App.Name + ".ini");

			_Init.reimbursment  = null;
			_Init.init_activity = null;
			_Init.bank_account  = null;
			_Init.tax           = null;
			_Init.revenue_stamp = null;

			if (!InitData(_Ini_File, ref _Init))
			{
				Application.Exit();
				return;
			}

			_Invoice = new Invoice(_Init.template_file_name, _Init.reimbursment, _Init.init_activity, _Init.revenue_stamp, _Init.bank_account, _Init.tax);

			InvoiceGridView.Columns[(int)INVOICE_GRID_VIEW_COLUMN_ID.DESCRIPTION_ID].ValueType = typeof(string);
			InvoiceGridView.Columns[(int)INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID].ValueType = typeof(string);
			InvoiceGridView.Columns[(int)INVOICE_GRID_VIEW_COLUMN_ID.UNIT_PRICE_ID].ValueType = typeof(string);
			InvoiceGridView.Columns[(int)INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID].ValueType = typeof(string);
			InvoiceGridView.Columns[(int)INVOICE_GRID_VIEW_COLUMN_ID.REIMBOURSE_ID].ValueType = typeof(bool);
			if (App.IsUnix)
			{
				InvoiceGridView.Columns[(int)INVOICE_GRID_VIEW_COLUMN_ID.REIMBOURSE_ID].ReadOnly = true;
			}
			InvoiceGridView.Columns[(int)INVOICE_GRID_VIEW_COLUMN_ID.VAT_ID].ValueType = typeof(decimal);
			InvoiceGridView.Columns[(int)INVOICE_GRID_VIEW_COLUMN_ID.VAT_ENABLE_ID].ValueType = typeof(bool);
			if (App.IsUnix)
			{
				InvoiceGridView.Columns[(int)INVOICE_GRID_VIEW_COLUMN_ID.VAT_ENABLE_ID].ReadOnly = true;
			}

			SummaryValue.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

			SummariesGridView.Columns[(int)SUMMARY_GRID_VIEW_COLUMN_ID.DATA_ID].ValueType = typeof(string);
			SummariesGridView.Columns[(int)SUMMARY_GRID_VIEW_COLUMN_ID.VALUE_ID].ValueType = typeof(decimal);

			disclaimer_signature = (App.Name + " " + App.Version);
			disclaimer_accepted = (_Ini_File.GetKeyValue("Disclaimer", "Accepted") == disclaimer_signature);
			_Ini_File.SetKeyValue("Disclaimer", "Accepted", disclaimer_signature);

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

			RevenueStampTextBox.Currency = _Init.revenue_stamp.Price.ToString();
			RevenueStampCheckBox.Checked = _Init.revenue_stamp.Enable;
			RevenueStampTextBox.ReadOnly = !RevenueStampCheckBox.Checked;

			_Ini_File.Save(App.Name + ".ini");

			UpdateSummaryDataGridViewHeader();
			UpdateSummaryDataGridView();
			UpdateGenerateButtons();
		}

		private bool InitData(IniFile ini_file, ref Init init)
		{
			string description;
			decimal quantity;
			decimal unit_price;
			decimal total_price;
			decimal vat_percent;
			decimal withholding_tax_percent;
			string  beneficiary;
			string  financial_institute;
			string  iban;
			string  abi;
			string  cab;
			string  bic;
			decimal reimbursment_percent;
			string  reimbursment_description;
			bool    revenue_stamp_enable;
			decimal revenue_stamp_price;

			OpenFileDialog open_file_dialog;

			description = ini_file.GetKeyValue("Init", "Description");

			try
			{
				quantity = decimal.Parse(ini_file.GetKeyValue("Init", "Quantity"));
			}
			catch
			{
				quantity = 1;
			}
			try
			{
				unit_price = decimal.Parse(ini_file.GetKeyValue("Init", "UnitPrice"));
			}
			catch
			{
				unit_price = 0;
			}
			try
			{
				vat_percent = decimal.Parse(ini_file.GetKeyValue("Tax", "Vat"));
			}
			catch
			{
				vat_percent = 22;
			}
			try
			{
				withholding_tax_percent = decimal.Parse(ini_file.GetKeyValue("Tax", "WithholdingTax"));
			}
			catch
			{
				withholding_tax_percent = 20;
				ini_file.SetKeyValue("Tax", "WithholdingTax", withholding_tax_percent.ToString());
			}

			total_price = quantity * unit_price;

			ini_file.SetKeyValue("Init", "Description", description);
			ini_file.SetKeyValue("Init", "Quantity", quantity.ToString());
			ini_file.SetKeyValue("Init", "UnitPrice", unit_price.ToString());

			ini_file.SetKeyValue("Tax", "Vat", vat_percent.ToString());
			ini_file.SetKeyValue("Tax", "WithholdingTax", withholding_tax_percent.ToString());

			init.template_file_name = ini_file.GetKeyValue("Conf", "Template");
			if (string.IsNullOrEmpty(init.template_file_name))
			{
				if (App.IsWindows)
				{
					init.template_file_name = App.Path + "Template.xml";
				}
				else if (App.IsUnix)
				{
					init.template_file_name = "/opt/eInvoiceFreelance/Template.xml";
				}
				else
				{
					init.template_file_name = "Template.xml";
				}
			}

			if (!File.Exists(init.template_file_name))
			{
				open_file_dialog = new OpenFileDialog();
				open_file_dialog.InitialDirectory = App.Path;
				open_file_dialog.Filter = "xml files (*.xml)|*.xml";
				open_file_dialog.RestoreDirectory = true;

				if (open_file_dialog.ShowDialog() == DialogResult.OK)
				{
					init.template_file_name = open_file_dialog.FileName;
				}
				else
				{
					return (false);
				}
			}

			ini_file.SetKeyValue("Conf", "Template", init.template_file_name);

			beneficiary         = ini_file.GetKeyValue("BankAccount", "Beneficiary");
			financial_institute = ini_file.GetKeyValue("BankAccount", "FinancialInstitute");
			iban                = ini_file.GetKeyValue("BankAccount", "IBAN");
			abi                 = ini_file.GetKeyValue("BankAccount", "ABI");
			cab                 = ini_file.GetKeyValue("BankAccount", "CAB");
			bic                 = ini_file.GetKeyValue("BankAccount", "BIC");

			try
			{
				reimbursment_percent = decimal.Parse(ini_file.GetKeyValue("Reimbursment", "Percent"));
			}
			catch
			{
				reimbursment_percent = 4;
				ini_file.SetKeyValue("Reimbursment", "Percent", reimbursment_percent.ToString());
			}

			reimbursment_description = ini_file.GetKeyValue("Reimbursment", "Description");
			if (string.IsNullOrEmpty(reimbursment_description))
			{
				reimbursment_description = "Cont. Prev.";
			}
			ini_file.SetKeyValue("Reimbursment", "Description", reimbursment_description);

			try
			{
				revenue_stamp_enable = (ini_file.GetKeyValue("RevenueStamp", "Enable") != "0");
			}
			catch
			{
				revenue_stamp_enable = false;
				ini_file.SetKeyValue("Reimbursment", "Enable", (revenue_stamp_enable ? "1" : "0"));
			}
			try
			{
				revenue_stamp_price = decimal.Parse(ini_file.GetKeyValue("RevenueStamp", "Price"));
			}
			catch
			{
				revenue_stamp_price = 2;
				ini_file.SetKeyValue("RevenueStamp", "Price", revenue_stamp_price.ToString());
			}

			init.init_activity = new ActivityField(description, quantity, unit_price, total_price, true, true);
			init.default_value = new object[] { description, quantity.ToString(), unit_price.ToString(), total_price.ToString(), true, vat_percent, true };
			init.tax           = new Tax(vat_percent, withholding_tax_percent);
			init.bank_account  = new BankAccount(beneficiary, financial_institute, iban, abi, cab, bic);
			init.reimbursment  = new Reimbursment(reimbursment_percent, reimbursment_description);
			init.revenue_stamp = new RevenueStamp(revenue_stamp_enable, revenue_stamp_price);

			init.save_directory = ini_file.GetKeyValue("Conf", "SaveDirectory");

			return (true);
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
			ActivityField field;

			string  description;
			decimal quantity;
			decimal unit_price;
			decimal total_price;
			bool    reimbursement;
			bool    vat_enable;

			if (_Init_Invoice_Data_Grid_View)
			{
				return;
			}

			description   = (string)row[(int)INVOICE_GRID_VIEW_COLUMN_ID.DESCRIPTION_ID];
			quantity      = decimal.Parse((string)row[(int)INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID]);
			unit_price    = decimal.Parse(((string)row[(int)INVOICE_GRID_VIEW_COLUMN_ID.UNIT_PRICE_ID]).Replace("€ ", ""));
			total_price   = decimal.Parse(((string)row[(int)INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID]).Replace("€ ", ""));
			reimbursement = (bool)row[(int)INVOICE_GRID_VIEW_COLUMN_ID.REIMBOURSE_ID];
			vat_enable    = (bool)row[(int)INVOICE_GRID_VIEW_COLUMN_ID.VAT_ENABLE_ID];

			field = new ActivityField(description, quantity, unit_price, total_price, reimbursement, vat_enable);

			_Invoice.ActivityList.Add(field);
			InvoiceGridView.Rows.Add(row);
		}

		private void InvoiceDataGridViewRowAdd(string description, decimal quantity, decimal unit_price, decimal total_price, bool reimbursement, bool vat_enable)
		{
			ActivityField field;
			object[]      row;

			if (_Init_Invoice_Data_Grid_View)
			{
				return;
			}

			row = new object[] { description, quantity.ToString(), unit_price.ToString(), total_price.ToString(), reimbursement, _Invoice.Tax.VatPercent };
			field = new ActivityField(description, quantity, unit_price, total_price, reimbursement, vat_enable);

			_Invoice.ActivityList.Add(field);
			InvoiceGridView.Rows.Add(row);
		}

		private void InvoiceDataGridViewRowInit(string description, decimal quantity, decimal unit_price, decimal total_price, bool reimbursement, bool vat_enable)
		{
			object[] row;

			row = new object[] { description, quantity.ToString(), unit_price.ToString(), total_price.ToString(), reimbursement, _Invoice.Tax.VatPercent, vat_enable };

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

		private void UpdateGenerateButtons()
		{
			DataGridViewRow row;
			int row_id;
			bool xml_enable;
			bool pdf_enable;

			try
			{
				row_id = 0;
				xml_enable = (InvoiceGridView.RowCount > 0);

				while (xml_enable && (row_id < InvoiceGridView.RowCount))
				{
					row = InvoiceGridView.Rows[row_id];
					if (!ValidateDescriptionCell(row))
					{
						xml_enable = false;
					}
					else if (!ValidateQuantityCell(row))
					{
						xml_enable = false;
					}
					else if (!ValidateUnitPriceCell(row))
					{
						xml_enable = false;
					}
					else if (!ValidateTotalPriceCell(row))
					{
						xml_enable = false;
					}
					else if (!ValidateVatCell(row))
					{
						xml_enable = false;
					}

					row_id++;
				}
			}
			catch
			{
				xml_enable = false;
			}

			XmlInvoiceToolStripMenuItem.Enabled = xml_enable;

			pdf_enable = xml_enable && !_Invoice.Supplier.IsEmpty() && !_Invoice.Customer.IsEmpty() && !_Invoice.BankAccount.IsEmpty();
			PdfProformaToolStripMenuItem.Enabled = pdf_enable;
			PdfInvoiceToolStripMenuItem.Enabled = pdf_enable;
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

				case INVOICE_GRID_VIEW_COLUMN_ID.REIMBOURSE_ID:
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

				case INVOICE_GRID_VIEW_COLUMN_ID.VAT_ENABLE_ID:
					break;

				default:
					Errors.Assert(false);
					break;
			}
		}

		private void UpdateSummaryDataGridViewHeader()
		{
			InvoiceGridView.Columns[(int)INVOICE_GRID_VIEW_COLUMN_ID.REIMBOURSE_ID].HeaderText += " " + _Invoice.Reimbursment.Percent.ToString("0.00") + "%";

			SummariesGridView.Rows.Add("Importo", 0);
			SummariesGridView.Rows.Add("Contributo Previdenziale " + _Invoice.Reimbursment.Percent.ToString("0.00") + "%", 0);
			SummariesGridView.Rows.Add("Totale Imponibile", 0);
			SummariesGridView.Rows.Add("Totale IVA " + _Invoice.Tax.VatPercent.ToString("0.00") + "%", 0);
			SummariesGridView.Rows.Add("Totale", 0);
			SummariesGridView.Rows.Add("Ritenuta d'acconto " + _Invoice.Tax.WithholdingTaxPercent.ToString("0.00") + "%", 0);
			SummariesGridView.Rows.Add("Totale", 0);
		}

		private void UpdateSummaryDataGridView()
		{
			try
			{
				SummariesGridView.Rows[(int)SUMMARY_GRID_VIEW_ROW_ID.TAXABLE_ID].Cells[(int)SUMMARY_GRID_VIEW_COLUMN_ID.VALUE_ID].Value = _Invoice.Summary.Taxable;
				SummariesGridView.Rows[(int)SUMMARY_GRID_VIEW_ROW_ID.REIMBURSMENT_ID].Cells[(int)SUMMARY_GRID_VIEW_COLUMN_ID.VALUE_ID].Value = _Invoice.Summary.Reimbursment;
				SummariesGridView.Rows[(int)SUMMARY_GRID_VIEW_ROW_ID.TOTAL_TAXABLE_ID].Cells[(int)SUMMARY_GRID_VIEW_COLUMN_ID.VALUE_ID].Value = _Invoice.Summary.TotalTaxable;
				SummariesGridView.Rows[(int)SUMMARY_GRID_VIEW_ROW_ID.TOTAL_VAT_ID].Cells[(int)SUMMARY_GRID_VIEW_COLUMN_ID.VALUE_ID].Value = _Invoice.Summary.TotalVat;
				SummariesGridView.Rows[(int)SUMMARY_GRID_VIEW_ROW_ID.TOTAL_ID].Cells[(int)SUMMARY_GRID_VIEW_COLUMN_ID.VALUE_ID].Value = _Invoice.Summary.Total;
				SummariesGridView.Rows[(int)SUMMARY_GRID_VIEW_ROW_ID.WITHHOLDING_TAX_ID].Cells[(int)SUMMARY_GRID_VIEW_COLUMN_ID.VALUE_ID].Value = _Invoice.Summary.TotalWithholdingTax;
				SummariesGridView.Rows[(int)SUMMARY_GRID_VIEW_ROW_ID.TO_PAY_ID].Cells[(int)SUMMARY_GRID_VIEW_COLUMN_ID.VALUE_ID].Value = _Invoice.Summary.ToPay;
			}
			catch
			{
			}
		}

		private void InvoiceGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
		{
			UpdateSummaryDataGridView();
			UpdateGenerateButtons();
		}

		private void InvoiceGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
		{
			if (_Init_Invoice_Data_Grid_View)
			{
				return;
			}

			try
			{
				_Invoice.ActivityList.RemoveAt(e.RowIndex);
			}
			catch
			{
			}

			UpdateSummaryDataGridView();
			UpdateGenerateButtons();
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
			DataGridViewCheckBoxCell check_box;
			try
			{
				if (App.IsWindows)
				{
					InvoiceGridView.Update();
					InvoiceGridView.BeginEdit(true);
					if ((e.ColumnIndex == (int)INVOICE_GRID_VIEW_COLUMN_ID.REIMBOURSE_ID) ||
					    (e.ColumnIndex == (int)INVOICE_GRID_VIEW_COLUMN_ID.VAT_ENABLE_ID))
					{
						check_box = (DataGridViewCheckBoxCell)InvoiceGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
						check_box.Value = !((bool)check_box.Value);
						InvoiceGridView.EndEdit();
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
			DataGridViewCheckBoxCell check_box;

			try
			{
				if (App.IsWindows)
				{
					if ((e.ColumnIndex == (int)INVOICE_GRID_VIEW_COLUMN_ID.REIMBOURSE_ID) ||
					    (e.ColumnIndex == (int)INVOICE_GRID_VIEW_COLUMN_ID.VAT_ENABLE_ID))
					{
						InvoiceGridView.Update();
						InvoiceGridView.BeginEdit(true);
						check_box = (DataGridViewCheckBoxCell)InvoiceGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
						check_box.Value = !((bool)check_box.Value);
						InvoiceGridView.EndEdit();
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
				if ((e.ColumnIndex == (int)INVOICE_GRID_VIEW_COLUMN_ID.REIMBOURSE_ID) ||
				    (e.ColumnIndex == (int)INVOICE_GRID_VIEW_COLUMN_ID.VAT_ENABLE_ID))
				{
					check_box = (DataGridViewCheckBoxCell)InvoiceGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
					check_box.Value = !((bool)check_box.Value);

					InvoiceGridView.Update();
				}
			}
		}

        private void RevenueStampCheckBox_CheckedChanged(object sender, EventArgs e)
        {
			RevenueStamp revenue_stamp;

			RevenueStampTextBox.ReadOnly = !RevenueStampCheckBox.Checked;

			_Ini_File.SetKeyValue("RevenueStamp", "Enable", RevenueStampCheckBox.Checked ? "1" : "0");
			_Ini_File.Save(App.Name + ".ini");

			revenue_stamp = new RevenueStamp(RevenueStampCheckBox.Checked, RevenueStampTextBox.Decimal);
			_Invoice.RevenueStamp = revenue_stamp;

			_Invoice.ActivityListUpdate();

			UpdateSummaryDataGridView();
			UpdateGenerateButtons();
        }

        private void RevenueStampTextBox_TextChanged(object sender, EventArgs e)
        {
			RevenueStamp revenue_stamp;

			_Ini_File.SetKeyValue("RevenueStamp", "Value", RevenueStampTextBox.Decimal.ToString());
			_Ini_File.Save(App.Name + ".ini");

			revenue_stamp = new RevenueStamp(RevenueStampCheckBox.Checked, RevenueStampTextBox.Decimal);
			_Invoice.RevenueStamp = revenue_stamp;

			_Invoice.ActivityListUpdate();

			UpdateSummaryDataGridView();
			UpdateGenerateButtons();
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
			string  description;
			decimal quantity;
			decimal unit_price;
			decimal total_price;
			bool    reimbursement;
			bool    vat_enable;

			int row_id;
			INVOICE_GRID_VIEW_COLUMN_ID col_id;
			DataGridViewRow row;
			bool go_to_news_cell;

			row_id = e.RowIndex;
			col_id = (INVOICE_GRID_VIEW_COLUMN_ID)e.ColumnIndex;
			row = InvoiceGridView.Rows[row_id];

			quantity = 0;
			unit_price = 0;
			total_price = 0;

			try
			{
				quantity = decimal.Parse(InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID].Value.ToString());
				unit_price = decimal.Parse(InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.UNIT_PRICE_ID].Value.ToString().Replace("€ ", ""));
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
				unit_price = (total_price / quantity);
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
				description   = InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.DESCRIPTION_ID].Value.ToString();
				reimbursement = bool.Parse(InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.REIMBOURSE_ID].Value.ToString());
				vat_enable    = bool.Parse(InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.VAT_ENABLE_ID].Value.ToString());

				_Invoice.ActivityList[row_id] = new ActivityField(description, quantity, unit_price, total_price, reimbursement, vat_enable);

				UpdateSummaryDataGridView();
			}
			catch
			{
			}

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
			DataGridViewRow             row;
			int                         row_id;
			INVOICE_GRID_VIEW_COLUMN_ID col_id;

			try
			{
				row_id = e.RowIndex;
				col_id = (INVOICE_GRID_VIEW_COLUMN_ID)e.ColumnIndex;
				row = InvoiceGridView.Rows[row_id];

				UpdateValueBackColor(col_id, row);
			}
			catch
			{
			}

			try
			{
				UpdateGenerateButtons();
			}
			catch
			{
			}
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
			else if ((e.KeyChar == '+') || (e.KeyChar == '-'))
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
			OpenFileDialog open_file_dialog;
			string file_name;

			open_file_dialog = new OpenFileDialog();
			open_file_dialog.InitialDirectory = _Init.save_directory;
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
				_Invoice = new Invoice(file_name, _Init.reimbursment, _Init.init_activity, _Init.revenue_stamp);
			}
			catch
			{
				MessageBox.Show("Impossibile caricare e deserializzare " + file_name, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			_Init_Invoice_Data_Grid_View = true;

			InvoiceGridView.Rows.Clear();

			try
			{
				foreach (ActivityField f in _Invoice.ActivityList.ToArray())
				{
					InvoiceDataGridViewRowInit(f.Description, f.Quantity, f.UnitPrice, f.TotalPrice, f.Reimbursement, f.VatEnable);
				}
			}
			catch
			{
				MessageBox.Show("Impossibile elaborare " + file_name, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			_Init_Invoice_Data_Grid_View = false;

			RevenueStampCheckBox.Checked = _Invoice.RevenueStamp.Enable;
			RevenueStampTextBox.Currency = _Invoice.RevenueStamp.Price.ToString();
		}

		private void AddStripMenuItem_Click(object sender, EventArgs e)
		{
			InvoiceDataGridViewRowAdd(_Init.default_value);

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

		private void XmlInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string file_name;
			InformationForm info_form;
			InformationForm.RESULT result;
			SaveFileDialog save_file_dialog;

			info_form = new InformationForm();
			info_form.ShowDialog(this);
			result = info_form.GetResult();
			if (!result.ok)
			{
				return;
			}

			file_name = result.number.ToString("000000") + "-" + result.customer_name + "-" + result.date_time.ToString("yyyyMMdd");

			save_file_dialog = new SaveFileDialog();
			save_file_dialog.InitialDirectory = _Init.save_directory;
			save_file_dialog.FileName = file_name;
			save_file_dialog.Filter = "xml files (*.xml) | *.xml";
			save_file_dialog.Title = "Salvataggio file generato";

			if (save_file_dialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			file_name = save_file_dialog.FileName;

			_Init.save_directory = Path.GetDirectoryName(file_name);
			_Ini_File.SetKeyValue("Conf", "SaveDirectory", _Init.save_directory);

			_Ini_File.Save(App.Name + ".ini");

			_Invoice.GenerateXML(file_name, result.number, result.date_time);
		}

		private void PdfProformaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string file_name;
			InformationForm info_form;
			InformationForm.RESULT result;
			SaveFileDialog save_file_dialog;

			try
			{
				info_form = new InformationForm();
				info_form.ShowDialog(this);
				result = info_form.GetResult();
				if (!result.ok)
				{
					return;
				}

				file_name = "Proforma-" +  result.number.ToString("000000") + "-" + result.customer_name + "-" + result.date_time.ToString("yyyyMMdd");

				save_file_dialog = new SaveFileDialog();
				save_file_dialog.InitialDirectory = _Init.save_directory;
				save_file_dialog.FileName = file_name;
				save_file_dialog.Filter = "pdf files (*.pdf) | *.pdf";
				save_file_dialog.Title = "Salvataggio file generato";

				if (save_file_dialog.ShowDialog() != DialogResult.OK)
				{
					return;
				}

				file_name = save_file_dialog.FileName;

				_Init.save_directory = Path.GetDirectoryName(file_name);
				_Ini_File.SetKeyValue("Conf", "SaveDirectory", _Init.save_directory);

				_Invoice.GeneratePDF(file_name, "Proforma", result.number, result.date_time);
			}
			catch
			{
				MessageBox.Show("Impossibile generare Proforma", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
		}

		private void PdfInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string file_name;
			InformationForm info_form;
			InformationForm.RESULT result;
			SaveFileDialog save_file_dialog;

			try
			{
				info_form = new InformationForm();
				info_form.ShowDialog(this);
				result = info_form.GetResult();
				if (!result.ok)
				{
					return;
				}

				file_name = "Fattura-" + result.number.ToString("000000") + "-" + result.customer_name + "-" + result.date_time.ToString("yyyyMMdd");

				save_file_dialog = new SaveFileDialog();
				save_file_dialog.InitialDirectory = _Init.save_directory;
				save_file_dialog.FileName = file_name;
				save_file_dialog.Filter = "pdf files (*.pdf) | *.pdf";
				save_file_dialog.Title = "Salvataggio file generato";

				if (save_file_dialog.ShowDialog() != DialogResult.OK)
				{
					return;
				}

				file_name = save_file_dialog.FileName;

				_Init.save_directory = Path.GetDirectoryName(file_name);
				_Ini_File.SetKeyValue("Conf", "SaveDirectory", _Init.save_directory);

				_Invoice.GeneratePDF(file_name, "Fattura", result.number, result.date_time);
			}
			catch
			{
				MessageBox.Show("Impossibile generare Proforma", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
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

