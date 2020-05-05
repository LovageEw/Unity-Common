using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Commons;
using DG.Tweening;
using Managers;
using Scenes.Common.Scripts;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menus
{
    [RequireComponent(typeof(CanvasGroup))]
    public class LoadingScreen : SingletonMonoBehaviour<LoadingScreen>
    {
        [SerializeField] private Image loadingImage;

        public bool IsShow { get; private set; }
        
        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            Observable.Interval(TimeSpan.FromMilliseconds(100))
                .TakeUntilDestroy(this)
                .Subscribe(_ => loadingImage.transform.Rotate(0,0,-30f)).AddTo(this);
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void Show()
        {
            var canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            IsShow = true;
            canvasGroup.DOFade(1f, 0.5f).SetEase(Ease.Linear);
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            var components = MainCanvasManager.Instance.GetComponentsInChildren<INeedPrepare>();
            UnityConsole.Log($"PrepareTargets : {components.Length}");
            if (components.Length == 0)
            {
                ShowPage();
                return;
            }
            this.UpdateAsObservable().Select(_ => components.All(x => x.IsPrepared))
                .DistinctUntilChanged()
                .Where(x => x)
                .Take(1)
                .Subscribe(_ => ShowPage());
        }

        private void ShowPage()
        {
            var canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.DOFade(0f, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                IsShow = false;
            });
        }
    }
}