using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Collections;
using UnityEngine.EventSystems;
using UIs;
using UnityEngine.UI;

namespace Managers {
    public class TapSoundManager : MonoBehaviour {
        [SerializeField] AudioClip tapButton = null;
        [SerializeField] AudioClip tapReturn = null;

        void Start(){
            this.UpdateAsObservable ()
                .Where (_ => Input.GetMouseButtonDown (0) && EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
                .Select (_ => EventSystem.current.currentSelectedGameObject.GetComponent<TapSound> ())
                .Subscribe (ts => {
                    if (ts != null) {
                        ts.PlayTapSound();
                    } else {
                        PlayDefaultSe(EventSystem.current.currentSelectedGameObject);
                    }
            });
        }

        void PlayDefaultSe(GameObject clickedObject) {
            var button = clickedObject.GetComponent<Button>();
            if (button != null && button.targetGraphic != null && button.targetGraphic.mainTexture.name == "return") {
                SoundManager.Instance.PlaySe(tapReturn );
            } else if (button != null) {
                SoundManager.Instance.PlaySe(tapButton);
            }
        }
    }
}