using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ShortcutSender.Properties;

namespace ShortcutSender
{
	public partial class SettingsDlg : Form
	{
		public SettingsDlg()
		{
			this.InitializeComponent();
			this.Icon = Icon.FromHandle(Resources.gear.GetHicon());
		}

		public SettingsDlg(string rootFolder) : this()
		{
			this.tbRootFolder.Text = rootFolder;
		}

		public string GetRootFolder()
		{
			return this.tbRootFolder.Text;
		}

		private void btnSelectFolder_Click(object sender, System.EventArgs e)
		{
			using (FolderBrowserDialog dlg = new FolderBrowserDialog())
			{
				if (this.tbRootFolder.TextLength != 0 && Directory.Exists(this.tbRootFolder.Text))
				{
					dlg.SelectedPath = this.tbRootFolder.Text;
				}

				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					this.tbRootFolder.Text = dlg.SelectedPath;
				}
			}
		}

		private void tbRootFolder_TextChanged(object sender, System.EventArgs e)
		{
			this.btnOk.Enabled = this.tbRootFolder.TextLength > 0;
		}
	}
}
