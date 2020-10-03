using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoiceFreelance
{
	class Summary
	{
		private decimal _Taxable;
		private decimal _Reimbursment;
		private decimal _Total_Taxable;
		private decimal _Total_Vat;
		private decimal _Total;
		private decimal _Total_Withholding_Tax;
		private decimal _To_Pay;

		public Summary()
		{
			_Taxable               = 0;
			_Reimbursment          = 0;
			_Total_Taxable         = 0;
			_Total_Vat             = 0;
			_Total                 = 0;
			_Total_Withholding_Tax = 0;
			_To_Pay                = 0;
		}

		public Summary(ActivityField[] activities, Reimbursment reimbursment, Tax tax, RevenueStamp revenue_stamp)
		{
			decimal reimbursment_value;
			decimal vat;

			_Taxable               = 0;
			_Reimbursment          = 0;
			_Total_Taxable         = 0;
			_Total_Vat             = 0;
			_Total                 = 0;
			_Total_Withholding_Tax = 0;
			_To_Pay                = 0;

			foreach (ActivityField field in activities)
			{
				_Taxable += field.TotalPrice;

				reimbursment_value = 0;
				if (field.Reimbursement)
				{
					reimbursment_value = ((field.TotalPrice * reimbursment.Percent) / 100);
				}

				_Reimbursment += reimbursment_value;

				vat = 0;
				if (field.VatEnable)
				{
					vat = (((field.TotalPrice + reimbursment_value) * tax.VatPercent) / 100);
				}

				_Total_Vat += vat;
			}

			_Total_Taxable         = _Taxable + _Reimbursment;
			_Total                 = _Total_Taxable + _Total_Vat;
			_Total_Withholding_Tax = ((_Total_Taxable * tax.WithholdingTaxPercent) / 100);
			_To_Pay                = _Total - _Total_Withholding_Tax;

			if (revenue_stamp.Enable)
			{
				_To_Pay += revenue_stamp.Price; 
			}
		}

		public decimal Taxable
		{
			get
			{
				return (_Taxable);
			}
		}
		public decimal Reimbursment
		{
			get
			{
				return (_Reimbursment);
			}
		}
		public decimal TotalTaxable
		{
			get
			{
				return (_Total_Taxable);
			}
		}
		public decimal TotalVat
		{
			get
			{
				return (_Total_Vat);
			}
		}
		public decimal Total
		{
			get
			{
				return (_Total);
			}
		}
		public decimal TotalWithholdingTax
		{
			get
			{
				return (_Total_Withholding_Tax);
			}
		}
		public decimal ToPay
		{
			get
			{
				return (_To_Pay);
			}
		}
	}
}
