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

        public MainForm()
        {
            InitializeComponent();

            _firebase = new Firebase(
                Properties.Resources.Auth,
                Properties.Resources.BasePath
            );

            _packageManager = new PackageManager(_firebase);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void tbUrl_TextChanged(object sender, EventArgs e)
        {
            btAdd.Enabled = !string.IsNullOrWhiteSpace(tbUrl.Text);
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            _packageManager.GetPackage(tbUrl.Text.Trim());
        }
    }
}
