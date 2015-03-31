using GeneralPackageControl.Config;
using GeneralPackageControl.Interface;
using GeneralPackageControl.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace GeneralPackageControl.lib
{
    public class PackageManager
    {
        private readonly string PATH = "packages";
        private string[] extends = new string[] { ".js", ".zip", ".rar", ".css" };

        private Firebase _firebase;
        private PackageReptile _reptile;
        private JavaScriptSerializer _js;
        private List<IUpdate> iUpdates;

        public PackageManager(Firebase firebase)
        {
            this._firebase = firebase;
            _reptile = new PackageReptile();
            _js = new JavaScriptSerializer();
            iUpdates = new List<IUpdate>(); ;
        }

        public void Register(IUpdate watcher)
        {
            iUpdates.Add(watcher);
        }

        public void UnRegister(IUpdate watcher)
        {
            iUpdates.Remove(watcher);
        }

        private void Notify(int done, int sum)
        {
            foreach (var watcher in iUpdates)
            {
                watcher.Update(done, sum);
            }
        }

        public PackageItem ReptilePackage(string url, string path)
        {
            var list = _reptile.Reptile(url);

            var result = Confirm.ShowDialog(list, path);
            return result;
        }

        public void Pull(Action<List<PackageItem>> callback)
        {
            _firebase.Get(PATH, (res2) =>
            {
                var result = res2.Body;
                var index = result.IndexOf(':');
                result = result.Substring(index + 1, result.Length - index - 2);

                JavaScriptSerializer js = new JavaScriptSerializer();
                var packages = js.Deserialize<List<PackageItem>>(result);
                callback(packages);
            });
        }

        public void Push(List<PackageItem> packages)
        {
            _firebase.Delete(PATH, (res) =>
            {
                Console.WriteLine(res.Body);

                _firebase.Push(PATH, packages, (res2) =>
                {
                    Console.WriteLine(res2.Body);
                });
            });
        }

        public void GetPackages(Action<PackageItem> callback)
        {
            _firebase.Get(PATH, (res) =>
            {
            });
        }

        public void UpdatePackage(PackageItem package)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(download));
            thread.Start(package);
        }

        public void UpdatePackage(List<PackageItem> packages)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(download));
            thread.Start(packages);
        }

        private void download(object packages)
        {
            var list = new List<PackageItem>();
            if (packages is PackageItem)
            {
                list.Add(packages as PackageItem);
            }
            else if (packages is List<PackageItem>)
            {
                list.AddRange(packages as List<PackageItem>);
            }
            else return;

            int count = 0, sum = list.Count;

            foreach (var item in list)
            {
                Console.WriteLine(item.PackageName);
                var result = _reptile.DownloadPackage(item);
                Console.WriteLine(result.Header);
                var packagePath = Path.Combine(item.LocalPath, item.PackageName);
                if (!Directory.Exists(packagePath))
                {
                    Directory.CreateDirectory(packagePath);
                }

                var filename = getName(result.Header, item.DownloadUrl).ToLower();
                File.WriteAllBytes(Path.Combine(packagePath, filename), result.ResultByte);

                DeCompressFile(packagePath, filename);

                count++;
                Notify(count, sum);
            }
        }

        private string getName(WebHeaderCollection Headers, string url)
        {
            var fileName = getNameByLocation(Headers);
            if (!string.IsNullOrEmpty(fileName)) return fileName;

            fileName = getNameByDisposition(Headers);
            if (!string.IsNullOrEmpty(fileName)) return fileName;

            var tmp = url.Split('/');
            fileName = tmp[tmp.Length - 1];
            if (!string.IsNullOrEmpty(fileName)) return fileName;

            return string.Empty;
        }

        private string getNameByLocation(WebHeaderCollection Headers)
        {
            var url = Headers["Content-Location"] as string;
            if (string.IsNullOrEmpty(url)) return string.Empty;

            var list = url.Split('/');
            var result = list[list.Length - 1];
            if (result.Contains('.')) return result;
            return string.Empty;
        }

        private string getNameByDisposition(WebHeaderCollection Headers)
        {
            var disp = Headers["Content-Disposition"] as string;
            if (string.IsNullOrEmpty(disp)) return string.Empty;
            return disp.Split('=')[1];
        }

        private void DeCompressFile(string sourcePath, string filename)
        {
            if (filename.Contains("zip"))
            {
                var sourceFile = Path.Combine(sourcePath, filename);
                Zip.unZipFile(sourceFile, sourcePath);
                File.Delete(sourceFile);
            }
        }
    }
}
