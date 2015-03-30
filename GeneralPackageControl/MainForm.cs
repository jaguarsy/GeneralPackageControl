using GeneralPackageControl.Config;
using GeneralPackageControl.lib;
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
                Properties.Resources.Auth,
                Properties.Resources.BasePath
            );

            _packageManager = new PackageManager(_firebase);
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
                ListBox.Items.Add(item.PackageName, false);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            init();
        }

        private void tbUrl_TextChanged(object sender, EventArgs e)
        {
            btAdd.Enabled = !string.IsNullOrWhiteSpace(tbUrl.Text);
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            var result = _packageManager.ReptilePackage(tbUrl.Text.Trim());

            _config.packages.Add(result);
            _configManager.SetConfig(_config);
            init();
            //_packageManager.AnalyzePackage(tbUrl.Text.Trim());
        }
    }
}
