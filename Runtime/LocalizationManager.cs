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
        [SerializeField]
        private Languages currentLanguage;
        
        public Action onChangeLanguage;

        [SerializeField]
        private LocalizationSystem.Mode currentMode;

        [SerializeField]
        private TextAsset assetCSV;

        [SerializeField]
        private string csvurl;

        private void OnValidate()
        {
            if(currentMode != LocalizationSystem.ActiveMode)
            {
                ChangeMode(currentMode, csvurl, assetCSV);
            }
            else
            {
                if (assetCSV != LocalizationSystem.AssetCSV)
                {
                    ChangeMode(currentMode, assetCSV: assetCSV);
                }
                    
                if (csvurl != LocalizationSystem.CSVURL)
                {
                    ChangeMode(currentMode, csvurl: csvurl);
                }
                    
            }                        
            
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

        public void ChangeMode(LocalizationSystem.Mode mode, string csvurl = null, TextAsset assetCSV = null)
        {
            LocalizationSystem.onChangeCSV = (int[] param) => { LanguageAvailable = param; };
            LocalizationSystem.ActiveMode = mode;

            LocalizationSystem.AssetCSV = assetCSV;
            LocalizationSystem.CSVURL = csvurl;
            ChangeLanguage(currentLanguage);
        }

        public void ChangeLanguage(Languages currentLanguage)
        {
            this.currentLanguage = currentLanguage;
            LocalizationSystem.Language = currentLanguage;
            onChangeLanguage?.Invoke();
        }

    }
}


