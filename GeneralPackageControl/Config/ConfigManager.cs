using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace GeneralPackageControl
{
    public class ConfigManager<T>
    {
        private readonly string PATH = "config";
        private static ConfigManager<T> instance;

        private ConfigManager()
        {
            if (!File.Exists(PATH))
            {
                File.Create(PATH).Close();
            }
        }

        public static ConfigManager<T> SingleInstance
        {
            get
            {
                if (instance == null)
                    instance = new ConfigManager<T>();
                return instance;
            }
        }

        public T GetConfig()
        {
            StreamReader sr = new StreamReader(PATH);
            JavaScriptSerializer js = new JavaScriptSerializer();

            var json = sr.ReadToEnd();
            var result = js.Deserialize<T>(json);
            sr.Close();
            return result;

        }

        public void SetConfig(T config)
        {
            StreamWriter sw = new StreamWriter(PATH);
            JavaScriptSerializer js = new JavaScriptSerializer();

            var result = js.Serialize(config);
            sw.WriteAsync(result);
            sw.Close();
        }
    }
}
