using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ShortcutSender.Properties;

namespace ShortcutSender
{
	public partial class AskFolderDlg : Form
	{
		public AskFolderDlg()
		{
			this.InitializeComponent();
			
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			this.FillList();
			this.SetLastFolder();
			this.SetSize();
		}

		private bool internalChanges = true;

		public string GetSelectedFolder()
		{
			if (this.lvFolders.SelectedItems.Count == 0)
			{
				throw new InvalidOperationException();
			}

			ListViewItem item = this.lvFolders.SelectedItems[0];
			string folder = (string)item.Tag;
			return folder;
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			this.columnHeader.Width = -2;

			if (!this.internalChanges)
			{
				Settings.Default.AskFolderDlgSize = this.Size;
				Settings.Default.Save();
			}
		}

		private void SetSize()
		{
			this.internalChanges = true;
			Size size = Settings.Default.AskFolderDlgSize;
			if (!size.IsEmpty)
			{
				this.Size = size;
			}
			this.internalChanges = false;
		}

		private void FillList()
		{
			try
			{
				this.lvFolders.BeginUpdate();
				this.lvFolders.Items.Clear();

				string root = Settings.Default.RootFolder;
				if (!string.IsNullOrWhiteSpace(root) && Directory.Exists(root))
				{
					foreach (string folder in Directory.GetDirectories(root))
					{
						ListViewItem item = this.lvFolders.Items.Add(Path.GetFileName(folder));
						item.ImageIndex = 0;
						item.Tag = folder;
					}

					if (this.lvFolders.Items.Count == 1)
					{
						this.lvFolders.Items[0].Selected = true;
					}
				}
			}
			finally
			{
				this.lvFolders.EndUpdate();
			}
		}

		private void SetLastFolder()
		{
			string lastFolder = Settings.Default.LastFolder;
			if (!string.IsNullOrEmpty(lastFolder))
			{
				foreach (ListViewItem item in this.lvFolders.Items)
				{
					string path = (string)item.Tag;
					if (lastFolder == path)
					{
						item.Checked = true;
						break;
					}
				}
			}
		}

		private void lvFolders_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			if (e.Item.Checked)
			{
				foreach (ListViewItem item in this.lvFolders.Items)
				{
					if (item != e.Item)
					{
						item.Checked = false;
					}
				}
			}

			this.btnOk.Enabled = this.lvFolders.CheckedItems.Count > 0;
		}

		private void btnSettings_Click(object sender, EventArgs e)
		{
			using (SettingsDlg dlg = new SettingsDlg(Settings.Default.RootFolder))
			{
				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					Settings.Default.RootFolder = dlg.GetRootFolder();
					Settings.Default.Save();

					this.FillList();
				}
			}
		}
	}
}
