using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoiceFreelance
{
	class Tax
	{
		private decimal _Vat_Percent;
		private decimal _Withholding_Tax_Percent;

		public Tax()
		{
			_Vat_Percent             = 0;
			_Withholding_Tax_Percent = 0;
		}
		public Tax(decimal vat_percent, decimal withholding_tax_percent)
		{
			_Vat_Percent             = vat_percent;
			_Withholding_Tax_Percent = withholding_tax_percent;
		}
		public Tax(FatturaElettronicaType invoice)
		{
			_Vat_Percent             = invoice.FatturaElettronicaBody[0].DatiBeniServizi.DatiRiepilogo[0].AliquotaIVA;
			_Withholding_Tax_Percent = invoice.FatturaElettronicaBody[0].DatiGenerali.DatiGeneraliDocumento.DatiRitenuta.AliquotaRitenuta;
		}
		public Tax(Tax tax)
		{
			_Vat_Percent             = tax.VatPercent;
			_Withholding_Tax_Percent = tax.WithholdingTaxPercent;
		}
/*
		public void GetFromInvoice(FatturaElettronicaType invoice)
		{
			_Vat_Percent             = invoice.FatturaElettronicaBody[0].DatiBeniServizi.DatiRiepilogo[0].AliquotaIVA;
			_Withholding_Tax_Percent = invoice.FatturaElettronicaBody[0].DatiGenerali.DatiGeneraliDocumento.DatiRitenuta.AliquotaRitenuta;
		}
		public void SetToInvoice(ref FatturaElettronicaType invoice)
		{
			invoice.FatturaElettronicaBody[0].DatiBeniServizi.DatiRiepilogo[0].AliquotaIVA                     = _Vat_Percent;
			invoice.FatturaElettronicaBody[0].DatiGenerali.DatiGeneraliDocumento.DatiRitenuta.AliquotaRitenuta = _Withholding_Tax_Percent;
		}
*/
		public decimal VatPercent
		{
			get
			{
				return (_Vat_Percent);
			}
		}
		public decimal WithholdingTaxPercent
		{
			get
			{
				return (_Withholding_Tax_Percent);
			}
		}
	}
}
