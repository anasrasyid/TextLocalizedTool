using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

namespace personaltools.textlocalizedtool
{
    [Serializable]
    public class LocalizationSystem
    {
        [SerializeField]
        private static Languages language = Languages.English;
        public static Languages Language
        {
            get => language;
            set
            {
                language = value;
                UpdateDictionaries();
            }
        }

        [SerializeField]
        private static Languages defaultLanguage = Languages.English;
        public static Languages DefaultLanguage
        {
            get => defaultLanguage;
            set
            {
                defaultLanguage = value;
                UpdateDictionaries();
            }
        }

        private static Dictionary<string, string> defaultlocalised;
        private static Dictionary<string, string> localised;

        private static bool isInit;
        private static int[] languagesIndex;
        public static int[] LanguageAvailable { get { return languagesIndex; } }

        public enum Mode { Online, Offline};
        public static Mode ActiveMode;
        public static Action<int[]> onChangeCSV;

        public static CSVLoader csvLoader;
        private static TextAsset assetCSV;
        
        public static TextAsset AssetCSV { 
            get 
            {
                return assetCSV;
            } 
            set 
            {
                if(value != null)
                {
                    assetCSV = value;
                    if (ActiveMode == Mode.Offline)
                        Init();
                }
            } 
        }

        private static string csvURL;

        public static string CSVURL
        {
            get
            {
                return csvURL;
            }
            set
            {
                if(value != null)
                {
                    csvURL = value;
                    if(ActiveMode == Mode.Online)
                    {
                        if (Application.isEditor)
                            DownloadFileAsnyc(csvURL);
                        else
                            Init();
                    }
                }                
            }
        }

        private static async void DownloadFileAsnyc(string url)
        {
            if (url == null || url == string.Empty)
                return;

            string result = await Task.Run(() =>
            {
                string rawFile = string.Empty;
                using (WebClient client = new WebClient())
                {
                    rawFile = client.DownloadString(csvURL);
                    Debug.Assert(rawFile != string.Empty, "Error Localization URL");
                }
                return rawFile;
            });

            assetCSV = new TextAsset(result);
            Init();
        }

        private static void DownloadFile(string url)
        {
            string rawFile = string.Empty;
            using (WebClient client = new WebClient())
            {
                rawFile = client.DownloadString(url);
                Debug.Assert(rawFile != string.Empty, "Error Localization URL");
            }
            assetCSV = new TextAsset(rawFile);
        }

        public static void Init()
        {
            isInit = true;
            if (Application.isPlaying && ActiveMode == Mode.Online)
                DownloadFile(CSVURL);

            Debug.Assert(AssetCSV != null, "Please Insert Localised File in Localization Manager");

            csvLoader = csvLoader ?? new CSVLoader();
            csvLoader.LoadCSV(AssetCSV, out languagesIndex);
            onChangeCSV?.Invoke(languagesIndex);

            UpdateDictionaries();
        }

        public static List<string> GetLanguageNames()
        {
            List<string> languagesNames = new List<string>();

            foreach (string language in Enum.GetNames(typeof(Languages)))
            {
                languagesNames.Add(language);
            }
            return languagesNames;
        }

        public static Languages GetEnumLanguage(string name)
        {
            foreach (Languages lan in Enum.GetValues(typeof(Languages)))
            {
                if (lan.ToString() == name)
                    return lan;
            }
            return Languages.English;
        }

        public static void UpdateDictionaries()
        {
            if (assetCSV == null)
                return;

            string defaultCode = DefaultLanguage.GetStringValuesAtribute();
            defaultlocalised = csvLoader.GetDictionary(defaultCode);

            string currentCode = Language.GetStringValuesAtribute();
            localised = csvLoader.GetDictionary(currentCode);
        }

        public static Dictionary<string, string> GetDictionartForEditor()
        {
            if (!isInit) { Init(); }
            return defaultlocalised;
        }

        public static string GetLocalisedValue(string key)
        {
            if (!isInit) { Init(); }
            var value = string.Empty;

            localised.TryGetValue(key, out value);

            return value;
        }

#if UNITY_EDITOR
        public static void CreateCSV(string name, List<Languages> languages)
        {
            HashSet<string> codes = new HashSet<string>();
            foreach (var language in languages)
                codes.Add(language.GetStringValuesAtribute());

            string path = Application.dataPath + @"/";
            CSVLoader.CreateCSV(path + name +".csv", codes.ToArray());
        }

        public static void AddLanguage(Languages language)
        {
            if (languagesIndex.Contains((int)language))
                return;

            csvLoader = csvLoader ?? new CSVLoader();
            csvLoader.LoadCSV(AssetCSV, out languagesIndex);
            csvLoader.AddLanguage(language.GetStringValuesAtribute());
            csvLoader.LoadCSV(AssetCSV, out languagesIndex);
            UpdateDictionaries();
        }

        public static void RemoveLanguage(Languages language)
        {
            if (!languagesIndex.Contains((int)language))
                return;

            csvLoader = csvLoader ?? new CSVLoader();
            csvLoader.LoadCSV(AssetCSV, out languagesIndex);
            csvLoader.RemoveLanguage(language.GetStringValuesAtribute());
            csvLoader.LoadCSV(AssetCSV, out languagesIndex);
            UpdateDictionaries();
        }

        public static void Add(string key, string value, string language)
        {

            if (value != null && value.Contains("\""))
            {
                value.Replace('"', '\"');
            }

            csvLoader = csvLoader ?? new CSVLoader();

            csvLoader.LoadCSV(AssetCSV, out languagesIndex);
            csvLoader.Add(key, value, language);
            csvLoader.LoadCSV(AssetCSV, out languagesIndex);

            UpdateDictionaries();
        }

        public static void Replace(string key, string value, string language)
        {
            if (value != null && value.Contains("\""))
            {
                value.Replace('"', '\"');
            }

            csvLoader = csvLoader ?? new CSVLoader();

            csvLoader.LoadCSV(AssetCSV, out languagesIndex);
            csvLoader.Edit(key, value, language);
            csvLoader.LoadCSV(AssetCSV, out languagesIndex);

            UpdateDictionaries();
        }

        public static void Remove(string key)
        {
            csvLoader = csvLoader ?? new CSVLoader();

            csvLoader.LoadCSV(AssetCSV, out languagesIndex);
            csvLoader.Remove(key);
            csvLoader.LoadCSV(AssetCSV, out languagesIndex);

            UpdateDictionaries();
        }
#endif
    }
}