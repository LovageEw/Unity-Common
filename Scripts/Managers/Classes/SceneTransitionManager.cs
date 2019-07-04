using UnityEngine;
using System.Collections;
using Controllers;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class SceneTransitionManager : Singleton<SceneTransitionManager>
    {
        const float FadeTime = 0.5f;

        public void ChangeScene(string sceneName) {
            FadingController.Instance.FadeOut(FadeTime, () => {
                SceneManager.LoadScene(sceneName);
                FadingController.Instance.FadeIn(FadeTime);
            });
        }

        public void ChangePage(GameObject previous, GameObject next) {
            FadingController.Instance.FadeOut(FadeTime, () => {
                previous.SetActive(false);
                next.SetActive(true);
                FadingController.Instance.FadeIn(FadeTime);
            });
        }
    }
}