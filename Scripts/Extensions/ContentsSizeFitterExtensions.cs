using UnityEngine.UI;

public static class ContentsSizeFitterExtensions
{
    public static void Reload(this ContentSizeFitter fitter)
    {
        fitter.enabled = false;
        fitter.enabled = true;
    }
}