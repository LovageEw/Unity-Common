using System;
using UnityEngine;

public static class UnityConsole
{
    public static void Log(object message)
    {
#if UNITY_EDITOR || UNITY_WEBGL
        Debug.Log(message);
#endif
    }
    
    public static void LogException(Exception exception)
    {
#if UNITY_EDITOR || UNITY_WEBGL
        Debug.LogException(exception);
#endif
    }
    
    public static void LogWarning(object message)
    {
#if UNITY_EDITOR || UNITY_WEBGL
        Debug.LogWarning(message);
#endif
    }
}