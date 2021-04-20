using System.Linq;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Databases;
using UnityEngine;
using UnityEngine.Networking;

namespace Projects.Commons.Scripts.Networking 
{
    public abstract class JsonRequester
    {
        public bool IsReady { get; set; }
        private string uri;
        protected string json;

        public void Init(string uri)
        {
            this.uri = uri;
        }

        public virtual async UniTask<bool> Request()
        {
            using (var request = UnityWebRequest.Get(uri))
            {
                request.SetRequestHeader("Content-Type", "application/json");
                await request.SendWebRequest();
                if (request.isNetworkError || request.isHttpError)
                {
                    return false;
                }

                IsReady = true;
                json = request.downloadHandler.text;
            }
            
            return true;
        }

        public abstract IEnumerable<object[]> SelectAll();
    }
}