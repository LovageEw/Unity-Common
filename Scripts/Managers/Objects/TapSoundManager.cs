using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Collections;
using UnityEngine.EventSystems;
using UIs;
using UnityEngine.UI;

namespace Managers
{
    public class TapSoundManager : MonoBehaviour
    {
        [SerializeField] private AudioClip tapButton = null;
        [SerializeField] private AudioClip tapReturn = null;

        private void Start()
        {
            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Where(_ => EventSystem.current != null)
                .Where(_ => EventSystem.current.currentSelectedGameObject != null)
                .Select(_ => EventSystem.current.currentSelectedGameObject.GetComponent<TapSound>())
                .Subscribe(ts =>
                {
                    if (ts != null)
                    {
                        ts.PlayTapSound();
                    }
                    else
                    {
                        PlayDefaultSe(EventSystem.current.currentSelectedGameObject);
                    }
                });
        }

        private void PlayDefaultSe(GameObject clickedObject)
        {
            var button = clickedObject.GetComponent<Button>();
            if (button != null && button.targetGraphic != null && button.targetGraphic.mainTexture.name == "return" &&
                tapReturn != null)
            {
                SoundManager.Instance.PlaySe(tapReturn);
            }
            else if (button != null && tapButton != null)
            {
                SoundManager.Instance.PlaySe(tapButton);
            }
        }
    }
}