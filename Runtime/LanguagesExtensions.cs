using System;
using System.Reflection;

namespace personaltools.textlocalizedtool
{
    public static class LanguagesExtensions
    {
        public static string GetStringValuesAtribute(this Languages value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].StringValue : null;
        }

        public static Languages GetLanguageEnumFromCode(this string value)
        {
            foreach (Languages lan in (Languages[])Enum.GetValues(typeof(Languages)))
            {
                if (value.CompareTo(lan.GetStringValuesAtribute()) == 0)
                    return lan;
            }
            return Languages.Custom_Language;
        }

        public static Languages GetLanguageEnum(this string value)
        {
            foreach (Languages lan in (Languages[])Enum.GetValues(typeof(Languages)))
            {
                if (value.CompareTo(lan.ToString()) == 0)
                    return lan;
            }
            return Languages.Custom_Language;
        }

        public static int GetLanguagesIndex(this string value)
        {
            foreach (Languages lan in (Languages[])Enum.GetValues(typeof(Languages)))
            {
                if (value.CompareTo(lan.ToString()) == 0)
                    return (int)lan;
            }

            return -1;
        }

        public static int GetLanguagesIndexFromCode(this string value)
        {
            foreach (Languages lan in (Languages[])Enum.GetValues(typeof(Languages)))
            {
                if (value.CompareTo(lan.GetStringValuesAtribute()) == 0)
                {
                    return (int)lan;
                }                    
            }
            return -1;
        }
    }
}
