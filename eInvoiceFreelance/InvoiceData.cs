using System;
using System.IO;
using System.Xml.Serialization;
using eInvoiceFreelance.Serializer;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace eInvoiceFreelance
{
	class Invoice
	{
		private FatturaElettronicaType _Invoice;
		private Supplier               _Supplier;
		private Customer               _Customer;
		private BankAccount            _Bank_Account;
		private Tax                    _Tax;
		private ActivityList           _Activity_List;
		private ActivityField          _Init_Activity;
		private Reimbursment           _Reimbursment;
		private Summary                _Summary;
		private RevenueStamp           _Revenue_Stamp;

		public Invoice(FatturaElettronicaType invoice, Reimbursment reimbursment, ActivityField init_activity, RevenueStamp revenue_stamp, BankAccount bank_account = null, Tax tax = null)
		{
			_Invoice       = invoice;
			_Reimbursment  = reimbursment;
			_Init_Activity = init_activity;
			_Revenue_Stamp = revenue_stamp;

			Initialize(init_activity, bank_account, tax);
		}

		public Invoice(string file_name, Reimbursment reimbursment, ActivityField init_activity, RevenueStamp revenue_stamp, BankAccount bank_account = null, Tax tax = null)
		{
			FatturaElettronicaType invoice;
			XmlSerializer serializer;
			StreamReader reader;

			invoice = new FatturaElettronicaType();
			serializer = new XmlSerializer(typeof(FatturaElettronicaType));
			reader = new StreamReader(file_name);
			invoice = (FatturaElettronicaType)serializer.Deserialize(reader);
			reader.Close();

			_Invoice       = invoice;
			_Reimbursment  = reimbursment;
			_Init_Activity = init_activity;
			_Revenue_Stamp = revenue_stamp;

			Initialize(init_activity, bank_account, tax);
		}

		private void Initialize(ActivityField init_activity, BankAccount bank_account, Tax tax)
		{
			try
			{
				if (bank_account == null)
				{
					_Bank_Account = new BankAccount(_Invoice);
				}
				else
				{
					_Bank_Account = new BankAccount(bank_account);

					_Invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].Beneficiario        = _Bank_Account.Beneficiary;
					_Invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].IstitutoFinanziario = _Bank_Account.FinancialInstitute;
					_Invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].IBAN                = _Bank_Account.IBAN;
					_Invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].ABI                 = _Bank_Account.ABI;
					_Invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].CAB                 = _Bank_Account.CAB;
					_Invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].BIC                 = _Bank_Account.BIC;
				}
			}
			catch
			{
				_Bank_Account = new BankAccount();
			}

			try
			{
				_Customer = new Customer(_Invoice);

			}
			catch
			{
				_Customer = new Customer();
			}

			try
			{
				_Supplier = new Supplier(_Invoice);

			}
			catch
			{
				_Supplier = new Supplier();
			}

			try
			{
				if (tax == null)
				{
					_Tax = new Tax(_Invoice);
				}
				else
				{
					_Tax = new Tax(tax);

					_Invoice.FatturaElettronicaBody[0].DatiBeniServizi.DatiRiepilogo[0].AliquotaIVA                     = _Tax.VatPercent;
					_Invoice.FatturaElettronicaBody[0].DatiGenerali.DatiGeneraliDocumento.DatiRitenuta.AliquotaRitenuta = _Tax.WithholdingTaxPercent;
				}
			}
			catch
			{
				_Tax = new Tax();
			}

			try
			{
				_Revenue_Stamp = new RevenueStamp(_Invoice);
			}
			catch
			{
				_Revenue_Stamp = new RevenueStamp();
			}


			if (_Activity_List != null)
			{
				_Activity_List.OnChanged -= OnActivityListUpdate;
			}

			_Activity_List = new ActivityList(init_activity);

			_Activity_List.FromInvoice(_Invoice, _Reimbursment, _Revenue_Stamp);

			if (_Revenue_Stamp.Enable)
			{
				_Invoice.FatturaElettronicaBody[0].DatiGenerali.DatiGeneraliDocumento.DatiBollo = new DatiBolloType();
				_Invoice.FatturaElettronicaBody[0].DatiGenerali.DatiGeneraliDocumento.DatiBollo.BolloVirtuale = BolloVirtualeType.SI;
				_Invoice.FatturaElettronicaBody[0].DatiGenerali.DatiGeneraliDocumento.DatiBollo.ImportoBollo = _Revenue_Stamp.Price;
			}

			try
			{
				_Summary = new Summary(_Activity_List.ToArray(), _Reimbursment, _Tax, _Revenue_Stamp);
			}
			catch
			{
				_Summary = new Summary();
			}

			if (_Activity_List != null)
			{
				_Activity_List.OnChanged += OnActivityListUpdate;
			}

		}

		void OnActivityListUpdate(object sender, EventArgs e)
		{
			ActivityListUpdate();
		}

		public void ActivityListUpdate()
		{
			try
			{
				_Summary = new Summary(_Activity_List.ToArray(), _Reimbursment, _Tax, _Revenue_Stamp);
			}
			catch
			{
				_Summary = new Summary();
			}

			_Invoice.FatturaElettronicaBody[0].DatiBeniServizi.DettaglioLinee = _Activity_List.ToInvoice(_Reimbursment, Tax, _Revenue_Stamp);

			_Invoice.FatturaElettronicaBody[0].DatiBeniServizi.DatiRiepilogo[0].ImponibileImporto = _Summary.TotalTaxable;
			_Invoice.FatturaElettronicaBody[0].DatiBeniServizi.DatiRiepilogo[0].Imposta = _Summary.TotalVat;
			_Invoice.FatturaElettronicaBody[0].DatiGenerali.DatiGeneraliDocumento.DatiRitenuta.ImportoRitenuta = _Summary.TotalWithholdingTax;
			_Invoice.FatturaElettronicaBody[0].DatiGenerali.DatiGeneraliDocumento.ImportoTotaleDocumento = _Summary.Total;
			_Invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].ImportoPagamento = _Summary.ToPay;
		}
		public void GenerateXML(string file_name, decimal number, DateTime date_time)
		{
			XmlSerializer invoice_serializer;
			XmlSerializerNamespaces name_space;
			StreamWriter writer;

			try
			{
				_Invoice.FatturaElettronicaBody[0].DatiGenerali.DatiGeneraliDocumento.Data = date_time;
				_Invoice.FatturaElettronicaBody[0].DatiGenerali.DatiGeneraliDocumento.Numero = number.ToString();
				_Invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].DataRiferimentoTerminiPagamento = date_time;
				_Invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].DataScadenzaPagamentoSpecified = true;
				_Invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].DataScadenzaPagamento = date_time;
			}
			catch
			{
			}

			invoice_serializer = new XmlSerializer(typeof(FatturaElettronicaType));

			writer = new StreamWriter(file_name);

			name_space = new XmlSerializerNamespaces();
			name_space.Add("ns2", "http://ivaservizi.agenziaentrate.gov.it/docs/xsd/fatture/v1.2");

			invoice_serializer.SerializeWithDecimalFormatting(writer, _Invoice, name_space);

			writer.Close();
		}

		public void GeneratePDF(string file_name, string title, decimal number, DateTime date_time)
		{
			FileStream pdf_file;
			Document pdf_document;
			PdfWriter pdf_writer;
			PdfPTable table;
			PdfPCell empty_cell;
			iTextSharp.text.Font title_font;
			iTextSharp.text.Font large_font;
			iTextSharp.text.Font small_font;

			title_font = new iTextSharp.text.Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLDOBLIQUE, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 24, iTextSharp.text.Font.NORMAL);
			large_font = new iTextSharp.text.Font(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 14, iTextSharp.text.Font.NORMAL);
			small_font = new iTextSharp.text.Font(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 8, iTextSharp.text.Font.NORMAL);

			pdf_file = new FileStream(file_name, FileMode.Create);

			pdf_document = new Document(PageSize.A4, 25, 25, 25, 25);
			pdf_writer = PdfWriter.GetInstance(pdf_document, pdf_file);

			pdf_document.AddAuthor(_Supplier.Name);
			pdf_document.AddCreator(App.Name + " " + App.Version);
			pdf_document.AddTitle(_Supplier.VatId + " " + title + " " + number);
			pdf_document.AddSubject(_Supplier.VatId + " " + title + " " + number);

			pdf_document.Open();

			try
			{
				// Supplier
				table = new PdfPTable(2);
				table.WidthPercentage = 100.0f;
				table.HorizontalAlignment = Element.ALIGN_LEFT;
				table.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
				table.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
				table.AddCell(new Phrase(new Chunk(_Supplier.Name, large_font)));
				table.AddCell("");
				table.AddCell(new Phrase(new Chunk(_Supplier.Address, small_font)));
				table.AddCell(new Phrase(new Chunk("C.F.\t" + _Supplier.FiscalId, small_font)));
				table.AddCell(new Phrase(new Chunk(_Supplier.City, small_font)));
				table.AddCell(new Phrase(new Chunk("P.IVA\t" + _Supplier.VatId, small_font)));
				pdf_document.Add(table);

				// Title
				table = new PdfPTable(1);
				table.WidthPercentage = 100.0f;
				table.HorizontalAlignment = Element.ALIGN_CENTER;
				table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
				table.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
				table.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
				table.AddCell(new Phrase(new Chunk(" ", title_font)));
				table.AddCell(new Phrase(new Chunk(title + " " + number, title_font)));
				table.AddCell(new Phrase(new Chunk("del", large_font)));
				table.AddCell(new Phrase(new Chunk(date_time.ToString("dd/MM/yyyy"), large_font)));
				table.AddCell(new Phrase(new Chunk(" ", title_font)));
				pdf_document.Add(table);

				// Customer
				table = new PdfPTable(2);
				table.TotalWidth = pdf_document.PageSize.Width;
				table.SetWidthPercentage(new float[] { table.TotalWidth * 8.0f / 100.0f, table.TotalWidth * 92.0f / 100.0f }, pdf_document.PageSize);
				table.HorizontalAlignment = Element.ALIGN_LEFT;
				table.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
				table.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
				table.AddCell(new Phrase(new Chunk("spettabile:", small_font)));
				table.AddCell(new Phrase(new Chunk(_Customer.Name, large_font)));
				table.AddCell("");
				table.AddCell(new Phrase(new Chunk(_Customer.Address, small_font)));
				table.AddCell("");
				table.AddCell(new Phrase(new Chunk("C.F.\t" + _Customer.FiscalId, small_font)));
				table.AddCell("");
				table.AddCell(new Phrase(new Chunk(_Customer.City, small_font)));
				table.AddCell("");
				table.AddCell(new Phrase(new Chunk("P.IVA\t" + _Customer.VatId, small_font)));
				pdf_document.Add(table);

				// Description
				table = new PdfPTable(4);
				table.TotalWidth = pdf_document.PageSize.Width;
				table.SetWidthPercentage(new float[] { table.TotalWidth * 8.0f / 100.0f, table.TotalWidth * 57.0f / 100.0f, table.TotalWidth * 23.0f / 100.0f, table.TotalWidth * 12.0f / 100.0f }, pdf_document.PageSize);
				table.HorizontalAlignment = Element.ALIGN_CENTER;
				table.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
				table.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
				table.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
				table.DefaultCell.FixedHeight = 12.0f;
				table.AddCell(" ");
				table.AddCell(" ");
				table.AddCell(" ");
				table.AddCell(" ");
				foreach (DettaglioLineeType l in _Invoice.FatturaElettronicaBody[0].DatiBeniServizi.DettaglioLinee)
				{
					table.AddCell(" ");
					table.AddCell(new Phrase(new Chunk(l.Descrizione, small_font)));
					table.AddCell(new Phrase(new Chunk(string.Format("€ {0:0.00}", l.PrezzoTotale), small_font)));
					table.AddCell(" ");
				}
				table.AddCell(" ");
				table.AddCell(" ");
				table.AddCell(" ");
				table.AddCell(" ");
				table.AddCell(" ");
				table.AddCell(" ");
				table.AddCell(" ");
				table.AddCell(" ");

				pdf_document.Add(table);

				// Bill
				table = new PdfPTable(3);
				table.TotalWidth = pdf_document.PageSize.Width * 50.0f / 100.0f;
				table.SetWidthPercentage(new float[] { table.TotalWidth * 45.0f / 100.0f, table.TotalWidth * 40.0f / 100.0f, table.TotalWidth * 25.0f / 100.0f }, pdf_document.PageSize);
				table.HorizontalAlignment = Element.ALIGN_CENTER;
				table.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
				table.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
				table.DefaultCell.FixedHeight = 12.0f;
				empty_cell = new PdfPCell(new Phrase(new Chunk(" ", small_font)));
				empty_cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
				table.AddCell(empty_cell);
				table.AddCell(new Phrase(new Chunk("Imponibile", small_font)));
				table.AddCell(new Phrase(new Chunk(string.Format("€ {0:0.00}", _Summary.TotalTaxable), small_font)));
				table.AddCell(empty_cell);
				table.AddCell(new Phrase(new Chunk(string.Format("Iva {0:0.00}%", _Tax.VatPercent), small_font)));
				table.AddCell(new Phrase(new Chunk(string.Format("€ {0:0.00}", _Summary.TotalVat), small_font)));
				table.AddCell(empty_cell);
				table.AddCell(new Phrase(new Chunk("Totale", small_font)));
				table.AddCell(new Phrase(new Chunk(string.Format("€ {0:0.00}", _Summary.Total), small_font)));
				table.AddCell(empty_cell);
				table.AddCell(new Phrase(new Chunk(string.Format("Ritenuta d'accontro {0:0.00}%", _Tax.WithholdingTaxPercent), small_font)));
				table.AddCell(new Phrase(new Chunk(string.Format("€ {0:0.00}", _Summary.TotalWithholdingTax), small_font)));
				table.AddCell(empty_cell);
				table.AddCell(new Phrase(new Chunk("Totale", small_font)));
				table.AddCell(new Phrase(new Chunk(string.Format("€ {0:0.00}", _Summary.ToPay), small_font)));
				pdf_document.Add(table);

				// Bank Account
				table = new PdfPTable(3);
				table.TotalWidth = pdf_document.PageSize.Width;
				table.SetWidthPercentage(new float[] { table.TotalWidth * 8.0f / 100.0f, table.TotalWidth * 16.0f / 100.0f, table.TotalWidth * 76.0f / 100.0f }, pdf_document.PageSize);
				table.HorizontalAlignment = Element.ALIGN_LEFT;
				table.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
				table.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
				table.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
				table.DefaultCell.FixedHeight = 12.0f;
				table.AddCell(" ");
				table.AddCell(" ");
				table.AddCell(" ");
				table.AddCell(" ");
				table.AddCell(" ");
				table.AddCell(" ");
				table.AddCell(" ");
				table.AddCell(new Phrase(new Chunk("Beneficiario", small_font)));
				table.AddCell(new Phrase(new Chunk(_Bank_Account.Beneficiary, small_font)));
				table.AddCell(" ");
				table.AddCell(new Phrase(new Chunk("Istituto finanziario", small_font)));
				table.AddCell(new Phrase(new Chunk(_Bank_Account.FinancialInstitute, small_font)));
				table.AddCell(" ");
				table.AddCell(new Phrase(new Chunk("IBAN", small_font)));
				table.AddCell(new Phrase(new Chunk(_Bank_Account.IBAN, small_font)));
				table.AddCell(" ");
				table.AddCell(new Phrase(new Chunk("ABI", small_font)));
				table.AddCell(new Phrase(new Chunk(_Bank_Account.ABI, small_font)));
				table.AddCell(" ");
				table.AddCell(new Phrase(new Chunk("CAB", small_font)));
				table.AddCell(new Phrase(new Chunk(_Bank_Account.CAB, small_font)));
				table.AddCell(" ");
				table.AddCell(new Phrase(new Chunk("BIC", small_font)));
				table.AddCell(new Phrase(new Chunk(_Bank_Account.BIC, small_font)));
				pdf_document.Add(table);
			}
			catch
			{
			}

			pdf_document.Close();
			pdf_writer.Close();
			pdf_file.Close();
		}

		public ActivityList ActivityList
		{
			get
			{
				return (_Activity_List);
			}
		}
		public BankAccount BankAccount
		{
			get
			{
				return (_Bank_Account);
			}
		}
		public Customer Customer
		{
			get
			{
				return (_Customer);
			}
		}
		public Reimbursment Reimbursment
		{
			get
			{
				return (_Reimbursment);
			}
		}
		public Summary Summary
		{
			get
			{
				return (_Summary);
			}
		}
		public Supplier Supplier
		{
			get
			{
				return (_Supplier);
			}
		}
		public Tax Tax
		{
			get
			{
				return (_Tax);
			}
		}

		public RevenueStamp RevenueStamp
		{
			get
			{
				return (_Revenue_Stamp);
			}
			set
			{
				_Revenue_Stamp = value;
				if (_Revenue_Stamp.Enable)
				{
					_Invoice.FatturaElettronicaBody[0].DatiGenerali.DatiGeneraliDocumento.DatiBollo = new DatiBolloType();
					_Invoice.FatturaElettronicaBody[0].DatiGenerali.DatiGeneraliDocumento.DatiBollo.BolloVirtuale = BolloVirtualeType.SI;
					_Invoice.FatturaElettronicaBody[0].DatiGenerali.DatiGeneraliDocumento.DatiBollo.ImportoBollo = _Revenue_Stamp.Price;
				}
				else
				{
					_Invoice.FatturaElettronicaBody[0].DatiGenerali.DatiGeneraliDocumento.DatiBollo = null;
				}
			}
		}
	}
}
