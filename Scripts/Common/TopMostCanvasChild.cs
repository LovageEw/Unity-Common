using Generals;
using UnityEngine;

namespace UIs
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TopMostCanvasChild : MonoBehaviour
    {
        [SerializeField, Range(0f, 5f)] protected float fadingDuration = 0.5f;

        public virtual void Close()
        {
            TopMostCanvas.Instance.CloseWindowWithFade(gameObject, fadingDuration);
        }
    }
}