using System;

namespace UIs
{
    public class AlternativeDialogContents
    {
        public Action OnOk { get; set; }
        public Action OnCancel { get; set; }
        public string MainText { get; set; }
        public string HeaderText { get; set; }
        public string OkText { get; set; }
        public string CancelText { get; set; }

        private int shadowAlpha;
        public int ShadowAlpha
        {
            get { return shadowAlpha; }
            set
            {
                if (value > 255)
                {
                    shadowAlpha = 255;
                }
                else if (shadowAlpha < 0)
                {
                    shadowAlpha = 0;
                }
                else
                {
                    shadowAlpha = value;
                }
            }
        }

        private float animationTime;
        public float AnimationTime
        {
            get { return animationTime; }
            set { animationTime = value < 0.1f ? 0.1f : value; }
        }

        public AlternativeDialogContents(string mainText, Action onOk)
        {
            MainText = mainText;
            OnOk = onOk;

            OkText = "OK";
            CancelText = "Cancel";
            HeaderText = "";
            ShadowAlpha = 0;
            AnimationTime = 0.2f;
        }
    }
}