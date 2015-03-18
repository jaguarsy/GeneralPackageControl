﻿using GeneralPackageControl.Models;
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
    public partial class Confirm : Form
    {
        private List<PackageItem> _list;
        private static PackageItem _result = null;

        public Confirm(List<PackageItem> list)
        {
            InitializeComponent();
            this._list = list;
        }

        public static PackageItem ShowDialog(List<PackageItem> list)
        {
            Confirm confirm = new Confirm(list);
            confirm.ShowDialog();
            return _result;
        }

        private void Confirm_Load(object sender, EventArgs e)
        {
            foreach (var item in _list)
            {
                add(item);
            }
        }

        private void add(PackageItem item)
        {
            var radio = new RadioButton()
            {
                Text = item.DownloadUrl,
                AutoSize = true,
                Tag = item
            };
            radio.CheckedChanged += new EventHandler(radio_CheckedChanged);

            ItemPanel.Controls.Add(radio);
        }

        private void btConfirm_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void radio_CheckedChanged(object sender, EventArgs e)
        {
            _result = ((RadioButton)sender).Tag as PackageItem;
        }
    }
}