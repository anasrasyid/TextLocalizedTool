using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace personaltools.textlocalizedtool
{
    public class LocalizationManager : MonoBehaviour
    {
        public static LocalizationManager Instance { get; private set; }

        public Languages currentLanguage;
        public TextAsset localizationCSV = LocalizationSystem.AssetCSV;

        public Action onChangeLanguage;

        private void OnValidate()
        {
            LocalizationSystem.AssetCSV = localizationCSV;
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


