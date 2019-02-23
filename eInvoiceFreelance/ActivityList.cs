using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoiceFreelance
{
	class ActivityField
	{
		private string  _Description;
		private decimal _Quantity;
		private decimal _Unit_Price;
		private decimal _Total_Price;
		private bool    _Reimbursement;

		public ActivityField()
		{
			_Description   = "";
			_Quantity      = 0;
			_Unit_Price    = 0;
			_Total_Price   = 0;
			_Reimbursement = false;
		}

        public ActivityField(ActivityField activity_field)
        {
			_Description   = activity_field.Description;
			_Quantity      = activity_field.Quantity;
			_Unit_Price    = activity_field.UnitPrice;
			_Total_Price   = activity_field.TotalPrice;
			_Reimbursement = activity_field.Reimbursement;
        }

		public ActivityField(string description, decimal quantity, decimal unit_price, decimal total_price, bool reimbursement)
		{
			_Description   = description;
			_Quantity      = quantity;
			_Unit_Price    = unit_price;
			_Total_Price   = total_price;
			_Reimbursement = reimbursement;
		}

		public string Description
		{
			get
			{
				return (_Description);
			}
		}
		public decimal Quantity
		{
			get
			{
				return (_Quantity);
			}
		}
		public decimal UnitPrice
		{
			get
			{
				return (_Unit_Price);
			}
		}
		public decimal TotalPrice
		{
			get
			{
				return (_Total_Price);
			}
		}
		public bool Reimbursement
		{
			get
			{
				return (_Reimbursement);
			}
		}
	}

	class ActivityList
	{
		public event EventHandler OnChanged;

		private ActivityField _Init_Activity;
		private List<ActivityField> _List;

		public ActivityList(ActivityField init_activity)
		{
			_Init_Activity = new ActivityField(init_activity);
			_List = new List<ActivityField>();
		}

		public ActivityField this[int index] 
		{ 
			get
			{
				return(_List[index]);
			} 
			
			set
			{
				_List[index] = value;
				if (OnChanged != null)
				{
					OnChanged(this, null);
				}
			}
		}

		public void New()
		{
			_List.Add(new ActivityField(_Init_Activity));
			if (OnChanged != null)
			{
				OnChanged(this, null);
			}
		}

		public void Add(ActivityField item)
		{
			_List.Add(item);
			if (OnChanged != null)
			{
				OnChanged(this, null);
			}
		}

		public void AddRange(IEnumerable<ActivityField> collection)
		{
			_List.AddRange(collection);
			if (OnChanged != null)
			{
				OnChanged(this, null);
			}
		}

		public void Clear()
		{
			_List.Clear();
			if (OnChanged != null)
			{
				OnChanged(this, null);
			}
		}

		public void Insert(int index, ActivityField item)
		{
			_List.Insert(index, item);
			if (OnChanged != null)
			{
				OnChanged(this, null);
			}
		}

		public void InsertRange(int index, IEnumerable<ActivityField> collection)
		{
			_List.InsertRange(index, collection);
			if (OnChanged != null)
			{
				OnChanged(this, null);
			}
		}

		public bool Remove(ActivityField item)
		{
			bool result;

			result = _List.Remove(item);
			if (OnChanged != null)
			{
				OnChanged(this, null);
			}

			return(result);
		}

		public int RemoveAll(Predicate<ActivityField> match)
		{
			int result;

			result = _List.RemoveAll(match);
			if (OnChanged != null)
			{
				OnChanged(this, null);
			}

			return(result);
		}

		public void RemoveAt(int index)
		{
			_List.RemoveAt(index);
			if (OnChanged != null)
			{
				OnChanged(this, null);
			}
		}

		public void RemoveRange(int index, int count)
		{
			_List.RemoveRange(index, count);
			if (OnChanged != null)
			{
				OnChanged(this, null);
			}
		}

		public ActivityField[] ToArray()
		{
			return(_List.ToArray());
		}

		public void FromInvoice(FatturaElettronicaType invoice, Reimbursment reimbursment)
		{
			int                  id;
			int                  lines_number;
			DettaglioLineeType[] invoice_lines;
			DettaglioLineeType   line;
			DettaglioLineeType   field;
			ActivityField        activity;

			_List.Clear();

			invoice_lines = invoice.FatturaElettronicaBody[0].DatiBeniServizi.DettaglioLinee;
			lines_number = invoice_lines.Length;

			id = 0;
			line = null;
			field = null;
			while (id < lines_number)
			{
				if (line == null)
				{
					line = invoice_lines[id];
					id++;
				}
				
				if (line.Descrizione != null)
				{
					if (line.Descrizione.Contains(reimbursment.Pattern) && (field != null))
					{
						activity = new ActivityField(field.Descrizione, field.Quantita, field.PrezzoUnitario, field.PrezzoTotale, true);
						_List.Add(activity);

						field = null;
						line = null;
					}
					else if (field == null)
					{
						field = line;
						line = null;
					}
					else
					{
						activity = new ActivityField(field.Descrizione, field.Quantita, field.PrezzoUnitario, field.PrezzoTotale, false);
						_List.Add(activity);

						field = null;
					}
				}
			}

			if (field != null)
			{
				if (field.Descrizione != null)
				{
					activity = new ActivityField(field.Descrizione, field.Quantita, field.PrezzoUnitario, field.PrezzoTotale, false);
					_List.Add(activity);

					field = null;
					line = null;
				}
			}

			if (OnChanged != null)
			{
				OnChanged(this, null);
			}
		}

		public DettaglioLineeType[] ToInvoice(Reimbursment reimbursment, Tax tax)
		{
			DettaglioLineeType bill_filed;
			List<DettaglioLineeType> invoice_lines;

			int line_number;

			invoice_lines = new List<DettaglioLineeType>();

			line_number = 1;
			foreach (ActivityField field in _List)
			{
				bill_filed = new DettaglioLineeType();
				bill_filed.NumeroLinea = line_number.ToString();
				bill_filed.Descrizione = field.Description;
				bill_filed.QuantitaSpecified = true;
				bill_filed.Quantita = field.Quantity;
				bill_filed.PrezzoUnitario = field.UnitPrice;
				bill_filed.PrezzoTotale = field.TotalPrice;
				bill_filed.AliquotaIVA = tax.VatPercent;

				invoice_lines.Add(bill_filed);
				line_number++;

				if (field.Reimbursement)
				{
					bill_filed = new DettaglioLineeType();
					bill_filed.NumeroLinea = line_number.ToString();
					bill_filed.Descrizione = field.Description + reimbursment.Pattern;
					bill_filed.QuantitaSpecified = true;
					bill_filed.Quantita = 1;
					bill_filed.PrezzoUnitario = (field.TotalPrice * reimbursment.Percent) / 100;
					bill_filed.PrezzoTotale = (field.TotalPrice * reimbursment.Percent) / 100;
					bill_filed.AliquotaIVA = tax.VatPercent;

					invoice_lines.Add(bill_filed);
					line_number++;
				}
			}

			return (invoice_lines.ToArray());
		}
	}
}
