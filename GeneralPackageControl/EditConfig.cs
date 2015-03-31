using GeneralPackageControl.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneralPackageControl
{
    public partial class EditConfig : Form
    {
        private static PackageItem _target;
        private static ConfigManager<PackageItem> _configManager;

        private EditConfig()
        {
            InitializeComponent();
        }

        public static PackageItem ShowDialog(PackageItem item)
        {
            _target = item;
            var edit = new EditConfig();
            if (edit.ShowDialog() == DialogResult.OK) return _target;
            return null;
        }

        private void EditConfig_Load(object sender, EventArgs e)
        {
            this.tbPackageName.Text = _target.PackageName;
            this.tbUrl.Text = _target.DownloadUrl;
            this.tbPath.Text = _target.LocalPath;
            this.tbWebsite.Text = _target.Website;
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            _target.PackageName = this.tbPackageName.Text;
            _target.DownloadUrl = this.tbUrl.Text;
            _target.LocalPath = this.tbPath.Text;
            _target.Website = this.tbWebsite.Text;

            this.DialogResult = DialogResult.OK;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
