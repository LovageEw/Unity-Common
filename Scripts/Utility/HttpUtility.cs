using UnityEngine;

namespace Commons.Scripts.Utility
{
    public static class HttpUtility
    {
        public static void Open(string url)
        {
#if UNITY_EDITOR
            Application.OpenURL(url);
#elif UNITY_WEBGL
            Application.ExternalEval($"window.open('{url}','_blank')");
#else
            Application.OpenURL(url);
#endif
        }
    }
}