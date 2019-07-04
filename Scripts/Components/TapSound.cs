using UnityEngine;
using System.Collections;
using Managers;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

namespace UIs
{
    public class TapSound : MonoBehaviour
    {
        [SerializeField] private AudioClip tapSound;

        private Button button;
        private Toggle toggle;

        private void Start()
        {
            button = GetComponent<Button>();
            if (button != null)
            {
                button.OnClickAsObservable().Subscribe(_ => PlayTapSound());
            }
            toggle = GetComponent<Toggle>();
            if (toggle != null)
            {
                toggle.OnPointerClickAsObservable().Subscribe(_ => PlayTapSound());
            }
        }
        
        public void PlayTapSound()
        {
            if (tapSound != null)
            {
                SoundManager.Instance.PlaySe(tapSound, false);
            }
        }
    }
}