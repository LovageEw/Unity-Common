using UnityEngine;
using System.Collections;

public class Singleton<T> where T : Singleton<T> , new() {

    static T mInstance;

    public static T Instance {
        get {
            if (mInstance == null) {
                mInstance = new T();
            }
            return mInstance;
        }
    }
}
