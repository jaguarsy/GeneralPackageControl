using GeneralPackageControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralPackageControl.Config
{
    public class ConfigModel
    {
        public string DefaultPath { get; set; }
        public List<PackageItem> packages { get; set; }
    }
}
