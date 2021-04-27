using System;
using UnityEngine;

namespace personaltools.textlocalizedtool
{
    public class EnumPartial : PropertyAttribute
    { 
        public Type type;
        public string propertyName;

        public EnumPartial(Type type, string propertyName)
        {
            this.type = type;
            this.propertyName = propertyName;
        }

    }
}
