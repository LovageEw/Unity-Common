using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Commons.Scripts.Extensions;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace UIs
{
    public enum PagingScreenDirection
    {
        Vertical,
        Horizontal,
    }
    
    [RequireComponent(typeof(RectTransform))]
    public class PagingScreen : MonoBehaviour
    {
        [SerializeField] private PagingScreenDirection direction;
        [SerializeField] private GameObject[] pageObjectList;
        [SerializeField] private Ease ease;
        [SerializeField][Range(0f,10f)] private float slideDuration = 1f;
        [SerializeField] private int firstPage;
        
        private RectTransform ownRect;
        public int PageCount { get; private set; }
        public int PageNumber { get; private set; }
  
        private void OnEnable()
        {
            ownRect = GetComponent<RectTransform>();
            AddLayoutGroup();
            AddSerializedChildren();

            this.LateUpdateAsObservable().Buffer(5).Select(_ => ownRect.GetSize()).DistinctUntilChanged().Subscribe(size =>
            {
                Enumerable.Range(0,ownRect.childCount).ForEach(i =>
                {
                    var targetTransform = ownRect.GetChild(i).GetComponent<RectTransform>();
                    targetTransform.sizeDelta = size;
                    targetTransform.localPosition = new Vector3(targetTransform.localPosition.x, targetTransform.localPosition.y, 0);
                });
                PageCount = ownRect.childCount;
                ResetAlignment();
                MoveToPage(PageNumber , 0);
            });
            
            MoveToPage(firstPage , 0);
        }

        private void ResetAlignment()
        {
            var layout = GetComponent<LayoutGroup>();
            var temp = layout.childAlignment;
            layout.childAlignment = TextAnchor.LowerCenter;
            layout.childAlignment = TextAnchor.UpperLeft;
            layout.childAlignment = temp;
        }

        private void AddSerializedChildren()
        {
            Display(go => pageObjectList.Select(go.Create).ToArray());
        }

        private void AddLayoutGroup()
        {
            if (direction == PagingScreenDirection.Horizontal)
            {
                var hLayout = gameObject.AddComponent<HorizontalLayoutGroup>();
                hLayout.childControlHeight = false;
                hLayout.childControlWidth = false;
                hLayout.childForceExpandHeight = false;
                hLayout.childForceExpandWidth = false;
            }
            else
            {
                var vLayout = gameObject.AddComponent<VerticalLayoutGroup>();
                vLayout.childControlHeight = false;
                vLayout.childControlWidth = false;
                vLayout.childForceExpandHeight = false;
                vLayout.childForceExpandWidth = false;
            }
        }

        public void Display(Func<GameObject, GameObject[]> createEachItemFunc)
        {
            foreach (var item in createEachItemFunc(gameObject))
            {
                item.GetComponent<RectTransform>().sizeDelta = ownRect.GetSize();
                PageCount++;
            }
        }

        public void MoveToPage(int pageNumber)
        {
            MoveToPage(pageNumber , slideDuration);
        }

        public void MoveToPage(int pageNumber , float duration)
        {
            if (PageCount <= pageNumber) { return; }

            PageNumber = pageNumber;

            if (direction == PagingScreenDirection.Horizontal)
            {
                ownRect.DOLocalMoveX(-ownRect.GetSize().x * pageNumber, duration).SetEase(ease);
            }
            else
            {
                ownRect.DOLocalMoveY(-ownRect.GetSize().y * pageNumber, duration).SetEase(ease);
            }
        }

        public void MoveLeft()
        {
            MoveToPage(PageNumber - 1);
        }

        public void MoveRight()
        {
            MoveToPage(PageNumber + 1);
        }
    }
}