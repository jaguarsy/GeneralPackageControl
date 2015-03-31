using GeneralPackageControl.Interface;
using GeneralPackageControl.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralPackageControl
{
    public partial class MainForm : IUpdate
    {
        void IUpdate.Update(int done, int sum)
        {
            Invoke(new Action(() =>
            {
                btUpdate.Enabled = done == sum;
                StatusLabel.Text = done == sum ? Resources.Done : Resources.Updating;
                ProgressBar.Value = (int)(((float)done / (float)sum) * 100);
            }));
        }
    }
}
