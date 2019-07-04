using UnityEngine;
using System.Collections;
using System;

public static class ActionExtensions {

    public static void SafeAction(this Action action)
    {
        action?.Invoke();
    }
    
    public static void SafeAction<T>(this Action<T> action, T arg)
    {
        action?.Invoke(arg);
    }
}
