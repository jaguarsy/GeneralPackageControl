using GeneralPackageControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeneralPackageControl.lib
{
    public class PackageManager
    {
        private readonly string PATH = "packages";

        private Firebase _firebase;
        private PackageReptile _reptile;

        public PackageManager(Firebase firebase)
        {
            this._firebase = firebase;
            _reptile = new PackageReptile();
        }

        public void GetPackage(string url)
        {
            var list = _reptile.Reptile(url);

            var result = Confirm.ShowDialog(list);
            if (result == null) return;

            _firebase.push(PATH, result, (res) =>
            {
                Console.WriteLine(res.Body);
            });
        }
    }
}
