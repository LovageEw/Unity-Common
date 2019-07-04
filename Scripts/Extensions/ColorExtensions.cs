using UnityEngine;

public static class ColorExtensions {

    public static Color SetR(this Color color, float r) {
        return color = new Color(r, color.g, color.b, color.a);
    }
    public static Color SetG(this Color color, float g) {
        return color = new Color(color.r, g, color.b, color.a);
    }
    public static Color SetB(this Color color, float b) {
        return color = new Color(color.r, color.g, b, color.a);
    }
    public static Color SetA(this Color color, float a) {
        return color = new Color(color.r, color.g, color.b, a);
    }

    public static string ToHexString(this Color color)
    {
        return "#" 
            + ((int)(color.r * 255)).ToString("X2") 
            + ((int)(color.g * 255)).ToString("X2") 
            + ((int)(color.b * 255)).ToString("X2") 
            + ((int)(color.a * 255)).ToString("X2");
    }
}
