﻿using UnityEngine;
using UnityEngine.UI;

public static class Vector3Extensions {

    public static Vector3 SetX(this Vector3 vec, float x) {
        return new Vector3(x, vec.y, vec.z);
    }
    public static Vector3 SetY(this Vector3 vec, float y) {
        return new Vector3(vec.x, y, vec.z);
    }
    public static Vector3 SetZ(this Vector3 vec, float z) {
        return new Vector3(vec.x, vec.y, z);
    }

}
