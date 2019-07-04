using System;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    public class SmartPlayerPref<T> where T : class
    {
        private readonly string key;
        private readonly T defaultValue;
        private T value;
        
        public SmartPlayerPref(string key , T defaultValue = default(T) )
        {
            this.key = key;
            this.defaultValue = defaultValue;
            
            value = defaultValue;
        }

        public void Set(T value)
        {
            this.value = value;
            var jsonValue = JsonUtility.ToJson( value );
            PlayerPrefs.SetString( key, jsonValue );
        }

        public T Get()
        {
            if (!PlayerPrefs.HasKey(key))
            {
                Set(defaultValue);
            }
            else if(EqualityComparer<T>.Default.Equals(value , defaultValue))
            {
                var jsonValue = PlayerPrefs.GetString(key);
                value = JsonUtility.FromJson<T>(jsonValue);
            }
            return value;
        }

        public void Clear()
        {
            value = defaultValue;
            PlayerPrefs.DeleteKey(key);
        }
        
        public bool HasData => !EqualityComparer<T>.Default.Equals(Get() , defaultValue);
    }
}