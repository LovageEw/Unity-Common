using System;
using UnityEngine;

public static class UnityConsole
{
    public static void Log(object message)
    {
#if UNITY_EDITOR
        Debug.Log(message);
#endif
    }
    
    public static void LogException(Exception exception)
    {
#if UNITY_EDITOR
        Debug.LogException(exception);
#endif
    }
    
    public static void LogWarning(object message)
    {
#if UNITY_EDITOR
        Debug.LogWarning(message);
#endif
    }
}