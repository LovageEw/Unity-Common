using UnityEngine;
using System;
using System.Collections;
using Constants;

static class CanvasGroupExtensions {

    public static IEnumerator FadeIn(this CanvasGroup canvas , float time , Action onFinish = null){
        int i = 0;
        float fps = DefaultSetting.TargetFps;
        while (i < time * fps) {
            canvas.alpha = (float)i / (time * fps);
            yield return new WaitForEndOfFrame();
            i++;
        }
        canvas.alpha = 1f;
        canvas.interactable = true;
        canvas.blocksRaycasts = true;
        if (onFinish != null) {
            onFinish ();
        }
    }

    public static IEnumerator FadeOut(this CanvasGroup canvas , float time , Action onFinish = null){
        int i= 0;
        float fps = DefaultSetting.TargetFps;
        while (i < time * fps) {
            canvas.alpha = (float)(time * fps - i) / (time * fps);
            yield return new WaitForEndOfFrame();
            i++;
        }
        canvas.alpha = 0f;
        canvas.interactable = false;
        canvas.blocksRaycasts = false;
        if (onFinish != null) {
            onFinish ();
        }
    }
}

