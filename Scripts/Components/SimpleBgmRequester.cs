using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace UIs {
    public class SimpleBgmRequester : MonoBehaviour
    {
        [SerializeField] private AudioClip bgm;
        [SerializeField][Range(0f,10f)] private float fadeTime;
        [SerializeField] private bool isLoop = true;

        private void Start()
        {
            if (bgm != null)
            {
                SoundManager.Instance.PlayBgm(bgm, fadeTime, true, isLoop);
            }
            else
            {
                SoundManager.Instance.StopBgm(fadeTime);
            }
        }
    }
}
