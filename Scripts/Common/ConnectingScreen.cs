using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Commons {
    public class ConnectingScreen : SingletonMonoBehaviour<ConnectingScreen>
    {
        [SerializeField] private Image loadingImage;
        
        public bool IsShow { get; private set; }
        private CanvasGroup canvasGroup;
        
        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            Observable.Interval(TimeSpan.FromMilliseconds(100))
                .TakeUntilDestroy(this)
                .Subscribe(_ => loadingImage.transform.Rotate(0,0,-30f)).AddTo(this);
        }
        
        public void Show()
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            IsShow = true;
            canvasGroup.DOFade(1f, 0.1f).SetEase(Ease.Linear);
        }
        
        public void Hide()
        {
            canvasGroup.DOFade(0f, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                IsShow = false;
            });
        }
    }
}
