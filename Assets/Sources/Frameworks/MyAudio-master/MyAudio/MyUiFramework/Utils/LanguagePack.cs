using System;
using MyAudios.MyUiFramework.Enums;
using UnityEngine;

namespace MyAudios.MyUiFramework.Utils
{
    [Serializable]
    public class LanguagePack : ScriptableObject
    {
        private const string CURRENT_LANGUAGE_PREFS_KEY = "Doozy.CurrentLanguage";
        public const Language DEFAULT_LANGUAGE = Language.English;

        private static Language s_currentLanguage = Language.Unknown;

        public static Language CurrentLanguage
        {
            get
            {
                if (s_currentLanguage != Language.Unknown)
                    return s_currentLanguage;
                
                CurrentLanguage = (Language)
                    PlayerPrefs.GetInt(CURRENT_LANGUAGE_PREFS_KEY, (int) DEFAULT_LANGUAGE);
                
                return s_currentLanguage;
            }
            set
            {
                SaveLanguagePreference(value);
                s_currentLanguage = value;
            }
        }

        private static void SaveLanguagePreference(Language language) =>
            SaveLanguagePreference(CURRENT_LANGUAGE_PREFS_KEY, language);

        private static void SaveLanguagePreference(string prefsKey, Language language)
        {
            PlayerPrefs.SetInt(prefsKey, (int) language);
            PlayerPrefs.Save();
        }
    }
}