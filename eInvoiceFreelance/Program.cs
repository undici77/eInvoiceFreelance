using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.Reflection;

namespace eInvoiceFreelance
{
	static class App
	{
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool SetForegroundWindow(IntPtr hWnd);

		static MainForm _MainForm;
		static public MainForm Instance
		{
			get
			{
				return (_MainForm);
			}
		}

		static string _Version;
		static public string Version
		{
			get
			{
				return (_Version);
			}
		}

		static string _Name;
		static public string Name
		{
			get
			{
				return (_Name);
			}
		}

		static string _Path;
		static public string Path
		{
			get
			{
				return (_Path);
			}
		}

		[STAThread]
		static void Main()
		{
			bool created;
			string process_name;

			process_name = System.IO.Directory.GetCurrentDirectory() + " - " + Application.ProductName;
			process_name = process_name.Replace("\\", "");
			process_name = process_name.Replace(".", "");
			process_name = process_name.Replace(":", "");

			Assembly assembly = Assembly.GetExecutingAssembly();
			FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
			_Version = fvi.FileVersion;
			_Name = fvi.ProductName;
			_Path = System.IO.Path.GetDirectoryName(fvi.FileName) + "\\";

			created = true;
			using (Mutex mutex = new Mutex(true, process_name, out created))
			{
				if (created)
				{
					Create();
				}
				else
				{
					Process current = Process.GetCurrentProcess();
					foreach (Process process in Process.GetProcessesByName(current.ProcessName))
					{
						if (process.Id != current.Id)
						{
							SetForegroundWindow(process.MainWindowHandle);
							break;
						}
					}
				}
			}
		}

		static void Create()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			_MainForm = new MainForm();
			Application.Run(_MainForm);
			_MainForm = null;
		}
	}
}

