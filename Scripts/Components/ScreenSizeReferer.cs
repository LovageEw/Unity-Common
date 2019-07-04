using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UIs
{
    [RequireComponent(typeof(RectTransform))]
    public class ScreenSizeReferer : MonoBehaviour
    {
        private void Start()
        {
            var ratio = MainCanvasManager.Instance.GetRatioToDesired();
            AdjustRect(ratio);
            AdjustGrid(ratio);
            AdjustText(ratio);
        }

        private void AdjustRect(float ratio)
        {
            var rect = GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x * ratio, rect.sizeDelta.y * ratio);
        }
        
        private void AdjustGrid(float ratio)
        {
            var grid = GetComponent<GridLayoutGroup>();
            if(grid == null) {return;}
            grid.cellSize = new Vector2(grid.cellSize.x * ratio, grid.cellSize.y * ratio);
        }
        
        private void AdjustText(float ratio)
        {
            var text = GetComponent<Text>();
            if(text == null) {return;}
            text.fontSize = (int)(text.fontSize * ratio);
        }
    }
}