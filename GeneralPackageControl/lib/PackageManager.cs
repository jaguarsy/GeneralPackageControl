using GeneralPackageControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace GeneralPackageControl.lib
{
    public class PackageManager
    {
        private readonly string PATH = "packages";

        private Firebase _firebase;
        private PackageReptile _reptile;
        private JavaScriptSerializer _js;

        public PackageManager(Firebase firebase)
        {
            this._firebase = firebase;
            _reptile = new PackageReptile();
            _js = new JavaScriptSerializer();
        }

        public PackageItem ReptilePackage(string url)
        {
            var list = _reptile.Reptile(url);

            var result = Confirm.ShowDialog(list);
            return result;
        }

        public void AnalyzePackage(string url)
        {
            _firebase.push(PATH, ReptilePackage(url), (res) =>
            {
                Console.WriteLine(res.Body);
            });
        }

        public void GetPackages(Action<PackageItem> callback)
        {
            _firebase.get(PATH, (res) =>
            {
            });
        }
    }
}
