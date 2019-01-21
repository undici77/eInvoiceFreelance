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
			public decimal vat;
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

		private decimal Reimbursment;
		private decimal Withholding_Tax;

		private List<ACTIVITY_FIELD> Activity_List;
		private SUMMARY Summary;
		private object[] Invoice_Grid_View_Default_Value;
		private IniFile Ini_File;

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
			decimal vat;

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
				vat = decimal.Parse(Ini_File.GetKeyValue("Init", "Vat"));
			}
			catch
			{
				vat = 22;
			}

			total_price = quantity * unit_price;

			Invoice_Grid_View_Default_Value = new object[] { description, quantity.ToString(), unit_price.ToString(), total_price, true, vat };

			Ini_File.SetKeyValue("Init", "Description", description);
			Ini_File.SetKeyValue("Init", "Quantity", quantity.ToString());
			Ini_File.SetKeyValue("Init", "UnitPrice", unit_price.ToString());
			Ini_File.SetKeyValue("Init", "Vat", vat.ToString());

			try
			{
				Reimbursment = decimal.Parse(Ini_File.GetKeyValue("Conf", "Reimbursment"));
			}
			catch
			{
				Reimbursment = 4;
				Ini_File.SetKeyValue("Conf", "Reimbursment", Reimbursment.ToString());
			}

			try
			{
				Withholding_Tax = decimal.Parse(Ini_File.GetKeyValue("Conf", "WithholdingTax"));
			}
			catch
			{
				Withholding_Tax = 20;
				Ini_File.SetKeyValue("Conf", "WithholdingTax", Reimbursment.ToString());
			}

			InvoiceGridView.Columns[(int)INVOICE_GRID_VIEW_COLUMN_ID.REIMBOURSE_ID].HeaderText += " " + Reimbursment.ToString("0.00") + "%";

			InvoiceGridView.Columns[(int)INVOICE_GRID_VIEW_COLUMN_ID.DESCRIPTION_ID].ValueType = typeof(string);
			InvoiceGridView.Columns[(int)INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID].ValueType    = typeof(string);
			InvoiceGridView.Columns[(int)INVOICE_GRID_VIEW_COLUMN_ID.UNIT_PRICE_ID].ValueType  = typeof(string);
			InvoiceGridView.Columns[(int)INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID].ValueType = typeof(string);
			InvoiceGridView.Columns[(int)INVOICE_GRID_VIEW_COLUMN_ID.REIMBOURSE_ID].ValueType  = typeof(bool);
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

					vat = (((field.total_price + reimbursment) * field.vat) / 100);

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

		private void InvoiceGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
		{
			ACTIVITY_FIELD field;

			field.description   = (string)Invoice_Grid_View_Default_Value[(int)INVOICE_GRID_VIEW_COLUMN_ID.DESCRIPTION_ID];
			field.quantity      = decimal.Parse(Invoice_Grid_View_Default_Value[(int)INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID].ToString());
			field.unit_price    = decimal.Parse(Invoice_Grid_View_Default_Value[(int)INVOICE_GRID_VIEW_COLUMN_ID.UNIT_PRICE_ID].ToString().Replace("€ ", ""));
			field.total_price   = decimal.Parse(Invoice_Grid_View_Default_Value[(int)INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID].ToString().Replace("€ ", ""));
			field.reimbursement = (bool)Invoice_Grid_View_Default_Value[(int)INVOICE_GRID_VIEW_COLUMN_ID.REIMBOURSE_ID];
			field.vat           = (decimal)Invoice_Grid_View_Default_Value[(int)INVOICE_GRID_VIEW_COLUMN_ID.VAT_ID];

			Activity_List.Insert(e.RowIndex, field);
			UpdateSummaryDataGridView();
			UpdateSaveButton();
		}

		private void InvoiceGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
		{
			Activity_List.RemoveAt(e.RowIndex);
			UpdateSummaryDataGridView();
			UpdateSaveButton();
		}

		private void InvoiceGridView_Leave(object sender, EventArgs e)
		{
			InvoiceGridView.EndEdit();
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

			try
			{
				if (InvoiceGridView.EditCancelled)
				{
					InvoiceGridView.CurrentCell = null;
				}
				else
				{
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

						default:
							row_id++;
							col_id = INVOICE_GRID_VIEW_COLUMN_ID.DESCRIPTION_ID;
							break;
					}

					if (row_id < InvoiceGridView.RowCount)
					{
						InvoiceGridView.CurrentCell = InvoiceGridView.Rows[row_id].Cells[(int)col_id];
						InvoiceGridView.BeginEdit(true);
					}
					else
					{
						InvoiceGridView.CurrentCell = null;
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
				field.vat           = 0;

				field.description   = InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.DESCRIPTION_ID].Value.ToString();
				field.quantity      = decimal.Parse(InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID].Value.ToString());
				field.unit_price    = decimal.Parse(InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.UNIT_PRICE_ID].Value.ToString().Replace("€ ", ""));
				field.total_price   = decimal.Parse(InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID].Value.ToString().Replace("€ ", ""));
				field.reimbursement = bool.Parse(InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.REIMBOURSE_ID].Value.ToString());
				field.vat           = decimal.Parse(InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.VAT_ID].Value.ToString());

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
		}

		private void AddStripMenuItem_Click(object sender, EventArgs e)
		{
			InvoiceGridView.Rows.Add(Invoice_Grid_View_Default_Value);

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
			FatturaElettronicaType f = new FatturaElettronicaType();

			XmlSerializer serializer = new XmlSerializer(typeof(FatturaElettronicaType));

			StreamReader reader = new StreamReader(App.Path + "Template.xml");
			f = (FatturaElettronicaType)serializer.Deserialize(reader);
			reader.Close();

			XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
			ns.Add("ns2", "http://ivaservizi.agenziaentrate.gov.it/docs/xsd/fatture/v1.2");
			XmlSerializer s = new XmlSerializer(typeof(FatturaElettronicaType));

			List<DettaglioLineeType> l = new List<DettaglioLineeType>(f.FatturaElettronicaBody[0].DatiBeniServizi.DettaglioLinee);
			DettaglioLineeType p = new DettaglioLineeType();

			p.Descrizione = "aaa";
			p.AliquotaIVA = 22;

			l.Add(p);

			f.FatturaElettronicaBody[0].DatiBeniServizi.DettaglioLinee = l.ToArray();

			using (StreamWriter writer = new StreamWriter(@"c:\Projects\aaa111.xml"))
			{
				s.Serialize(writer, f, ns);
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

		private void InvoiceGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				InvoiceGridView.EditCancelled = true;

				SummariesGridView.ClearSelection();
				InvoiceGridView.Rows[e.RowIndex].Selected = true;

				InvoiceGridView.BeginEdit(true);
			}
			catch
			{
			}
		}
	}
}
