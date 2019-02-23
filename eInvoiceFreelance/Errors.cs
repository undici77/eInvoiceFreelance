using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

static class Errors
{
	static public void Assert(bool assettion, [CallerFilePath] string file_path = null, [CallerLineNumber] int line_number = 0, [CallerMemberName] string caller = null)
	{
		if (!assettion)
		{
			string message;

			message = "file: " + Path.GetFileName(file_path) + "\nline:" + line_number + "\nmethod:" + caller;

			try
			{
				MessageBox.Show(message, "Assert false", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch
			{
				throw new System.ArgumentException(message, "Assert false");
			}
	
			Application.Exit();			
		}
 	}

	static public void Assert(object obj, [CallerFilePath] string file_path = null, [CallerLineNumber] int line_number = 0, [CallerMemberName] string caller = null)
	{
		if (obj == null)
		{
			string message;

			message = Path.GetFileName(file_path) + " " + line_number + " " + caller;

			try
			{
				MessageBox.Show(message, "Assert false", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch
			{
				throw new System.ArgumentException(message, "Assert false");
			}
	
			Application.Exit();			
		}
 	}
}
