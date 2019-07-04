using System;
using System.Collections.Generic;
using Constants;
using UnityEngine;

namespace Settings
{
    public class StringPlayerPrefs
    {
        private readonly string key;
        private readonly string defaultValue;
        private string value;
        
        public StringPlayerPrefs(string key , string defaultValue = "" )
        {
            this.key = key;
            this.defaultValue = defaultValue;
            
            value = defaultValue;
        }

        public void Set(string value)
        {
            this.value = value;
            PlayerPrefs.SetString( key, value );
        }

        public string Get()
        {
            if (!PlayerPrefs.HasKey(key))
            {
                Set(defaultValue);
            }
            else if(value == defaultValue)
            {
                value = PlayerPrefs.GetString(key);
            }
            return value;
        }

        public void Clear()
        {
            value = defaultValue;
            PlayerPrefs.DeleteKey(key);
        }
        
        public bool HasData => Get() != defaultValue;
    }
}