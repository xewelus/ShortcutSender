using System;
using System.Threading;
using System.Windows.Forms;
using IWshRuntimeLibrary;

namespace ShortcutSender
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(params string[] args)
		{
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			Application.ThreadException += OnApplicationOnThreadException;

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			using (AskFolderDlg dlg = new AskFolderDlg())
			{
				if (dlg.ShowDialog() == DialogResult.OK)
				{
				}
			}
		}

		private static void OnApplicationOnThreadException(object sender, ThreadExceptionEventArgs e)
		{
			if (e.Exception != null)
			{
				ShowError(e.Exception);
			}
		}

		public static void CreateShortcut(string shortcutName, string shortcutPath, string targetFileLocation)
		{
			string shortcutLocation = System.IO.Path.Combine(shortcutPath, shortcutName + ".lnk");
			WshShell shell = new WshShell();
			IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

			shortcut.Description = "My shortcut description";   // The description of the shortcut
			shortcut.IconLocation = @"c:\myicon.ico";           // The icon of the shortcut
			shortcut.TargetPath = targetFileLocation;                 // The path of the file that will launch when the shortcut is run
			shortcut.Save();                                    // Save the shortcut
		}

		public static void ShowError(Exception ex)
		{
			MessageBox.Show(ex.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}
}
