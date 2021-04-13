using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace personaltools.textlocalizedtool.editor
{
    [CustomPropertyDrawer(typeof(LocalisedString))]
    public class LocalisedStringDrawer : PropertyDrawer
    {
        bool dropdown;
        float height;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (dropdown)
            {
                return height + 25;
            }

            return 20;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            position.width -= 34;
            position.height = 20;

            Rect valueRect = new Rect(position);
            valueRect.x += 15;
            valueRect.width -= 15;

            Rect foldButtonRect = new Rect(position)
            {
                width = 15
            };

            dropdown = EditorGUI.Foldout(foldButtonRect, dropdown, "");

            position.x += 5;
            position.width -= 15;

            SerializedProperty key = property.FindPropertyRelative("key");
            key.stringValue = EditorGUI.TextField(position, key.stringValue);

            position.x += position.width + 2;
            position.width = 20;
            position.height = 20;

            GUIContent searchContent = EditorGUIUtility.IconContent("Search Icon");
            if (GUI.Button(position, searchContent))
            {
                TextLocaliserSearchWindow.Open();
            }

            position.x += position.width + 2;

            GUIContent storeContent = EditorGUIUtility.IconContent("CreateAddNew");

            if (GUI.Button(position, storeContent))
            {
                TextLocaliserEditWindow.Open(key.stringValue);
            }

            if (dropdown)
            {
                var value = LocalizationSystem.GetLocalisedValue(key.stringValue);
                var lan = LocalizationSystem.Language.ToString() + " : ";

                GUIStyle style = GUI.skin.box;
                height = style.CalcHeight(new GUIContent(value), valueRect.width);

                valueRect.height = height;
                valueRect.y += 21;
                EditorGUI.LabelField(valueRect, lan);

                valueRect.y += 2;
                valueRect.x += 60;
                EditorGUI.LabelField(valueRect, value, EditorStyles.wordWrappedLabel);
            }

            EditorGUI.EndProperty();
        }

    }

}