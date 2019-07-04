using System;
using System.Collections.Generic;
using Constants;
using UnityEngine;

namespace Settings
{
    public class FloatPlayerPrefs
    {
        private readonly string key;
        private readonly float defaultValue;
        private float value;
        
        public FloatPlayerPrefs(string key , float defaultValue = 0 )
        {
            this.key = key;
            this.defaultValue = defaultValue;
            
            value = defaultValue;
        }

        public void Set(float value)
        {
            this.value = value;
            PlayerPrefs.SetFloat( key, value );
        }

        public float Get()
        {
            if (!PlayerPrefs.HasKey(key))
            {
                Set(defaultValue);
            }
            else if(Math.Abs(value - defaultValue) < CalcHelper.FloatTolerance)
            {
                value = PlayerPrefs.GetFloat(key);
            }
            return value;
        }

        public void Clear()
        {
            value = defaultValue;
            PlayerPrefs.DeleteKey(key);
        }
        
        public bool HasData => Math.Abs(Get() - defaultValue) > CalcHelper.FloatTolerance;
    }
}