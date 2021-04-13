using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace personaltools.textlocalizedtool.editor
{
    public class LocaliserWindow : EditorWindow
    {
        [MenuItem("Window/Localiser")]
        public static void Open()
        {
            LocaliserWindow window = (LocaliserWindow)EditorWindow.GetWindow<LocaliserWindow>();
            window.titleContent.text = "Localiser Window";
            window.minSize = new Vector2(480, 300);
            window.Show();
        }

        Languages defaultLanguage = LocalizationSystem.DefaultLanguage;
        Languages currentLanguage = LocalizationSystem.Language;

        string value;
        private Vector2 scroll;
        private bool isAutoTranslate;
        private Languages autoLanguage;

        public void OnEnable()
        {
            LocalizationSystem.DefaultLanguage = defaultLanguage;
        }

        public void OnGUI()
        {
            SearchBar();

            ShowAll();
            GetSearchResults();

            ShowLanguageInCSV();

            ChangeLanguage();
        }

        public void ChangeLanguage()
        {
            if (LocalizationSystem.Language != currentLanguage)
            {
                LocalizationSystem.Language = currentLanguage;
            }
            if (LocalizationSystem.DefaultLanguage != defaultLanguage)
            {
                LocalizationSystem.DefaultLanguage = defaultLanguage;
            }
        }

        public void SearchBar()
        {
            EditorGUILayout.BeginHorizontal("Box");
            EditorGUILayout.LabelField("Search: ", EditorStyles.boldLabel);
            value = EditorGUILayout.TextField(value);

            GUIContent addContent = EditorGUIUtility.IconContent("CreateAddNew", "Add New Key");
            if (GUILayout.Button(addContent, GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
            {
                TextLocaliserEditWindow.Open(value);
            }
            EditorGUILayout.EndHorizontal();
        }

        public void ShowLanguageInCSV()
        {
            GUILayout.Label("Language", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal("box");
            defaultLanguage = (Languages)EditorGUILayout.EnumPopup
                ("Editor Language :", defaultLanguage, GUILayout.MinWidth(220));

            currentLanguage = (Languages)EditorGUILayout.EnumPopup
                ("Current Language : ", currentLanguage, GUILayout.MinWidth(220));

            EditorGUILayout.EndHorizontal();
        }

        public void ShowAll()
        {
            GUILayout.Label("Dictionary Item", EditorStyles.boldLabel);
            if (value != null) { return; }

            EditorGUILayout.BeginVertical();
            scroll = EditorGUILayout.BeginScrollView(scroll);
            var dictionary = LocalizationSystem.GetDictionartForEditor();
            foreach (KeyValuePair<string, string> element in dictionary)
            {
                DrawItem(element);
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        public void GetSearchResults()
        {
            if (value == null) { return; }

            EditorGUILayout.BeginVertical();
            scroll = EditorGUILayout.BeginScrollView(scroll);
            var dictionary = LocalizationSystem.GetDictionartForEditor();
            foreach (KeyValuePair<string, string> element in dictionary)
            {
                bool isKey = element.Key.ToLower().Contains(value.ToLower());
                bool isValue = element.Value.ToLower().Contains(value.ToLower());
                if (isKey || isValue)
                {
                    DrawItem(element);
                }
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        public void DrawItem(KeyValuePair<string, string> element)
        {
            EditorGUILayout.BeginHorizontal("box");

            GUIContent deleteContent = EditorGUIUtility.IconContent("winbtn_win_close", "Remove");
            if (GUILayout.Button(deleteContent, GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
            {
                if (EditorUtility.DisplayDialog("Remove Key " + element.Key + "?",
                    "This will remove the element from localisation, are sure?", "Do it"))
                {
                    LocalizationSystem.Remove(element.Key);
                    AssetDatabase.Refresh();
                    LocalizationSystem.Init();
                }
            }

            GUIContent editContent = EditorGUIUtility.IconContent("_Popup", "Remove");
            if (GUILayout.Button(editContent, GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
            {
                TextLocaliserEditWindow.Open(element.Key, element.Value);
            }

            EditorGUILayout.TextField(element.Key);
            EditorGUILayout.LabelField(element.Value);
            EditorGUILayout.EndHorizontal();
        }
    }
}
