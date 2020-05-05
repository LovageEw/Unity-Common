using System;
using System.Net;
using Commons.Scripts.Utility;
using Constants;
using Databases.Transaction;
using Generals;
using Managers;
using UIs;
using UnityEngine;

namespace Networks.NetworkingResults
{
    public class VersionCheckNetworkingResult : NetworkingResultBase
    {
        public override string MethodName => "version";

        public override object GetLambdaEventSource()
        {
            return new VersionPayload();
        }

        [Serializable]
        private class VersionPayload
        {
            // ReSharper disable once NotAccessedField.Local
            [SerializeField] private string body;
        
            public VersionPayload()
            {
                var lambdaBody = new VersionBody{Version = VersionCheckValues.ServerVersion};
                body = JsonUtility.ToJson(lambdaBody);
            }
        }
        
        [Serializable]
        private class VersionBody
        {
            // ReSharper disable once NotAccessedField.Local
            public int Version;
        }
        
        public override void OnFunctionError(ErrorResponse response)
        {
            TopMostCanvas.Instance.ShowDialog(new SingleDialogContents(ErrorBody.OldVersion, () =>
            {
                HttpUtility.Open(VersionCheckValues.StoreUrl);
                SceneTransitionManager.Instance.ChangeScene("Title");
            }) {OkText = "<<Update>>"});
        }

        public override bool Deserialize(string json)
        {
            var response = JsonUtility.FromJson<ResponseHeader>(json);
            return response.statusCode == (int) HttpStatusCode.OK;
        }
    }
}