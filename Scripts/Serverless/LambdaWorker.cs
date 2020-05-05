using System.Text;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Networks.NetworkingResults;
using UnityEngine;

namespace Networks
{
    public class LambdaWorker
    {
        private IAmazonLambda lambdaClient;
        
        private static string FunctionName(string name) =>
            $"{NetworkSetting.ServiceName}-{NetworkSetting.Staging}-{name}";

        public void Init(CredentialHolder holder)
        {
            lambdaClient = new AmazonLambdaClient(holder.Credentials, NetworkSetting.Region);
        }

        public void Invoke(NetworkingResultBase result)
        {
            var eventJson = JsonUtility.ToJson(result.GetLambdaEventSource());
            UnityConsole.Log(eventJson);
            Invoke(FunctionName(result.MethodName), eventJson , result);
        }

        private void Invoke(string functionName, string payload, NetworkingResultBase result)
        {
            var request = new InvokeRequest {FunctionName = functionName, Payload = payload};
            UnityConsole.Log($"Request : {request.FunctionName}\nPayload : {payload}");
            lambdaClient.InvokeAsync(request, response =>
            {
                result.IsCompleted = true;
                if (response.Exception == null && response.Response.StatusCode == 200)
                {
                    OnSucceeded(response.Response, result);
                }
                else
                {
                    OnFailure(response.Response, result);
                }
            });
        }

        private void OnSucceeded(InvokeResponse response, NetworkingResultBase result)
        {
            var decoded = Encoding.ASCII.GetString(response.Payload.ToArray());
            UnityConsole.Log("Lambda Succeeded.\n\n" + decoded);
            if (result.Deserialize(decoded))
            {
                result.IsSucceeded = true;
            }
            else
            {
                result.OnFailed(response);
            }
        }
        
        private void OnFailure(InvokeResponse response, NetworkingResultBase result)
        {
            UnityConsole.Log(response.ToString());
            result.OnFailed(response);
        }
    }
}