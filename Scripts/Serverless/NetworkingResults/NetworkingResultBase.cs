using System;
using System.Net;
using System.Text;
using Amazon.Lambda.Model;
using Constants;
using Databases.Transaction;
using Generals;
using Managers;
using UIs;
using UniRx;
using UnityEngine;

namespace Networks.NetworkingResults
{
    public abstract class NetworkingResultBase
    {
        public abstract string MethodName { get; }
        public ReactiveProperty<bool> IsCompleted { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> IsSucceeded { get; } = new ReactiveProperty<bool>();
        public abstract object GetLambdaEventSource();

        public abstract void OnFunctionError(ErrorResponse response);
        public abstract bool Deserialize(string json);

        public void OnFailed(InvokeResponse response)
        {
            NetworkManager.Instance.CloseLoadingWindow();
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                var decoded = Encoding.ASCII.GetString(response.Payload.ToArray());
                var header = JsonUtility.FromJson<ResponseHeader>(decoded);
                var error = JsonUtility.FromJson<ErrorResponse>(header.body);
                OnFunctionError(error);
            }
            else
            {
                OnServerError();
            }
        }

        public void OnServerError()
        {
            ForceRetry();
        }

        protected void ForceRetry(string body = ErrorBody.NetworkFailForceRetry)
        {
            var dialog =
                new SingleDialogContents(body, () => NetworkManager.Instance.Access(this))
                {
                    HeaderText = ErrorHeader.NetworkError
                };
            TopMostCanvas.Instance.ShowDialog(dialog);
        }

        protected void AskRetry(Action onCancel, string body = ErrorBody.NetworkFailAskRetry)
        {
            var dialog =
                new AlternativeDialogContents(body,
                    () => NetworkManager.Instance.Access(this))
                {
                    HeaderText = ErrorHeader.NetworkError,
                    OnCancel = onCancel
                };
            TopMostCanvas.Instance.ShowAlternativeDialog(dialog);
        }

        protected void BackToTitle(string body = ErrorBody.ErrorReturnTitle)
        {
            TopMostCanvas.Instance.ShowDialog(new SingleDialogContents(body, () =>
            {
                SlideScreenStacker.Instance.Clear("Title");
                SceneTransitionManager.Instance.ChangeScene("Title");
            }));
        }
    }
    
    public static class NetworkingResultExtensions
    {
        public static void AddOnCompleted(this NetworkingResultBase result, GameObject parent, Action onCompleted)
        {
            result.IsCompleted.Where(x => x).Take(1).TakeUntilDestroy(parent).Subscribe(_ => onCompleted());
        }
        
        public static void AddOnSucceeded(this NetworkingResultBase result, GameObject parent, Action onSucceeded)
        {
            result.IsSucceeded.Where(x => x).Take(1).TakeUntilDestroy(parent).Subscribe(_ => onSucceeded());
        }
    }
}