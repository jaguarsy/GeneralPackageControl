using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralPackageControl.Models
{
    /// <summary>
    /// 第三方库、包、控件、插件
    /// </summary>
    public class PackageItem
    {
        public string PackageName { get; set; }
        public string Website { get; set; }
        public string DownloadUrl { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string LocalPath { get; set; }
    }
}
