using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Settings;

namespace Managers
{
    public class SoundManager : SingletonMonoBehaviour<SoundManager>
    {
        [SerializeField] private AudioSource bgm;
        private readonly FloatPlayerPrefs bgmVolumeSetting = new FloatPlayerPrefs("BgmVolumeSettingKey" , -1f);
        private readonly FloatPlayerPrefs seVolumeSetting = new FloatPlayerPrefs("SeVolumeSettingKey" , -1f);

        public AudioClip CurrentSong => bgm.clip;
        private float bgmVolume;
        public float BgmVolume{
            get { return bgmVolume; }
            set {
                if (bgm != null) {
                    bgm.volume = value;
                }
                bgmVolume = value;
                bgmVolumeSetting.Set(value);
            }
        }

        private float seVolume;
        public float SeVolume {
            get { return seVolume; }
            set {
                if (seList != null) {
                    foreach (AudioSource se in seList) {
                        se.volume = value;
                    }
                }
                seVolume = value;
                seVolumeSetting.Set(value);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
            BgmVolume = bgmVolumeSetting.HasData ? bgmVolumeSetting.Get() : 1.0f;
            SeVolume = seVolumeSetting.HasData ? seVolumeSetting.Get() : 1.0f;
        }

        public void PlayBgm(AudioClip clip , float fadeTime = 0f, bool notReplaySameBgm = true, bool isLoop = true)
        {
            if (bgm == null) { return;}
            if (notReplaySameBgm && bgm.clip == clip) { return; }

            if (bgm.clip == null || bgm.clip.name != clip.name) {
                bgm.clip = clip;
                bgm.loop = isLoop;
            }
            FadeIn(fadeTime);
        }

        public void StopBgm(float fadeTime = 0f)
        {
            if (!bgm.isPlaying) { return; }
            
            FadeOut(fadeTime);
        }
        
        private void FadeIn(float fadeTime)
        {
            bgm.Play();
            bgm.volume = 0;
            bgm.DOFade(bgmVolume, fadeTime).SetEase(Ease.Linear);
        }

        private void FadeOut(float fadeTime)
        {
            bgm.DOFade(0, fadeTime).SetEase(Ease.Linear).OnComplete(() =>
            {
                bgm.clip = null;
            });
        }

        List<AudioSource> seList = new List<AudioSource>();
        public void PlaySe(AudioClip clip , bool isStopCurrent = true)
        {
            if (isStopCurrent)
            {
                StopSe(clip);
            }

            var availableSe = seList.FirstOrDefault(x => !x.isPlaying);
            if (availableSe == null)
            {
                availableSe = gameObject.AddComponent<AudioSource>();
                availableSe.volume = seVolume;
                seList.Add(availableSe);
            }
            
            availableSe.clip = clip;
            availableSe.PlayOneShot(clip);
            // StartCoroutine(SoundStopTask(availableSe));
        }

        public void StopSe(AudioClip clip)
        {
            foreach (var audioSource in seList.Where(x => x.clip == clip))
            {
                audioSource.Stop();
            }
        }

        IEnumerator SoundStopTask(AudioSource audio)
        {
            yield return new WaitForSeconds(audio.clip.length);
            audio.Stop();
        }
    }
}