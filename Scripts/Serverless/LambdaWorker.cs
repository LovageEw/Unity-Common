#if !NO_NETWORKING
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Databases.Transaction;
using Networks.NetworkingResults;
using UnityEngine;
using Utf8Json;

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

        public void Invoke(NetworkingResultBase result, bool isStream = false)
        {
            var functionName = FunctionName(result.MethodName);
            InvokeRequest request;
            if (isStream)
            {
                var stream = JsonSerializer.Serialize(result.GetLambdaEventSource());
                request = new InvokeRequest {FunctionName = functionName, PayloadStream = new MemoryStream(stream)};
                UnityConsole.Log($"Request : {functionName}\nPayloadStream : [binary]");
            }
            else
            {
                var payload = JsonSerializer.ToJsonString(result.GetLambdaEventSource());
                request = new InvokeRequest {FunctionName = functionName, Payload = payload};
                UnityConsole.Log($"Request : {functionName}\nPayload : {payload}");
            }
            Invoke(request , result);
        }

        private void Invoke(InvokeRequest request, NetworkingResultBase result)
        {
            lambdaClient.InvokeAsync(request, response =>
            {
                result.IsCompleted.Value = true;
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
            
            var innerError = JsonSerializer.Deserialize<InnerErrorResponse>(response.Payload);
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
                result.IsSucceeded.Value = true;
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
#endif