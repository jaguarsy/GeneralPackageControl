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
    public partial class SetDefault : Form
    {
        private static string _path;

        public SetDefault(string path)
        {
            InitializeComponent();
            _path = path;
            tbPath.Text = path;
        }

        public static string ShowDialog(string path)
        {
            SetDefault set = new SetDefault(path);
            if (set.ShowDialog() == DialogResult.OK) return _path;
            return string.Empty;
        }

        private void btChoose_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                tbPath.Text = folderBrowserDialog.SelectedPath;
                _path = tbPath.Text;
            }
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
