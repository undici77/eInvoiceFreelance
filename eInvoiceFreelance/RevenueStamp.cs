using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoiceFreelance
{
	class RevenueStamp
	{		
		private bool    _Enable;
		private decimal _Price;

		public RevenueStamp()
		{
			_Price  = 0;
			_Enable = false;
		}
		public RevenueStamp(bool enable, decimal price)
		{
			_Enable = enable;
			_Price  = price;
		}
		public RevenueStamp(FatturaElettronicaType invoice)
		{
			if (invoice.FatturaElettronicaBody[0].DatiGenerali.DatiGeneraliDocumento.DatiBollo != null)
			{
				_Enable = (invoice.FatturaElettronicaBody[0].DatiGenerali.DatiGeneraliDocumento.DatiBollo.BolloVirtuale == BolloVirtualeType.SI);
				_Price  = invoice.FatturaElettronicaBody[0].DatiGenerali.DatiGeneraliDocumento.DatiBollo.ImportoBollo;
			}
			else
			{
				_Enable = false;
				_Price  = 0;
			}
		}
		public RevenueStamp(RevenueStamp revenue_stamp)
		{
			_Enable = revenue_stamp.Enable;
			_Price  = revenue_stamp.Price;
		}
		public bool Enable
		{
			get
			{
				return (_Enable);
			}
		}

		public decimal Price
		{
			get
			{
				return (_Price);
			}
		}

		static public string Pattern
		{
			get
			{
				return ("Bollo");
			}
		}
	}
}
