using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using DG.Tweening;

namespace Controllers
{
    public class FadingController : SingletonMonoBehaviour<FadingController>
    {
        [SerializeField] Image screen = null;

        public void FadeIn(float time , Action onFinish = null){
            screen.enabled = true;
            screen.DOFade(0.0f, time).OnComplete(() => {
                screen.enabled = false;
                if(onFinish != null){
                    onFinish();
                }
            }).OnStart(() => screen.SetA(0.0f));
        }

        public void FadeOut(float time, Action onFinish = null) {
            screen.enabled = true;
            screen.DOFade(1.0f, time).OnComplete(() => {
                screen.enabled = false;
                if (onFinish != null) {
                    onFinish();
                }
            });
        }


    }
}