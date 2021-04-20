using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Projects.Commons.Scripts.Networking;

namespace Databases
{
    public class WebGLJsonDatabase : IClientDatabase
    {
        private readonly Dictionary<string,JsonRequester> jsonRequesters = new Dictionary<string, JsonRequester>();

        public void RegisterTable<T>(string tableName, string uri) where T : JsonRequester, new()
        {
            if (jsonRequesters.ContainsKey(tableName))
            {
                return;
            }
            var requester = new T();
            requester.Init(uri);
            jsonRequesters.Add(tableName, requester);
        }

        public async UniTask<bool> Fetch()
        {
            var requests = await UniTask.WhenAll(jsonRequesters.Values.Where(x => !x.IsReady).Select(x => x.Request()));
            return requests.All(x => x);
        }

        public List<object[]> SelectAll(DAOBase dao)
        {
            return jsonRequesters.ContainsKey(dao.TableName) ? jsonRequesters[dao.TableName].SelectAll().ToList() : null;
        }
    }
}