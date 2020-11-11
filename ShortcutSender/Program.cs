using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using ShortcutSender.Properties;

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
			if (args.Length == 0)
			{
				return;
			}

			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			Application.ThreadException += OnApplicationOnThreadException;

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			try
			{
				using (AskFolderDlg dlg = new AskFolderDlg())
				{
					if (dlg.ShowDialog() == DialogResult.OK)
					{
						string folder = dlg.GetSelectedFolder();
						Settings.Default.LastFolder = folder;
						Settings.Default.Save();

						foreach (string path in args)
						{
							try
							{
								string shortcutName = Path.GetFileName(path);
								CreateShortcut(shortcutName, folder, path);
							}
							catch (Exception ex)
							{
								throw new Exception($"Error while process '{path}'.", ex);
							}
							
						}
					}
				}
			}
			catch (Exception ex)
			{
				ShowError(ex);
			}
		}

		private static void OnApplicationOnThreadException(object sender, ThreadExceptionEventArgs e)
		{
			if (e.Exception != null)
			{
				ShowError(e.Exception);
			}
		}

		private static void CreateShortcut(string shortcutName, string shortcutPath, string targetFileLocation)
		{
			string shortcutLocation = Path.Combine(shortcutPath, shortcutName + ".lnk");
			int index = 1;

			do
			{
				if (!System.IO.File.Exists(shortcutLocation))
				{
					break;
				}

				shortcutLocation = Path.Combine(shortcutPath, shortcutName + index + ".lnk");
				index++;
			}
			while (true);

			WshShell shell = new WshShell();
			IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);
			//shortcut.Description = "My shortcut description";		// The description of the shortcut
			//shortcut.IconLocation = @"c:\myicon.ico";				// The icon of the shortcut
			shortcut.TargetPath = targetFileLocation;				// The path of the file that will launch when the shortcut is run
			shortcut.Save();										// Save the shortcut
		}

		public static void ShowError(Exception ex)
		{
			MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}
}
