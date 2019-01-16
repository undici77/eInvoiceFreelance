using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
			public string description;
			public decimal quantity;
			public decimal unit_price;
			public decimal total_price;
			public bool   reimbursement;
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
			CURRENCY_ID,
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
		private List<ACTIVITY_FIELD> Activity_List;
		private SUMMARY Summary;
		private object[] Invoice_Grid_View_Default_Value;
		private IniFile Ini_File;

		public MainForm()
		{
			InitializeComponent();
			
            this.Text = App.Name + " " + App.Version;			
		}

		private void MainFormLoad(object sender, EventArgs e)
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

			Invoice_Grid_View_Default_Value = new object[] { description, quantity.ToString("0.00"), unit_price.ToString("0.00"), total_price.ToString("0.00"), true, vat.ToString("0.00") };

			Ini_File.SetKeyValue("Init", "Description", description);
			Ini_File.SetKeyValue("Init", "Quantity", quantity.ToString("0.00"));
			Ini_File.SetKeyValue("Init", "UnitPrice", unit_price.ToString("0.00"));
			Ini_File.SetKeyValue("Init", "Vat", vat.ToString("0.00"));

			try
			{
				Reimbursment = decimal.Parse(Ini_File.GetKeyValue("Conf", "Reimbursment"));
			}
			catch
			{
				Reimbursment = 4;
				Ini_File.SetKeyValue("Conf", "Reimbursment", Reimbursment.ToString("0.00"));
			}

			InvoiceGridView.Columns[(int)INVOICE_GRID_VIEW_COLUMN_ID.REIMBOURSE_ID].HeaderText = "Reimb. " + Reimbursment.ToString("0.00") + "%";
			InvoiceGridView.Rows.Add(Invoice_Grid_View_Default_Value);
			Activity_List = new List<ACTIVITY_FIELD>();

			SummaryCurrency.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			SummaryValue.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

			SummariesGridView.Rows.Add("Taxable", "€", "");
			SummariesGridView.Rows.Add("Reimbursment " + Reimbursment.ToString("0.00") + "%", "€", "");
			SummariesGridView.Rows.Add("Total Taxable", "€", "");
			SummariesGridView.Rows.Add("Total VAT", "€", "");
			SummariesGridView.Rows.Add("Total", "€", "");
			SummariesGridView.Rows.Add("Withholding Tax", "€", "");
			SummariesGridView.Rows.Add("To Pay", "€", "");

			Ini_File.Save(App.Name + ".ini");
		}

		private void InvoiceGridViewCellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
			decimal value;
			INVOICE_GRID_VIEW_COLUMN_ID row_id;
			DataGridViewRow row;

			row_id = (INVOICE_GRID_VIEW_COLUMN_ID)e.RowIndex;
			row = InvoiceGridView.Rows[e.RowIndex];

			switch (row_id)
			{
				case INVOICE_GRID_VIEW_COLUMN_ID.DESCRIPTION_ID:
					if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
					{
						row.ErrorText = "Description must not be empty";
						e.Cancel = true;
					}
					break;

				case INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID:
					if (decimal.TryParse(e.FormattedValue.ToString(), out value))
					{
						if (value <= 0)
						{
							row.ErrorText = "Quantity must be a number and >0";
							e.Cancel = true;
						}
					}
					break;

				case INVOICE_GRID_VIEW_COLUMN_ID.UNIT_PRICE_ID:
					if (decimal.TryParse(e.FormattedValue.ToString(), out value))
					{
						if (value <= 0)
						{
							row.ErrorText = "Unit price must be a number and >0";
							e.Cancel = true;
						}
					}
					break;

				case INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID:
					if (decimal.TryParse(e.FormattedValue.ToString(), out value))
					{
						if (value <= 0)
						{
							row.ErrorText = "Total price must be a number and >0";
							e.Cancel = true;
						}
					}
					break;

				case INVOICE_GRID_VIEW_COLUMN_ID.VAT_ID:
					if (decimal.TryParse(e.FormattedValue.ToString(), out value))
					{
						if (value <= 0)
						{
							row.ErrorText = "Vat must be a number and >0";
							e.Cancel = true;
						}
					}
					break;

				default:
					throw new NotImplementedException ("Undefined row id");
			}
		}

		private void InvoiceGridViewCellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			ACTIVITY_FIELD              field;
			int                         row_id;
			INVOICE_GRID_VIEW_COLUMN_ID col_id;

			row_id = e.RowIndex;
			col_id = (INVOICE_GRID_VIEW_COLUMN_ID)e.ColumnIndex;

			field.description = "";
			field.quantity = 0;
			field.unit_price = 0;
			field.total_price = 0;
			field.reimbursement = false;
			field.vat = 0;

			try
			{
				field.description = (string)InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.DESCRIPTION_ID].Value;
				field.quantity = decimal.Parse((string)InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID].Value);
				field.unit_price = decimal.Parse((string)InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.UNIT_PRICE_ID].Value);
				field.total_price = decimal.Parse((string)InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID].Value);
				field.reimbursement = (bool)InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.REIMBOURSE_ID].Value;
				field.vat = decimal.Parse((string)InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.VAT_ID].Value);

				if (!string.IsNullOrEmpty(field.description) &&
					(field.quantity > 0) &&
					(field.unit_price > 0) &&
					(field.total_price > 0) &&
					(field.vat > 0))
				{
					if (Activity_List.Count > row_id)
					{
						Activity_List[row_id] = field;
					}
					else
					{
						Activity_List.Add(field);
						InvoiceGridView.Rows.Add(Invoice_Grid_View_Default_Value);
					}

					UpdateSummary();
				}
			}
			catch
			{
			}
		}

		private void InvoiceGridViewCellEndEdit(object sender, DataGridViewCellEventArgs e)
		{								
			ACTIVITY_FIELD field;
			int row_id;
			INVOICE_GRID_VIEW_COLUMN_ID col_id;

			InvoiceGridView.Rows[e.RowIndex].ErrorText = String.Empty;

			row_id = e.RowIndex;
			col_id = (INVOICE_GRID_VIEW_COLUMN_ID)e.ColumnIndex;

			field.quantity    = 0;
			field.unit_price  = 0;
			field.total_price = 0;
			field.vat         = 0;

			try
			{
				field.quantity      = decimal.Parse((string)InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID].Value);
				InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID].Value = field.quantity.ToString("0.00");
			}
			catch
			{
			}

			try
			{
				field.unit_price    = decimal.Parse((string)InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.UNIT_PRICE_ID].Value);
				InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.UNIT_PRICE_ID].Value = field.unit_price.ToString("0.00");
			}
			catch
			{
			}

			try
			{
				field.total_price   = decimal.Parse((string)InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID].Value);
				InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID].Value = field.total_price.ToString("0.00");
			}
			catch
			{
			}

			if ((col_id == INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID) && (field.quantity > 0) && (field.unit_price > 0))
			{
				field.total_price = (field.quantity * field.unit_price);
				InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID].Value = field.total_price.ToString("0.00");
			}
			else if ((col_id == INVOICE_GRID_VIEW_COLUMN_ID.UNIT_PRICE_ID) && (field.quantity > 0) && (field.unit_price > 0))
			{
				field.total_price = (field.quantity * field.unit_price);
				InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID].Value = field.total_price.ToString("0.00");
			}
			else if ((col_id == INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID) && (field.unit_price > 0) && (field.total_price > 0) && (field.quantity == 0))
			{
				field.quantity = (field.total_price / field.unit_price);
				InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID].Value = field.quantity.ToString("0.00");
			}
			else if ((col_id == INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID) && (field.quantity > 0) && (field.total_price > 0))
			{
				field.unit_price = (field.total_price / field.quantity);
				InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.UNIT_PRICE_ID].Value = field.unit_price.ToString("0.00");
			}
			else if ((col_id == INVOICE_GRID_VIEW_COLUMN_ID.TOTAL_PRICE_ID) && (field.unit_price > 0) && (field.total_price > 0))
			{
				field.quantity = (field.total_price / field.unit_price);
				InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.QUANTITY_ID].Value = field.quantity.ToString("0.00");
			}

			try
			{
				field.vat = decimal.Parse((string)InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.VAT_ID].Value);
				InvoiceGridView.Rows[row_id].Cells[(int)INVOICE_GRID_VIEW_COLUMN_ID.VAT_ID].Value = field.vat.ToString("0.00");
			}
			catch
			{
			}
		}

		private void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			InvoiceGridView.CancelEdit();
		}

		private void StripMenuFileCloseClick(object sender, EventArgs e)
		{
			Close();
		}

		private void StripMenuFileOpenClick(object sender, EventArgs e)
		{

		}

		private void StripMenuGenerateClick(object sender, EventArgs e)
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

		private void UpdateSummary()
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

			Summary.total_taxable   = Summary.taxable + Summary.reimbursment;
			Summary.total           = Summary.total_taxable + Summary.total_vat;
			Summary.withholding_tax = ((Summary.total_taxable * 20) / 100);
			Summary.to_pay          = Summary.total - Summary.withholding_tax;

			SummariesGridView.Rows[(int)SUMMARY_GRID_VIEW_ROW_ID.TAXABLE_ID].Cells[(int)SUMMARY_GRID_VIEW_COLUMN_ID.VALUE_ID].Value = Summary.taxable.ToString("0.00");
			SummariesGridView.Rows[(int)SUMMARY_GRID_VIEW_ROW_ID.REIMBURSMENT_ID].Cells[(int)SUMMARY_GRID_VIEW_COLUMN_ID.VALUE_ID].Value = Summary.reimbursment.ToString("0.00");
			SummariesGridView.Rows[(int)SUMMARY_GRID_VIEW_ROW_ID.TOTAL_TAXABLE_ID].Cells[(int)SUMMARY_GRID_VIEW_COLUMN_ID.VALUE_ID].Value = Summary.total_taxable.ToString("0.00");
			SummariesGridView.Rows[(int)SUMMARY_GRID_VIEW_ROW_ID.TOTAL_VAT_ID].Cells[(int)SUMMARY_GRID_VIEW_COLUMN_ID.VALUE_ID].Value = Summary.total_vat.ToString("0.00");
			SummariesGridView.Rows[(int)SUMMARY_GRID_VIEW_ROW_ID.TOTAL_ID].Cells[(int)SUMMARY_GRID_VIEW_COLUMN_ID.VALUE_ID].Value = Summary.total.ToString("0.00");
			SummariesGridView.Rows[(int)SUMMARY_GRID_VIEW_ROW_ID.WITHHOLDING_TAX_ID].Cells[(int)SUMMARY_GRID_VIEW_COLUMN_ID.VALUE_ID].Value = Summary.withholding_tax.ToString("0.00");
			SummariesGridView.Rows[(int)SUMMARY_GRID_VIEW_ROW_ID.TO_PAY_ID].Cells[(int)SUMMARY_GRID_VIEW_COLUMN_ID.VALUE_ID].Value = Summary.to_pay.ToString("0.00");
		}

		private void InvoiceGridViewCellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			InvoiceGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}
	}
}
