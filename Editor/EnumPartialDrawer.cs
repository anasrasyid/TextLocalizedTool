using UnityEditor;
using UnityEngine;

namespace personaltools.textlocalizedtool.Editor
{
    public class Pair<T, U>
    {
        public Pair()
        {
        }

        public Pair(T first, U second)
        {
            this.First = first;
            this.Second = second;
        }

        public T First { get; set; }
        public U Second { get; set; }
    };


    [CustomPropertyDrawer(typeof(EnumPartial))]
    public class EnumPartialDrawer : PropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EnumPartial atb = attribute as EnumPartial;
            int[] showIndex = null;

            if (atb.type.GetField(atb.propertyName) != null)
            {
                showIndex = atb.type.GetField(atb.propertyName).GetValue(atb.type) as int[];
            }

            if (showIndex == null || showIndex.Length <= 0)
            {
                EditorGUI.PropertyField(position, property);
                return;
            }

            EditorGUI.BeginProperty(position, label, property);                         

            // Store Name and Real Index
            string[] items = new string[showIndex.Length];
            Pair<int, int>[] arrayIndex = new Pair<int, int>[showIndex.Length];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = property.enumNames[showIndex[i]];
                arrayIndex[i] = new Pair<int, int>(i, showIndex[i]);
            }

            // Get Selected Index
            int index = -1;
            for (int i = 0; i < arrayIndex.Length; i++)
            {
                if (arrayIndex[i].Second == property.enumValueIndex)
                {
                    index = arrayIndex[i].First;
                }
            }

            // Display popup
            index = EditorGUI.Popup(position, label.text, index, items);

            // Get Real Index
            int realIndex = arrayIndex[0].Second;
            for (int i = 0; i < arrayIndex.Length; i++)
            {
                if (arrayIndex[i].First == index)
                {
                    realIndex = arrayIndex[i].Second;
                }
            }

            // Set Enum Value
            property.enumValueIndex = realIndex;

            // Default
            //EditorGUI.PropertyField(position, property, new GUIContent("*" + label.text));

            EditorGUI.EndProperty();

        }
    }
}
