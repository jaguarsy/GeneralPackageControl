using FireSharp;
using FireSharp.Config;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralPackageControl.lib
{
    public class Firebase
    {
        private string _auth;
        private string _basePath;
        private FirebaseClient _client;
        private FirebaseConfig _config;

        private Firebase() { }

        /// <summary>
        /// Constructed Function
        /// </summary>
        /// <param name="auth">Firebase Token</param>
        /// <param name="basePath">Firebase Path</param>
        public Firebase(string auth, string basePath)
        {
            this._auth = auth;
            this._basePath = basePath;
            this._config = new FirebaseConfig()
            {
                AuthSecret = auth,
                BasePath = basePath
            };
            this._client = new FirebaseClient(this._config);
        }

        /// <summary>
        /// Firebase Get
        /// </summary>
        /// <param name="path">Relative Path</param>
        /// <param name="callback">Callback Function</param>
        public async void get(string path, Action<FirebaseResponse> callback)
        {
            var res = await _client.GetAsync(path);
            callback(res);
        }

        /// <summary>
        /// Firebase Push
        /// </summary>
        /// <param name="path">Relative Path</param>
        /// <param name="data">Push Data</param>
        /// <param name="callback">Callback Function</param>
        public async void push(string path, object data, Action<FirebaseResponse> callback)
        {
            var res = await _client.PushAsync(path, data);
            callback(res);
        }

        /// <summary>
        /// Firebase Set
        /// </summary>
        /// <param name="path">Relative Path</param>
        /// <param name="data">Set Data</param>
        /// <param name="callback">Callback Function</param>
        public async void set(string path, object data, Action<FirebaseResponse> callback)
        {
            var res = await _client.SetAsync(path, data);
            callback(res);
        }

        /// <summary>
        /// Firebase Update
        /// </summary>
        /// <param name="path">Relative Path</param>
        /// <param name="data">Update Data</param>
        /// <param name="callback">Callback Function</param>
        public async void update(string path, object data, Action<FirebaseResponse> callback)
        {
            var res = await _client.UpdateAsync(path, data);
            callback(res);
        }

        /// <summary>
        /// Firebase Delete
        /// </summary>
        /// <param name="path">Relative Path</param>
        /// <param name="callback">Callback Function</param>
        public async void delete(string path, Action<FirebaseResponse> callback)
        {
            var res = await _client.DeleteAsync(path);
            callback(res);
        }
    }
}
