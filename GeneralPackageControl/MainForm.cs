using GeneralPackageControl.Config;
using GeneralPackageControl.lib;
using GeneralPackageControl.Models;
using GeneralPackageControl.Properties;
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
    public partial class MainForm : Form
    {
        private Firebase _firebase;
        private PackageManager _packageManager;
        private ConfigManager<ConfigModel> _configManager;
        private ConfigModel _config;

        public MainForm()
        {
            InitializeComponent();

            _firebase = new Firebase(
                Resources.Auth,
                Resources.BasePath
            );

            _packageManager = new PackageManager(_firebase);
            _packageManager.Register(this);

            _configManager = ConfigManager<ConfigModel>.SingleInstance;
            _config = _configManager.GetConfig();

            if (_config != null) return;
            _config = new ConfigModel();
            _config.packages = new List<PackageItem>();
        }

        private void init()
        {
            if (_config == null) return;
            ListBox.Items.Clear();
            foreach (var item in _config.packages)
            {
                if (item == null) continue;
                ListBox.Items.Add(item.PackageName, false);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            init();

            StatusLabel.Text = Resources.Synchronizing;
            _packageManager.Pull((p) =>
            {
                _config.packages = p;
                _configManager.SetConfig(_config);

                init();
                StatusLabel.Text = Resources.Synchronized;
            });
        }

        private void tbUrl_TextChanged(object sender, EventArgs e)
        {
            btAdd.Enabled = !string.IsNullOrWhiteSpace(tbUrl.Text);
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            var result = _packageManager.ReptilePackage(tbUrl.Text.Trim());
            if (result == null) return;

            _config.packages.Add(result);
            _configManager.SetConfig(_config);
            _packageManager.Push(_config.packages);
            init();
        }

        private void btChooseAll_Click(object sender, EventArgs e)
        {
            setSelected();
        }

        private void setSelected()
        {
            var count = ListBox.Items.Count;
            var selected = ListBox.CheckedItems.Count < count;
            for (var i = 0; i < count; i++)
            {
                ListBox.SetItemChecked(i, selected);
            }
        }

        private void btUpdate_Click(object sender, EventArgs e)
        {
            var list = ListBox.CheckedItems;
            var checkedList = _config.packages.Where(p => list.Contains(p.PackageName)).ToList();

            btUpdate.Enabled = false;
            StatusLabel.Text = Resources.Updating;
            _packageManager.UpdatePackage(checkedList);
        }

        private void ToolStripMenuItemEdit_Click(object sender, EventArgs e)
        {
            var target = ListBox.SelectedItem;
            var package = _config.packages.SingleOrDefault(p => p.PackageName.Equals(target));

            if (EditConfig.ShowDialog(package) == null) return;

            _configManager.SetConfig(_config);
            _packageManager.Push(_config.packages);

            init();
        }

        private void ToolStripMenuItemUpdate_Click(object sender, EventArgs e)
        {
            var item = ListBox.SelectedItem;
            var target = _config.packages.SingleOrDefault(p => p.PackageName.Equals(item));

            btUpdate.Enabled = false;
            StatusLabel.Text = Resources.Updating;
            _packageManager.UpdatePackage(target);
        }
    }
}
