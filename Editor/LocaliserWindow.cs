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
        enum WindowState { Initialize, Load, Create}
        WindowState currentState = WindowState.Initialize;

        string value;
        private Vector2 scroll;
        string fileName = @"file 1";
        List<Languages> languages = new List<Languages>();

        public void OnGUI()
        {
            if(currentState == WindowState.Initialize)
            {
                GUILayout.Label("Localiser Menu", EditorStyles.boldLabel);
                bool isLoad = GUILayout.Button("Load", GUILayout.MinHeight(25));
                GUILayout.Space(5);
                bool isCreate = GUILayout.Button("Create", GUILayout.MinHeight(25));
                currentState = isLoad ? WindowState.Load : currentState;
                currentState = isCreate ? WindowState.Create : currentState;
            }

            if(currentState == WindowState.Load)
            {
                SearchBar();

                ShowAll();
                GetSearchResults();

                ShowLanguageInCSV();
                UtilityLocaliser();
            }

            if(currentState == WindowState.Create)
            {                
                fileName = EditorGUILayout.TextField("File Name",fileName);

                EditorGUILayout.LabelField("Languages", EditorStyles.boldLabel);
                scroll = EditorGUILayout.BeginScrollView(scroll);
                for (int i = 0; i < languages.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal("Box");

                    EditorGUILayout.LabelField($"Language - {i + 1}");
                    languages[i] = (Languages)EditorGUILayout.EnumPopup(languages[i]);

                    GUIContent deleteContent = EditorGUIUtility.IconContent("winbtn_win_close", "Remove");
                    if (GUILayout.Button(deleteContent, GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
                    {
                        languages.RemoveAt(i);
                    }
                    
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndScrollView();

                bool isAdd = GUILayout.Button("Add Language");
                if (isAdd)
                {
                    languages.Add(Languages.English);
                }
                
                bool isCreate = GUILayout.Button("Create File");
                if (isCreate)
                {
                    // Call CSV Loader
                    LocalizationSystem.CreateCSV(fileName, languages);

                    // Reset State
                    currentState = WindowState.Initialize;
                    languages.Clear();
                }
            }
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
                    string languageCode = LocalizationSystem.DefaultLanguage.GetStringValuesAtribute();
                    TextLocaliserEditWindow.Open(value, language: languageCode);
                }
            }            
            EditorGUILayout.EndHorizontal();
        }

        Languages addLanguage, removeLanguage;
        bool isUtility;

        public void UtilityLocaliser()
        {
            isUtility = EditorGUILayout.BeginToggleGroup("Utility", isUtility);
            {
                EditorGUILayout.BeginHorizontal("box");
                EditorGUILayout.LabelField("Add Language");
                addLanguage = (Languages)EditorGUILayout.EnumPopup(addLanguage);
                if (GUILayout.Button("Add", GUILayout.MaxWidth(60f), GUILayout.MaxHeight(20f)))
                {
                    LocalizationSystem.AddLanguage(addLanguage);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal("box");
                EditorGUILayout.LabelField("Remove Language");
                removeLanguage = ActiveLanguage("",removeLanguage, 100);
                if (GUILayout.Button("Remove", GUILayout.MaxWidth(60f), GUILayout.MaxHeight(20f)))
                {
                    LocalizationSystem.RemoveLanguage(removeLanguage);
                }
                EditorGUILayout.EndHorizontal();
            }            
            EditorGUILayout.EndToggleGroup();
        }

        public Languages ActiveLanguage(string label, Languages selected, int minWidth = 250)
        {
            var items = new string[LocalizationSystem.LanguageAvailable.Length];
            int index = 0;
            for (int i = 0; i < items.Length; i++)
            {

                items[i] = ((Languages)LocalizationSystem.LanguageAvailable[i]).ToString();
                if ((int)selected == LocalizationSystem.LanguageAvailable[i])
                    index = i;
            }

            index = EditorGUILayout.Popup(label, index, items,
                GUILayout.MinWidth(minWidth), GUILayout.MaxWidth(300));

            return items[index].GetLanguageEnum();
        }

        public void ShowLanguageInCSV()
        {
            GUILayout.Label("Details", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal("Box");

            defaultLanguage = ActiveLanguage("Editor Language :", defaultLanguage);

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
                if (GUILayout.Button(deleteContent, GUILayout.MinWidth(20), GUILayout.MinWidth(20)))
                {
                    if (EditorUtility.DisplayDialog("Remove Key " + element.Key + "?",
                        "This will remove the element from localisation, are sure?", "Do it"))
                    {
                        LocalizationSystem.Remove(element.Key);
                        AssetDatabase.Refresh();
                        LocalizationSystem.Init();
                    }
                }

                GUIContent editContent = EditorGUIUtility.IconContent("_Popup", "Edit");
                if (GUILayout.Button(editContent, GUILayout.MinWidth(20), GUILayout.MinWidth(20)))
                {
                    string languageCode = LocalizationSystem.DefaultLanguage.GetStringValuesAtribute();
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
