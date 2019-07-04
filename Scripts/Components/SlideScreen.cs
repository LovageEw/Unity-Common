using System;
using System.Collections.Generic;
using System.Linq;
using Commons.Scripts.Extensions;
using Managers;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace UIs
{
    public class SlideScreen : MonoBehaviour
    {
        [SerializeField] private string screenName;
        [SerializeField] private Direction moveInDirection;
        [SerializeField] private bool isLeaveToScene;

        public string ScreenName => screenName;
        public Direction MoveInDirection => moveInDirection;
        public bool IsLeaveToScene => isLeaveToScene;

        private Vector3 initialPosition;

        public Vector3 InitialPosition
        {
            get { return initialPosition; }
            set
            {
                initialPosition = value;
                transform.position = initialPosition;
            }
        }

        public GameObject CreateScreen(GameObject parent)
        {
            var screen = gameObject;
            if (!gameObject.activeInHierarchy)
            {
                screen = parent.Create(gameObject);
            }

            var rect = screen.GetComponent<RectTransform>();
            var parentRect = parent.GetComponent<RectTransform>();
            rect.sizeDelta = parentRect.GetSize();
            return screen;
        }

        public virtual void OnEndSlide()
        {
        }
    }
}