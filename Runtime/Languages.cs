using System;
using System.Reflection;

namespace personaltools.textlocalizedtool
{
    public enum Languages 
    {
        [StringValue("en")]
        English,
        [StringValue("fr")]
        France,
        [StringValue("ind")]
        Indonesia 
    };

    [Serializable]
    public class StringValueAttribute : Attribute
    {
        public string StringValue { get; protected set; }

        public StringValueAttribute(string value)
        {
            this.StringValue = value;
        }
    }

    public static class LanguagesExtensions
    {
        public static string GetStringValue(this Languages value)
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
    }
}
