using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoiceFreelance
{
	class BankAccount
	{
		private string _Beneficiary;
		private string _Financial_Institute;
		private string _IBAN;
		private string _ABI;
		private string _CAB;
		private string _BIC;

		public BankAccount()
		{
			_Beneficiary         = " ";
			_Financial_Institute = " ";
			_IBAN                = " ";
			_ABI                 = " ";
			_CAB                 = " ";
			_BIC                 = " ";
		}
		public BankAccount(string beneficiary, string financial_institute, string iban, string abi, string cab, string bic)
		{
			_Beneficiary         = beneficiary;
			_Financial_Institute = financial_institute;
			_IBAN                = iban;
			_ABI                 = abi;
			_CAB                 = cab;
			_BIC                 = bic;
		}
		public BankAccount(FatturaElettronicaType invoice)
		{
			_Beneficiary         = invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].Beneficiario;
			_Financial_Institute = invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].IstitutoFinanziario;
			_IBAN                = invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].IBAN;
			_ABI                 = invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].ABI;
			_CAB                 = invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].CAB;
			_BIC                 = invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].BIC;
		}
		public BankAccount(BankAccount bank_account)
		{
			_Beneficiary         = bank_account.Beneficiary;
			_Financial_Institute = bank_account.FinancialInstitute;
			_IBAN                = bank_account.IBAN;
			_ABI                 = bank_account.ABI;
			_CAB                 = bank_account.CAB;
			_BIC                 = bank_account.BIC;
		}
		public void GetFromInvoice(FatturaElettronicaType invoice)
		{
			_Beneficiary         = invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].Beneficiario;
			_Financial_Institute = invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].IstitutoFinanziario;
			_IBAN                = invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].IBAN;
			_ABI                 = invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].ABI;
			_CAB                 = invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].CAB;
			_BIC                 = invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].BIC;
		}
		public void SetToInvoice(ref FatturaElettronicaType invoice)
		{
			invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].Beneficiario        = _Beneficiary;
			invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].IstitutoFinanziario = _Financial_Institute;
			invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].IBAN                = _IBAN;
			invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].ABI                 = _ABI;
			invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].CAB                 = _CAB;
			invoice.FatturaElettronicaBody[0].DatiPagamento[0].DettaglioPagamento[0].BIC                 = _BIC;
		}
		public bool IsEmpty()
		{
			return(string.IsNullOrWhiteSpace(_Beneficiary)         || string.IsNullOrEmpty(_Beneficiary) ||
			       string.IsNullOrWhiteSpace(_Financial_Institute) || string.IsNullOrEmpty(_Financial_Institute) ||
			       string.IsNullOrWhiteSpace(_IBAN)                || string.IsNullOrEmpty(_IBAN) ||
			       string.IsNullOrWhiteSpace(_ABI)                 || string.IsNullOrEmpty(_ABI) ||
			       string.IsNullOrWhiteSpace(_CAB)                 || string.IsNullOrEmpty(_CAB) ||
			       string.IsNullOrWhiteSpace(_BIC)                 || string.IsNullOrEmpty(_BIC));
		}
		public string Beneficiary
		{
			get
			{
				return (_Beneficiary);
			}
		}
		public string FinancialInstitute
		{
			get
			{
				return (_Financial_Institute);
			}
		}
		public string IBAN
		{
			get
			{
				return (_IBAN);
			}
		}
		public string ABI
		{
			get
			{
				return (_ABI);
			}
		}
		public string CAB
		{
			get
			{
				return (_CAB);
			}
		}
		public string BIC
		{
			get
			{
				return (_BIC);
			}
		}
	};
}
