using System;
using System.Data;
using System.Net;
using System.Text;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Databases.Transaction;
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
                if (response.Exception == null && response.Response.StatusCode == (int)HttpStatusCode.OK)
                {
                    OnSucceeded(response.Response, result);
                }
                else if(response.Response != null)
                {
                    OnFailure(response.Response, result);
                }
                else
                {
                    UnityConsole.Log(response.Exception);
                    result.OnServerError();
                }
            });
        }

        private void OnSucceeded(InvokeResponse response, NetworkingResultBase result)
        {
            var decoded = Encoding.ASCII.GetString(response.Payload.ToArray());
            UnityConsole.Log("Lambda Succeeded.\n" + decoded);
            
            var innerError = JsonUtility.FromJson<InnerErrorResponse>(decoded);
            if (innerError.IsErrorState())
            {
                UnityConsole.Log($"Error occured in Function {result.MethodName}.");
                UnityConsole.Log(innerError);
                result.OnServerError();
                return;
            }
            if (result.Deserialize(decoded))
            {
                UnityConsole.Log($"Function {result.MethodName} Succeeded.");
                result.IsSucceeded = true;
            }
            else
            {
                result.OnFailed(response);
            }
        }
        
        private void OnFailure(InvokeResponse response, NetworkingResultBase result)
        {
            UnityConsole.Log("Lambda Failed.\n\n");
            UnityConsole.Log(response);
            result.OnFailed(response);
        }
    }
}