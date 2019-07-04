using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using System;
using Commons.Scripts.Extensions;

namespace UIs {
    [RequireComponent(typeof(RectTransform))]
    public class MoveInOutObject : MonoBehaviour {
        [SerializeField] private bool fireOnStart = false;
        [SerializeField] private Direction moveInDirection = Direction.Left;
        [SerializeField] private Ease easeMoveIn = Ease.Unset;
        [SerializeField , Range(0.0f , 3.0f)] private float moveInTime = 1.0f;
        [SerializeField] private Ease easeMoveOut = Ease.Unset;
        [SerializeField , Range(0.0f , 3.0f)] private float moveOutTime = 1.0f;

        protected Vector3 moveTo;
        protected Vector3 moveFrom;
        protected RectTransform rect;
        private Tweener tween;

        protected virtual void Start(){
            rect = GetComponent<RectTransform>();
            moveTo = rect.anchoredPosition3D;
            switch (moveInDirection) {
                case Direction.Left:
                    moveFrom = moveTo.SetX(-rect.GetSize().x);
                    break;
                case Direction.Right:
                    moveFrom = moveTo.SetX(rect.GetSize().x);
                    break;
                case Direction.Up:
                    moveFrom = moveTo.SetY(-rect.GetSize().y);
                    break;
                case Direction.Down:
                    moveFrom = moveTo.SetY(rect.GetSize().y);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            rect.anchoredPosition3D = moveFrom;
            if (fireOnStart)
            {
                MoveIn(moveFrom);
            }
        }

        public virtual void MoveIn(Action onFinish = null) {
            MoveIn(moveFrom, onFinish);
        }

        public virtual void MoveIn(Vector3 moveFrom, Action onFinish = null) {
            gameObject.SetActive(true);
            StopTweenOperation();
            tween = rect.DOAnchorPos3D(moveTo, moveInTime).SetEase(easeMoveIn);
            tween.OnComplete(() => {
                tween = null;
                onFinish?.Invoke();
            });
        }

        public virtual void MoveOut(Action onFinish = null) {
            MoveOut(moveFrom, onFinish);
        }

        public virtual void MoveOut(Vector3 moveTo, Action onFinish = null) {
            StopTweenOperation();
            tween = rect.DOAnchorPos3D(moveTo, moveOutTime).SetEase(easeMoveOut);
            tween.OnComplete(() => {
                gameObject.SetActive(false);
                tween = null;
                onFinish?.Invoke();
            });
        }

        public void StopTweenOperation() {
            tween?.Kill();
            moveFrom = rect.anchoredPosition3D;
        }
    }
}