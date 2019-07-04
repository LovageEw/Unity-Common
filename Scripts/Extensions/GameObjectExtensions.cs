using UnityEngine;
using System.Collections;

public static class GameObjectExtensions {

    public static GameObject Create(this GameObject parent , GameObject prefab) {
        var item = Object.Instantiate(prefab);
        item.transform.SetParent(parent.transform);
        item.transform.localScale = Vector3.one;
        item.transform.localRotation = new Quaternion(0, 0, 0, 0);

        return item;
    }
}
