using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoiceFreelance
{
	class Customer
	{
		private string _Name;
		private string _Address;
		private string _City;
		private string _Fiscal_Id;
		private string _Vat_Id;

		public Customer()
		{
			_Name      = " ";
			_Address   = " ";
			_City      = " ";
			_Fiscal_Id = " ";
			_Vat_Id    = " ";
		}
		public Customer(FatturaElettronicaType invoice)
		{
			_Name = "";
			foreach (string i in invoice.FatturaElettronicaHeader.CessionarioCommittente.DatiAnagrafici.Anagrafica.Items)
			{
				_Name += i + " ";
			}
			_Address   = invoice.FatturaElettronicaHeader.CessionarioCommittente.Sede.Indirizzo;
			_City      = invoice.FatturaElettronicaHeader.CessionarioCommittente.Sede.CAP + " " +
			             invoice.FatturaElettronicaHeader.CessionarioCommittente.Sede.Comune + " " +
			             invoice.FatturaElettronicaHeader.CessionarioCommittente.Sede.Provincia + " " +
			             invoice.FatturaElettronicaHeader.CessionarioCommittente.Sede.Nazione;
			_Fiscal_Id = invoice.FatturaElettronicaHeader.CessionarioCommittente.DatiAnagrafici.CodiceFiscale;
			_Vat_Id    = (invoice.FatturaElettronicaHeader.CessionarioCommittente.DatiAnagrafici.IdFiscaleIVA.IdPaese +
			              invoice.FatturaElettronicaHeader.CessionarioCommittente.DatiAnagrafici.IdFiscaleIVA.IdCodice);
		}
		public Customer(Customer customer)
		{
			_Name      = customer.Name;
			_Address   = customer.Address;
			_City      = customer.City;
			_Fiscal_Id = customer.FiscalId;
			_Vat_Id    = customer.VatId;
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
			foreach (string i in invoice.FatturaElettronicaHeader.CessionarioCommittente.DatiAnagrafici.Anagrafica.Items)
			{
				_Name += i + " ";
			}
			_Address   = invoice.FatturaElettronicaHeader.CessionarioCommittente.Sede.Indirizzo;
			_City      = invoice.FatturaElettronicaHeader.CessionarioCommittente.Sede.CAP + " " +
			             invoice.FatturaElettronicaHeader.CessionarioCommittente.Sede.Comune + " " +
			             invoice.FatturaElettronicaHeader.CessionarioCommittente.Sede.Provincia + " " +
			             invoice.FatturaElettronicaHeader.CessionarioCommittente.Sede.Nazione;
			_Fiscal_Id = invoice.FatturaElettronicaHeader.CessionarioCommittente.DatiAnagrafici.CodiceFiscale;
			_Vat_Id    = (invoice.FatturaElettronicaHeader.CessionarioCommittente.DatiAnagrafici.IdFiscaleIVA.IdPaese +
			              invoice.FatturaElettronicaHeader.CessionarioCommittente.DatiAnagrafici.IdFiscaleIVA.IdCodice);
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
