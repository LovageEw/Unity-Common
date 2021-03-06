﻿using System;
using Generals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIs {
    public class SingleDialog : MonoBehaviour
    {
        [SerializeField] private Image shadow;
        [SerializeField] private TextMeshProUGUI mainText;
        [SerializeField] private TextMeshProUGUI headerText;
        [SerializeField] private TextMeshProUGUI okText;
        
        private Action onOk;
        private float animationTime;

        public void SetContents(SingleDialogContents contents)
        {
            if (mainText != null)
            {
                mainText.text = contents.MainText;
            }
            if (headerText != null)
            {
                headerText.text = contents.HeaderText;
            }
            if (okText != null)
            {
                okText.text = contents.OkText;
            }
            if (shadow != null)
            {
                shadow.SetA(contents.ShadowAlpha);
            }
            onOk = contents.OnOk;
            animationTime = contents.AnimationTime;
        }

        public void OnOk()
        {
            TopMostCanvas.Instance.CloseWindowWithFade(gameObject , animationTime , () =>
            {
                onOk.SafeAction();
            });
        }
    }
}
