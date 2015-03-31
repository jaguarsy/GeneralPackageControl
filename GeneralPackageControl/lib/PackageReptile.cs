using GeneralPackageControl.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GeneralPackageControl.lib
{
    public class PackageReptile
    {
        private string a_pattern = @"\<a[^\>]+href=""([^""]+)""[^\>]+\>([\s\S]+?)\<\/a\>";
        private string title_pattern = @"\<title\>([^\<]+)\<\/title\>";
        private string[] keyword = new string[] { "down", "下载", "download" };
        private string[] extends = new string[] { ".js", ".zip", ".rar", ".css" };

        private HttpHelper _httpHelper;
        private Regex a_Regex;
        private Regex title_Regex;

        public PackageReptile()
        {
            this._httpHelper = new HttpHelper();
            this.a_Regex = new Regex(a_pattern, RegexOptions.IgnoreCase);
            this.title_Regex = new Regex(title_pattern, RegexOptions.IgnoreCase);
        }

        private string getHtml(string url)
        {
            return _httpHelper.GetHtml(new HttpItem() { URL = url }).Html;
        }

        private string getAttribute(string html, string attr)
        {
            html = html.ToLower();
            var index = html.IndexOf(attr);
            if (index < 0) return null;

            var isStart = false;
            var temp = new StringBuilder();
            for (int i = index, len = html.Length; i < len; i++)
            {
                var ch = html[i];
                if (ch != '\"' && isStart)
                {
                    temp.Append(ch);
                }
                if (ch == '\"')
                {
                    if (!isStart) isStart = true;
                    else return temp.ToString();
                }
            }
            return temp.ToString();
        }

        private string getHtmlTitle(string html)
        {
            var match = title_Regex.Match(html);

            return Regex.Replace(match.Groups[1].Value, "\\s*|\t|\r|\n", "");
        }

        private string getTitle(string html)
        {
            var result = getAttribute(html, "title");
            if (!string.IsNullOrEmpty(result)) result = result.Trim();
            return result;
        }

        private bool isInKeyword(string str)
        {
            if (string.IsNullOrEmpty(str)) return false;
            foreach (var key in keyword)
            {
                if (str.IndexOf(key) > -1) return true;
            }
            return false;
        }

        private bool isUnusedHref(string href)
        {
            return href.Contains("javascript") ||
                   href.StartsWith("#");
        }

        private bool isInExtends(string href)
        {
            foreach (var item in extends)
            {
                if (href.Contains(item)) return true;
            }
            return false;
        }

        public List<PackageItem> Reptile(string url)
        {
            var html = getHtml(url);
            var packageList = new List<PackageItem>();

            var matches = a_Regex.Matches(html);
            foreach (Match item in matches)
            {
                var href = item.Groups[1].Value;
                if (isUnusedHref(href)) continue;

                var aTitle = getTitle(html);
                var text = item.Groups[2].Value;
                if (!isInKeyword(text) &&
                    !isInKeyword(aTitle) &&
                    !isInKeyword(href) &&
                    !isInExtends(href)) continue;
                var title = getHtmlTitle(html);

                var package = new PackageItem()
                {
                    PackageName = string.IsNullOrEmpty(title) ? text : title,
                    DownloadUrl = completeUrl(href, url),
                    LastUpdateTime = DateTime.Now,
                    Website = url
                };
                packageList.Add(package);
            }

            return packageList;
        }

        private string completeUrl(string href, string url)
        {
            if (href.StartsWith("./")) href = href.Substring(1, href.Length - 1);
            if (href.StartsWith("/") || !href.EndsWith("/") && !href.Contains("/")) return combine(url, href);
            else return href;
        }

        private string combine(string baseUrl, string ex)
        {
            if (ex.StartsWith("/")) ex = ex.Substring(1, ex.Length - 1);
            return baseUrl + ex;
        }

        public HttpResult DownloadPackage(PackageItem package)
        {
            return _httpHelper.GetHtml(new HttpItem()
            {
                URL = package.DownloadUrl,
                ResultType = ResultType.Byte
            });
        }
    }
}
