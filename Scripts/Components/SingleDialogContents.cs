using System;

namespace UIs
{
    public class SingleDialogContents
    {
        public Action OnOk { get; set; }
        public string MainText { get; set; }
        public string HeaderText { get; set; }
        public string OkText { get; set; }
        
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

        public SingleDialogContents(string mainText, Action onOk = null)
        {
            MainText = mainText;
            OnOk = onOk;

            OkText = "OK";
            HeaderText = "";
            ShadowAlpha = 0;
            AnimationTime = 0.2f;
        }
    }
}