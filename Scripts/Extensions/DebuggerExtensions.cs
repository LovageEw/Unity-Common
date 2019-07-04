using UnityEngine;

public static class LoggerExtensions
{
    public static void ColorLog(this Logger logger, string log, Color color)
    {
        logger.Log("<color=" + color.ToHexString() + ">" + log + "</color>");
    }
}