using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace personaltools.textlocalizedtool
{
    public class LocalizationManager : MonoBehaviour
    {
        public static LocalizationManager Instance { get; private set; }
        public static int[] avaliable;

        public static int[] LanguageAvailable = new int[] { };
        [EnumPartial(typeof(LocalizationManager), "LanguageAvailable")]
        public Languages currentLanguage;
        public Action onChangeLanguage;

        public LocalizationSystem.Mode currentMode;
        public TextAsset AssetCSV;
        public string CSVURL;

        private void OnValidate()
        {
            LocalizationSystem.ActiveMode = currentMode;
            LocalizationSystem.AssetCSV = AssetCSV;
            LocalizationSystem.CSVURL = CSVURL;
            LanguageAvailable = LocalizationSystem.LanguageAvailable;

            Debug.Log(currentLanguage);
            ChangeLanguage(currentLanguage);
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            currentLanguage = LocalizationSystem.Language;
        }

        public void ChangeLanguage(Languages currentLanguage)
        {
            this.currentLanguage = currentLanguage;
            LocalizationSystem.Language = currentLanguage;
            onChangeLanguage?.Invoke();
        }

    }
}


