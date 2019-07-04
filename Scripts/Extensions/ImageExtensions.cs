using UnityEngine.UI;

public static class ImageExtensions{

    public static void SetR(this Image image, float red) {
        image.color = image.color.SetR(red);
    }
    public static void SetG(this Image image, float green) {
        image.color = image.color.SetG(green);
    }
    public static void SetB(this Image image, float blue) {
        image.color = image.color.SetB(blue);
    }
    public static void SetA(this Image image, float alpha) {
        image.color = image.color.SetA(alpha);
    }

}
