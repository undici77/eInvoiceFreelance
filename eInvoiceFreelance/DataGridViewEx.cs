using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class DataGridViewEx : DataGridView
{
	private bool Edit_Cancelled;

	public bool EditCancelled
	{
		get
		{
			return (Edit_Cancelled);
		}
		set
		{
			Edit_Cancelled = value;
		}
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys key_data)
	{
		if ((key_data == Keys.Enter) || (key_data == Keys.Tab))
		{
			Edit_Cancelled = false;

			EndEdit();

			return (true);
		}
		else if (key_data == Keys.Escape)
		{
			Edit_Cancelled = true;
		}

		return (base.ProcessCmdKey(ref msg, key_data));
	}
}
