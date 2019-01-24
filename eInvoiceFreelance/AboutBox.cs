using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eInvoiceFreelance
{
	partial class AboutBox : Form
	{
		private bool Ok;
		private const string Disclaimer = @"IL SOFTWARE È FORNITO DAI DETENTORI DEL COPYRIGHT E DAI COLLABORATORI 'COSÌ COM'È' E NON SI RICONOSCE ALCUNA ALTRA GARANZIA ESPRESSA O IMPLICITA, INCLUSE, A TITOLO ESEMPLIFICATIVO, GARANZIE IMPLICITE DI COMMERCIABILITÀ E IDONEITÀ PER UN FINE PARTICOLARE. IN NESSUN CASO IL PROPRIETARIO DEL COPYRIGHT O I RELATIVI COLLABORATORI POTRANNO ESSERE RITENUTI RESPONSABILI PER DANNI DIRETTI, INDIRETTI, INCIDENTALI, SPECIALI, PUNITIVI, O CONSEQUENZIALI (INCLUSI, A TITOLO ESEMPLIFICATIVO, DANNI DERIVANTI DALLA NECESSITÀ DI SOSTITUIRE BENI E SERVIZI, DANNI PER MANCATO UTILIZZO, PERDITA DI DATI O MANCATO GUADAGNO, INTERRUZIONE DELL'ATTIVITÀ), IMPUTABILI A QUALUNQUE CAUSA E INDIPENDENTEMENTE DALLA TEORIA DELLA RESPONSABILITÀ, SIA NELLE CONDIZIONI PREVISTE DAL CONTRATTO CHE IN CASO DI 'STRICT LIABILITY', ERRORI (INCLUSI NEGLIGENZA O ALTRO), DERIVANTI O COMUNQUE CORRELATI ALL'UTILIZZO DEL SOFTWARE, ANCHE QUALORA SIANO STATI INFORMATI DELLA POSSIBILITÀ DEL VERIFICARSI DI TALI DANNI.";

		public bool Result { get { return (Ok); } }

		public AboutBox(bool ok)
		{
			InitializeComponent();
			this.Text = String.Format("About {0}", AssemblyTitle);
			this.ProductNameLabel.Text = AssemblyProduct;
			this.VersionLabel.Text = String.Format("Version {0}", AssemblyVersion);
			this.CopyrightLabel.Text = AssemblyCopyright;
			this.CompanyNameLabel.Text = AssemblyCompany;
			this.BoxDescriptionText.Text = Disclaimer;

			Ok = ok;

			OkButton.Enabled = ok;

			if (ok)
			{
				OkCheckBox.Visible = false;
			}
			else
			{
				OkCheckBox.Enabled = false;
				OkCheckBox.Checked = false;
			}
		}

		#region Assembly Attribute Accessors

		public string AssemblyTitle
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				if (attributes.Length > 0)
				{
					AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
					if (titleAttribute.Title != "")
					{
						return titleAttribute.Title;
					}
				}
				return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}

		public string AssemblyVersion
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}
		}

		public string AssemblyDescription
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyDescriptionAttribute)attributes[0]).Description;
			}
		}

		public string AssemblyProduct
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyProductAttribute)attributes[0]).Product;
			}
		}

		public string AssemblyCopyright
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
			}
		}

		public string AssemblyCompany
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCompanyAttribute)attributes[0]).Company;
			}
		}
		#endregion

		private void OkButton_Click(object sender, EventArgs e)
		{
			Ok = true;
			Close();
		}

		private void OkCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			OkButton.Enabled = OkCheckBox.Checked;
		}

		private void DonatePictureBox_Click(object sender, EventArgs e)
		{
			Donate();
		}

		private void Donate()
		{
			string url;

			string business = "PBUGWQJTH5MGC";
			string description = "Donation";
			string country = "IT";
			string currency = "EUR";

			url = "https://www.paypal.com/cgi-bin/webscr" +
			      "?cmd=" + "_donations" +
			      "&business=" + business +
			      "&lc=" + country +
			      "&item_name=" + description +
			      "&currency_code=" + currency +
			      "&bn=" + "PP%2dDonationsBF";

			System.Diagnostics.Process.Start(url);
		}

		private void AboutBox_FormClosed(object sender, FormClosedEventArgs e)
		{
		}

		private void BoxDescriptionText_VScroll(object sender, EventArgs e)
		{
			OkCheckBox.Enabled = true;
		}
	}
}
