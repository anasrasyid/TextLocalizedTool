using System;
using UnityEngine;

namespace personaltools.textlocalizedtool
{
    public class EnumPartialAtribute : PropertyAttribute
    { 
        public Type type;
        public string propertyName;

        public EnumPartialAtribute(Type type, string propertyName)
        {
            this.type = type;
            this.propertyName = propertyName;
        }

    }
}
