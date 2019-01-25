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

		static MainForm Main_Form;
		static public MainForm Instance
		{
			get
			{
				return (Main_Form);
			}
		}

		static string Software_Version;
		static public string Version
		{
			get
			{
				return (Software_Version);
			}
		}

		static string Software_Name;
		static public string Name
		{
			get
			{
				return (Software_Name);
			}
		}

		static string Exe_Path;
		static public string Path
		{
			get
			{
				return (Exe_Path);
			}
		}

		static OperatingSystem Operating_System;
		static public OperatingSystem OperatingSystem
		{
			get
			{
				return (OperatingSystem);
			}
		}

		static PlatformID Platform_Id;
		static public PlatformID PlatformId
		{
			get
			{
				return (Platform_Id);
			}
		}

		static public bool IsWindows
		{
			get
			{
				return((Platform_Id == PlatformID.Win32NT)      ||
				       (Platform_Id == PlatformID.Win32S)       ||
				       (Platform_Id == PlatformID.Win32Windows) ||
				       (Platform_Id == PlatformID.WinCE));
			}
		}

		static public bool IsUnix
		{
			get
			{
				return((Platform_Id == PlatformID.Unix));
			}
		}

		static public bool IsOsx
		{
			get
			{
				return((Platform_Id == PlatformID.MacOSX));
			}
		}

		[STAThread]
		static void Main()
		{
			bool created;
			string process_name;

			Operating_System = Environment.OSVersion;
			Platform_Id      = Operating_System.Platform;

			Console.WriteLine(Platform_Id.ToString());

			process_name = System.IO.Directory.GetCurrentDirectory() + " - " + Application.ProductName;
			process_name = process_name.Replace("\\", "");
			process_name = process_name.Replace(".", "");
			process_name = process_name.Replace(":", "");

			Assembly assembly = Assembly.GetExecutingAssembly();
			FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
			Software_Version = fvi.FileVersion;
			Software_Name = fvi.ProductName;
			Exe_Path = System.IO.Path.GetDirectoryName(fvi.FileName) + "\\";

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

			Main_Form = new MainForm();
			Application.Run(Main_Form);
			Main_Form = null;
		}
	}
}

