using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;
using System;
using Managers;
using UIs;

namespace Generals {
    public class TopMostCanvas : SingletonMonoBehaviour<TopMostCanvas> {

        [SerializeField] private Camera topMostCamera;
        [SerializeField] private Image screen;
        [SerializeField] private RectTransform windowContainer;
        [SerializeField] private SingleDialog singleDialog;
        [SerializeField] private AlternativeDialog alternativeDialog;
        [SerializeField] private RectTransform fitter;

        public Camera TopMostCamera => topMostCamera;

        public bool TopMostCanvasEnabled{
            set{
                screen.enabled = value;
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(this);
            AdjustOffset();
        }

        public void AdjustOffset()
        {
            if (!MainCanvasManager.IsExist) { return; }

            fitter.offsetMin = new Vector2(fitter.offsetMin.x, MainCanvasManager.Instance.AdjustedLower);
            fitter.offsetMax = new Vector2(fitter.offsetMax.x, -MainCanvasManager.Instance.AdjustedUpper);
        }

        public GameObject ShowWindow(GameObject windowObj){
            TopMostCanvasEnabled = true;
            var obj = windowContainer.gameObject.Create(windowObj);
            var rect = obj.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2 (windowContainer.sizeDelta.x, windowContainer.sizeDelta.y);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            
            return obj;
        }

        public GameObject ShowWindowWithFade(GameObject windowPrefab , float fadeTime , Action<GameObject> onShow = null , bool autoClose = false , float autoCloseTime = 2.0f , Action onCloseFinish = null){
            var window = windowPrefab;
            UnityConsole.Log("Window active : " +  window.gameObject.activeInHierarchy.ToString());
            if (!window.gameObject.activeInHierarchy)
            {
                window = ShowWindow(windowPrefab);
            }
            var canvasGroup = window.GetComponent<CanvasGroup> ();
            if (canvasGroup == null) {
                return window;
            }
            StartCoroutine(canvasGroup.FadeIn (fadeTime , () => {
                onShow.SafeAction(window);
                if(autoClose){
                    this.UpdateAsObservable()
                        .First()
                        .Delay(TimeSpan.FromMilliseconds(autoCloseTime * 1000f))
                        .TakeUntilDestroy(this)
                        .Subscribe(_ => CloseWindowWithFade(window , fadeTime , onCloseFinish));
                }
            } ));
            return window;
        }

        public void CloseWindowWithFade(GameObject windowInstance , float fadeTime , Action onCloseFinish = null){
            var canvasGroup = windowInstance.GetComponent<CanvasGroup> ();
            if (canvasGroup == null) {
                CloseWindow (windowInstance);
                onCloseFinish.SafeAction();
            } else {
                StartCoroutine(canvasGroup.FadeOut (fadeTime , () => {
                    CloseWindow (windowInstance);
                    onCloseFinish.SafeAction();
                }));
            }
        }

        public SingleDialog ShowDialog(SingleDialogContents contents)
        {
            if (singleDialog == null)
            {
                UnityConsole.Log("[singleDialog] is empty!!");
                return null;
            }
            var dialog = ShowWindowWithFade(singleDialog.gameObject, contents.AnimationTime).GetComponent<SingleDialog>();
            dialog.SetContents(contents);
            return dialog;
        }

        public AlternativeDialog ShowAlternativeDialog(AlternativeDialogContents contents)
        {
            if (singleDialog == null)
            {
                UnityConsole.Log("[singleDialog] is empty!!");
                return null;
            }
            var dialog = ShowWindowWithFade(alternativeDialog.gameObject, contents.AnimationTime).GetComponent<AlternativeDialog>();
            dialog.SetContents(contents);
            return dialog;
        }

        public void CloseWindow(GameObject windowInstance){
            TopMostCanvasEnabled = false;
            Destroy (windowInstance);
        }
    }
}