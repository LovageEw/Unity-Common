using System;
using Generals;
using UnityEngine;
using UnityEngine.UI;

namespace UIs {
    public class AlternativeDialog : MonoBehaviour
    {
        [SerializeField] private Image shadow;
        [SerializeField] private Text mainText;
        [SerializeField] private Text headerText;
        [SerializeField] private Text cancelText;
        [SerializeField] private Text okText;
        [SerializeField] [Range(0, 255)] private int shadhowAlpha;
        
        private Action onOk;
        private Action onCancel;
        private float animationTime;

        public void SetContents(AlternativeDialogContents contents)
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
            if (cancelText != null)
            {
                cancelText.text = contents.CancelText;
            }
            if (shadow != null)
            {
                shadow.SetA(contents.ShadowAlpha);
            }
            onOk = contents.OnOk;
            onCancel = contents.OnCancel;
            animationTime = contents.AnimationTime;
        }

        public void OnOk()
        {
            TopMostCanvas.Instance.CloseWindowWithFade(gameObject , animationTime , () =>
            {
                onOk.SafeAction();
            });
        }

        public void OnCancel()
        {
            TopMostCanvas.Instance.CloseWindowWithFade(gameObject , animationTime , () =>
            {
                onCancel.SafeAction();
            });
        }
    }
}
