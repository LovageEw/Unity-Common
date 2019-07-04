using System;
using UnityEngine;

namespace Commons.Scripts.Extensions
{
    public static class RectTransformExtensions
    {
        public static Vector2 GetSize(this RectTransform rect)
        {
            if (rect.sizeDelta.x > 0 && rect.sizeDelta.y > 0) { return rect.sizeDelta; }

            var scaleX = rect.sizeDelta.x <= 0 ? rect.anchorMax.x - rect.anchorMin.x : 1;
            var scaleY = rect.sizeDelta.y <= 0 ? rect.anchorMax.y - rect.anchorMin.y : 1;

            var screenSize = GetSize(rect.parent.GetComponent<RectTransform>());
            var offsetX = rect.offsetMin.x - rect.offsetMax.x;
            var offsetY = rect.offsetMin.y - rect.offsetMax.y;

            return new Vector2(screenSize.x * scaleX - offsetX, screenSize.y * scaleY - offsetY);
        }
    }
}