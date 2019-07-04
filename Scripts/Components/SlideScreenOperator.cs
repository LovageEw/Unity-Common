using System;
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
    public class SlideScreenOperator : MonoBehaviour
    {
        [SerializeField] private Transform centerCanvas;
        [SerializeField] private Transform leftCanvas;
        [SerializeField] private Transform rightCanvas;
        [SerializeField] private Transform upCanvas;
        [SerializeField] private Transform downCanvas;
        [Space(10)]
        [SerializeField] private SlideScreen initialScreen;
        [SerializeField] private List<SlideScreen> screens;
        [SerializeField] private Ease ease;
        [SerializeField][Range(0f,10f)] private float slideDuration = 1f;
        [Space(10)]
        [SerializeField] private bool isRememberStack;
        [SerializeField, Tooltip("Is Remember Stackでないなら不要")]
        private string sceneName;

        public int StackCount => screenStack?.Count ?? 0;

        private Stack<string> screenStack;
        private readonly List<SlideScreen> instantiated = new List<SlideScreen>();
        public SlideScreen Current { get; private set; }
        private Sequence sequence;

        private void Start()
        {
            Current = initialScreen;
            initialScreen.GetComponent<RectTransform>().sizeDelta = gameObject.GetComponent<RectTransform>().GetSize();
            if (!isRememberStack)
            {
                screenStack = new Stack<string>();
            }
            else
            {
                screenStack = SlideScreenStacker.Instance.GetStackReference(sceneName);
                var preserved = slideDuration;
                slideDuration = 0;
                Backward();
                slideDuration = preserved;
            }
        }

        public bool Forward(string screenName, bool isPushStack = true)
        {
            if (sequence != null) { return false; }

            if (isPushStack)
            {
                screenStack.Push(Current.ScreenName);
            }

            var isSucceeded = SlideScreen(screenName, false);
            
            if (!isSucceeded)
            {
                screenStack.Pop();
            }

            return isSucceeded;
        }

        public bool Backward()
        {
            if (sequence != null || StackCount == 0) { return false; }

            string previous = screenStack.Pop();
            var isSucceeded = SlideScreen(previous, true);

            if (!isSucceeded)
            {
                screenStack.Push(previous);
            }

            return isSucceeded;
        }

        public bool PushCurrentToStack()
        {
            if (sequence != null) { return false; }
            if (screenStack.Count > 0 && screenStack.Peek() == Current.ScreenName) { return false; }

            screenStack.Push(Current.ScreenName);
            return true;
        }
        
        private bool SlideScreen(string screenName , bool isBackward)
        {
            if (centerCanvas == null) { return false;}
            var targetScreen = instantiated.FirstOrDefault(x => x.ScreenName == screenName);
            if (targetScreen != null)
            {
                return DoSlide(targetScreen, isBackward);
            }
            targetScreen = screens.FirstOrDefault(x => x.ScreenName == screenName);
            if (targetScreen == null) {return false;}
            
            var targetGameObject = CreateTargetScreen(targetScreen);
            return DoSlide(targetGameObject.GetComponent<SlideScreen>(), isBackward);
        }

        private GameObject CreateTargetScreen(SlideScreen targetScreen)
        {
            var generated = targetScreen.CreateScreen(centerCanvas.gameObject);
                
            switch (targetScreen.MoveInDirection)
            {
                case Direction.Left:
                    generated.GetComponent<SlideScreen>().InitialPosition = rightCanvas.position;
                    break;
                case Direction.Right:
                    generated.GetComponent<SlideScreen>().InitialPosition = leftCanvas.position;
                    break;
                case Direction.Up:
                    generated.GetComponent<SlideScreen>().InitialPosition = downCanvas.position;
                    break;
                case Direction.Down:
                    generated.GetComponent<SlideScreen>().InitialPosition = upCanvas.position;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return generated;
        }

        private bool DoSlide(SlideScreen targetScreen , bool isBackward)
        {
            Vector3 currentScreenMoveTo;
            try
            {
                if (isBackward)
                {
                    switch (Current.MoveInDirection)
                    {
                        case Direction.Left:
                            currentScreenMoveTo = rightCanvas.transform.position;
                            targetScreen.transform.position = leftCanvas.transform.position;
                            break;
                        case Direction.Right:
                            currentScreenMoveTo = leftCanvas.transform.position;
                            targetScreen.transform.position = rightCanvas.transform.position;
                            break;
                        case Direction.Up:
                            currentScreenMoveTo = downCanvas.transform.position;
                            targetScreen.transform.position = upCanvas.transform.position;
                            break;
                        case Direction.Down:
                            currentScreenMoveTo = upCanvas.transform.position;
                            targetScreen.transform.position = downCanvas.transform.position;
                            break;
                        default:
                            return false;
                    }
                }
                else
                {
                    switch (targetScreen.MoveInDirection)
                    {
                        case Direction.Left:
                            currentScreenMoveTo = leftCanvas.transform.position;
                            targetScreen.transform.position = rightCanvas.transform.position;
                            break;
                        case Direction.Right:
                            currentScreenMoveTo = rightCanvas.transform.position;
                            targetScreen.transform.position = leftCanvas.transform.position;
                            break;
                        case Direction.Up:
                            currentScreenMoveTo = upCanvas.transform.position;
                            targetScreen.transform.position = downCanvas.transform.position;
                            break;
                        case Direction.Down:
                            currentScreenMoveTo = downCanvas.transform.position;
                            targetScreen.transform.position = upCanvas.transform.position;
                            break;
                        default:
                            return false;
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                UnityConsole.Log(ex.ToString());
                return false;
            }

            ExecuteSequence(targetScreen , currentScreenMoveTo);

            return true;
        }

        private void ExecuteSequence(SlideScreen targetScreen, Vector3 currentScreenMoveTo)
        {
            sequence = DOTween.Sequence()
                .Append(targetScreen.transform.DOMove(centerCanvas.transform.position, slideDuration).SetEase(ease))
                .Join(Current.transform.DOMove(currentScreenMoveTo, slideDuration).SetEase(ease)).OnComplete(() =>
                {
                    OnEndSequence(targetScreen);
                });

            sequence.Play();
        }

        private void OnEndSequence(SlideScreen targetScreen)
        {
            targetScreen.OnEndSlide();
            if (Current.IsLeaveToScene)
            {
                instantiated.Add(Current);
            }
            else
            {
                Destroy(Current.gameObject);
            }
            
            Current = targetScreen;
            sequence = null;
        }
    }
}
