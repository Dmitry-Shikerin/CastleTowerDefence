﻿using System.Collections.Generic;
using System.Linq;
using Sources.Frameworks.UiFramework.Domain.Localizations.Configs;
using Sources.Frameworks.UiFramework.Domain.Localizations.Phrases;
using UnityEditor;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.Texts.Extensions
{
    public static class LocalizationExtension
    {
        public static List<string> GetTranslateId()
        {
#if UNITY_EDITOR
            return FindAssets<LocalizationConfig>("t:LocalizationConfig")
                .FirstOrDefault()
                .LocalizationIds;
#endif
            return new List<string>();
        }

        public static List<LocalizationPhrase> FindAllLocalizationPhrases()
        {
#if UNITY_EDITOR
            return FindAssets<LocalizationPhrase>("t:LocalizationPhrase");
#endif
            return new List<LocalizationPhrase>();
        }

        public static void CreateLocalizationPhrase()
        {
#if UNITY_EDITOR
            LocalizationPhrase phrase = ScriptableObject.CreateInstance<LocalizationPhrase>();
            
            AssetDatabase.CreateAsset(phrase, 
                "Assets/Resources/Configs/Localizations/LocalizationPhrase.asset");
            AssetDatabase.SaveAssets();
#endif
        }

        public static void RenameAsset(Object asset, string newName)
        {
#if UNITY_EDITOR
            string path = AssetDatabase.GetAssetPath(asset);
            AssetDatabase.RenameAsset(path, newName);
#endif
        }

        private static List<T> FindAssets<T>(string assetName) where T : Object
        {
            return AssetDatabase
                .FindAssets(assetName)
                .Select(path => AssetDatabase.GUIDToAssetPath(path))
                .Select(path => AssetDatabase.LoadAssetAtPath<T>(path))
                .ToList();
        }
    }
}