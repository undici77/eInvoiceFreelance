using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoiceFreelance
{
	class Supplier
	{
		private string _Name;
		private string _Address;
		private string _City;
		private string _Fiscal_Id;
		private string _Vat_Id;

		public Supplier()
		{
			_Name      = " ";
			_Address   = " ";
			_City      = " ";
			_Fiscal_Id = " ";
			_Vat_Id    = " ";
		}
		public Supplier(FatturaElettronicaType invoice)
		{
			_Name = "";
			foreach (string i in invoice.FatturaElettronicaHeader.CedentePrestatore.DatiAnagrafici.Anagrafica.Items)
			{
				_Name += i + " ";
			}
			_Address   = invoice.FatturaElettronicaHeader.CedentePrestatore.Sede.Indirizzo;
			_City      = invoice.FatturaElettronicaHeader.CedentePrestatore.Sede.CAP + " " +
			             invoice.FatturaElettronicaHeader.CedentePrestatore.Sede.Comune + " " +
			             invoice.FatturaElettronicaHeader.CedentePrestatore.Sede.Provincia + " " +
			             invoice.FatturaElettronicaHeader.CedentePrestatore.Sede.Nazione;
			_Fiscal_Id = invoice.FatturaElettronicaHeader.CedentePrestatore.DatiAnagrafici.CodiceFiscale;
			_Vat_Id    = (invoice.FatturaElettronicaHeader.CedentePrestatore.DatiAnagrafici.IdFiscaleIVA.IdPaese +
			              invoice.FatturaElettronicaHeader.CedentePrestatore.DatiAnagrafici.IdFiscaleIVA.IdCodice);
		}
		public Supplier(Supplier supplier)
		{
			_Name      = supplier.Name;
			_Address   = supplier.Address;
			_City      = supplier.City;
			_Fiscal_Id = supplier.FiscalId;
			_Vat_Id    = supplier.VatId;
		}
		public bool IsEmpty()
		{
			return(string.IsNullOrWhiteSpace(_Name)      || string.IsNullOrEmpty(_Name) ||
			       string.IsNullOrWhiteSpace(_Address)   || string.IsNullOrEmpty(_Address) ||
			       string.IsNullOrWhiteSpace(_City)      || string.IsNullOrEmpty(_City) ||
			       string.IsNullOrWhiteSpace(_Fiscal_Id) || string.IsNullOrEmpty(_Fiscal_Id) ||
			       string.IsNullOrWhiteSpace(_Vat_Id)    || string.IsNullOrEmpty(_Vat_Id));
		}
		public void GetFromInvoice(FatturaElettronicaType invoice)
		{
			_Name = "";
			foreach (string i in invoice.FatturaElettronicaHeader.CedentePrestatore.DatiAnagrafici.Anagrafica.Items)
			{
				_Name += i + " ";
			}
			_Address   = invoice.FatturaElettronicaHeader.CedentePrestatore.Sede.Indirizzo;
			_City      = invoice.FatturaElettronicaHeader.CedentePrestatore.Sede.CAP + " " +
			             invoice.FatturaElettronicaHeader.CedentePrestatore.Sede.Comune + " " +
			             invoice.FatturaElettronicaHeader.CedentePrestatore.Sede.Provincia + " " +
			             invoice.FatturaElettronicaHeader.CedentePrestatore.Sede.Nazione;
			_Fiscal_Id = invoice.FatturaElettronicaHeader.CedentePrestatore.DatiAnagrafici.CodiceFiscale;
			_Vat_Id    = (invoice.FatturaElettronicaHeader.CedentePrestatore.DatiAnagrafici.IdFiscaleIVA.IdPaese +
			              invoice.FatturaElettronicaHeader.CedentePrestatore.DatiAnagrafici.IdFiscaleIVA.IdCodice);
		}
		public string Name
		{
			get
			{
				return (_Name);
			}
		}
		public string Address
		{
			get
			{
				return (_Address);
			}
		}
		public string City
		{
			get
			{
				return (_City);
			}
		}
		public string FiscalId
		{
			get
			{
				return (_Fiscal_Id);
			}
		}
		public string VatId
		{
			get
			{
				return (_Vat_Id);
			}
		}
	};
}
