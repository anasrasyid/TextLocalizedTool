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
        [EnumPartialAtribute(typeof(LocalizationManager), "LanguageAvailable")]
        public Languages currentLanguage;
        public Action onChangeLanguage;

        public LocalizationSystem.Mode currentMode;
        public TextAsset AssetCSV;
        public string CSVURL;

        private void OnValidate()
        {
            ChangeMode(currentMode);
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

        public void ChangeMode(LocalizationSystem.Mode mode)
        {
            LocalizationSystem.onChangeCSV = (int[] param) => { LanguageAvailable = param; };
            LocalizationSystem.ActiveMode = mode;
            LocalizationSystem.AssetCSV = AssetCSV;
            LocalizationSystem.CSVURL = CSVURL;
        }

        public void ChangeLanguage(Languages currentLanguage)
        {
            this.currentLanguage = currentLanguage;
            LocalizationSystem.Language = currentLanguage;
            onChangeLanguage?.Invoke();
        }

    }
}


