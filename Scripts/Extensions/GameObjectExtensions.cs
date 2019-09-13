using UnityEngine;
using System.Collections;

public static class GameObjectExtensions
{
    public static GameObject Create(this GameObject parent, GameObject prefab)
    {
        return parent.Create(prefab, true);
    }
    
    public static GameObject Create(this GameObject parent, GameObject prefab, bool isClearScaleAndPosition)
    {
        var item = Object.Instantiate(prefab, parent.transform, true);
        if (!isClearScaleAndPosition) {return item;}
        
        item.transform.localScale = Vector3.one;
        item.transform.localRotation = new Quaternion(0, 0, 0, 0);
        return item;
    }
}