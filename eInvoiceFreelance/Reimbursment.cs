using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoiceFreelance
{
	class Reimbursment
	{
		public decimal _Percent;
		public string _Description;
		public string _Pattern;

		public Reimbursment()
		{
			_Percent     = 0;
			_Description = "";
			_Pattern     = "";
		}
		public Reimbursment(decimal percent, string description)
		{
			_Percent     = percent;
			_Description = description;
			_Pattern     = " (" + _Description + " " + _Percent.ToString() + "%)";
		}
		public decimal Percent
		{
			get
			{
				return (_Percent);
			}
		}
		public string Description
		{
			get
			{
				return (_Description);
			}
		}
		public string Pattern
		{
			get
			{
				return (_Pattern);
			}
		}
	}
}
