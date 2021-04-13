using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace personaltools.textlocalizedtool
{
    [System.Serializable]
    public class LocalisedString
    {
        public string key;

        public LocalisedString(string key)
        {
            this.key = key;
        }

        public string Value
        {
            get
            {
                return LocalizationSystem.GetLocalisedValue(key);
            }
        }

        public static implicit operator LocalisedString(string key)
        {
            return new LocalisedString(key);
        }

    }
}

