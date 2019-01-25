using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class DataGridViewEx : DataGridView
{
	private bool Enter_Tab_Pressed;
	public bool EnterTabPressed
	{
		get
		{
			return (Enter_Tab_Pressed);
		}
		set
		{
			Enter_Tab_Pressed = value;
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		EnterTabPressed = false;

		base.OnMouseDown(e);
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys key_data)
	{
		Enter_Tab_Pressed = false;
		if ((key_data == Keys.Enter) || (key_data == Keys.Tab))
		{
			Enter_Tab_Pressed = true;

			EndEdit();

			return (true);
		}

		return (base.ProcessCmdKey(ref msg, key_data));
	}
}
