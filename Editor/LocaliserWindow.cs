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
            window.minSize = new Vector2(480, 360);
            window.Show();
        }

        Languages defaultLanguage = LocalizationSystem.DefaultLanguage;

        string value;
        private Vector2 scroll;

        public void OnGUI()
        {
            SearchBar();

            ShowAll();
            GetSearchResults();

            ShowLanguageInCSV();
        }

        public void SearchBar()
        {
            EditorGUILayout.BeginHorizontal("Box");
            EditorGUILayout.LabelField("Search: ", EditorStyles.boldLabel);
            value = EditorGUILayout.TextField(value);

            if(LocalizationSystem.ActiveMode == LocalizationSystem.Mode.Offline)
            {
                GUIContent addContent = EditorGUIUtility.IconContent("CreateAddNew", "Add New Key");
                if (GUILayout.Button(addContent, GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
                {
                    string languageCode = LocalizationSystem.DefaultLanguage.GetStringValue();
                    TextLocaliserEditWindow.Open(value, language: languageCode);
                }
            }            
            EditorGUILayout.EndHorizontal();
        }

        public void ShowLanguageInCSV()
        {
            GUILayout.Label("Details", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal("Box");

            defaultLanguage = (Languages)EditorGUILayout.EnumPopup
                ("Editor Language :", defaultLanguage, GUILayout.MinWidth(250), GUILayout.MaxWidth(300));

            if (LocalizationSystem.DefaultLanguage != defaultLanguage)
            {
                LocalizationSystem.DefaultLanguage = defaultLanguage;
            }

            GUILayout.Space(80);

            EditorGUILayout.LabelField("Active Mode : " + LocalizationSystem.ActiveMode.ToString());
            GUILayout.EndHorizontal();
        }

        public void ShowAll()
        {
            GUILayout.Label("Dictionary Item", EditorStyles.boldLabel);
            if (value != null) { return; }

            EditorGUILayout.BeginVertical();
            scroll = EditorGUILayout.BeginScrollView(scroll);
            var dictionary = LocalizationSystem.GetDictionartForEditor();
            
            EditorGUILayout.BeginHorizontal("Box");

            if (LocalizationSystem.ActiveMode == LocalizationSystem.Mode.Offline)
                GUILayout.Space(50);
            else
                GUILayout.Space(10);

            EditorGUILayout.LabelField("Key");
            EditorGUILayout.LabelField("Value");

            EditorGUILayout.EndHorizontal();
            
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

            if (LocalizationSystem.ActiveMode == LocalizationSystem.Mode.Offline)
            {
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
                    string languageCode = LocalizationSystem.DefaultLanguage.GetStringValue();
                    TextLocaliserEditWindow.Open(element.Key, element.Value, languageCode);
                }
            }
            else
            {
                GUILayout.Space(5);
            }         

            EditorGUILayout.TextField(element.Key);
            GUILayout.Space(10);
            EditorGUILayout.LabelField(element.Value);
            EditorGUILayout.EndHorizontal();
        }
    }
}
