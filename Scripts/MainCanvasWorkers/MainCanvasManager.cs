using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

namespace Managers
{
    public class MainCanvasManager : SingletonMonoBehaviour<MainCanvasManager>
    {
        [SerializeField] CanvasScaler scaler;
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform sizeFitRect;
        [SerializeField] private RectTransform upperRect;
        [SerializeField] private float upperMinHeight;
        [SerializeField] private RectTransform lowerRect;
        [SerializeField] private float lowerMinHeight;
        [SerializeField] private Vector2 desiredAspectRatio;
        
        public float ScreenX => GetComponent<RectTransform>().sizeDelta.x;
        public float ScreenY => GetComponent<RectTransform>().sizeDelta.y;
        public Vector2 ScreenSize => GetComponent<RectTransform>().sizeDelta;
        public float ScaleX => Screen.width / ScreenX;
        public float ScaleY => Screen.height / ScreenY;
        public Camera RenderCamera => canvas.worldCamera;
        public float AdjustedUpper { get; private set; }
        public float AdjustedLower { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            if (ScreenX * desiredAspectRatio.y >= ScreenY * desiredAspectRatio.x)
            {
                AdjustScreenSize(upperMinHeight, lowerMinHeight);
            }
            else
            {
                var adjustTotalHeight = ScreenY - ScreenX * desiredAspectRatio.y / desiredAspectRatio.x;
                var upper = Math.Max(adjustTotalHeight / 2.0f, upperMinHeight);
                var lower = Math.Max(adjustTotalHeight - upper, lowerMinHeight);
                AdjustScreenSize(upper,lower);
            }
        }

        private void AdjustScreenSize(float offsetUpper, float offsetLower)
        {
            if (upperRect == null || lowerRect == null) { return; }

            AdjustedUpper = offsetUpper;
            AdjustedLower = offsetLower;
            upperRect.anchoredPosition = new Vector2(upperRect.anchoredPosition.x, -offsetUpper);
            lowerRect.anchoredPosition = new Vector2(lowerRect.anchoredPosition.x, offsetLower);
            upperRect.offsetMin = new Vector2(upperRect.offsetMin.x, -offsetUpper);
            lowerRect.offsetMax = new Vector2(lowerRect.offsetMax.x, offsetLower);
            sizeFitRect.offsetMin = new Vector2(sizeFitRect.offsetMin.x, offsetLower);
            sizeFitRect.offsetMax = new Vector2(sizeFitRect.offsetMax.x, -offsetUpper);
        }

        public float GetRatioToDesired()
        {
            if (scaler.matchWidthOrHeight == 1)
            {
                return Math.Min(1.0f, ScreenX / scaler.referenceResolution.x) ;
            }
            if(scaler.matchWidthOrHeight == 0)
            {
                return Math.Min(1.0f, ScreenY / scaler.referenceResolution.y) ;
            }

            // Not Defined
            return 1;
        }
    }
}