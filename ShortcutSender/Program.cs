using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShortcutSender
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			using (AskFolderDlg dlg = new AskFolderDlg())
			{
				if (dlg.ShowDialog() == DialogResult.OK)
				{
				}
			}
		}
	}
}
