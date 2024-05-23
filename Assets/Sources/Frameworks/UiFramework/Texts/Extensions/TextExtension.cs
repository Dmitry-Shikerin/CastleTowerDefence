using System.Collections.Generic;
using System.Linq;
using Sources.Frameworks.UiFramework.Domain.Localizations.Configs;
using Sources.Frameworks.UiFramework.Domain.Localizations.Phrases;
using UnityEditor;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.Extensions
{
    public static class TextExtension
    {
        public static List<string> GetTranslateId()
        {
#if UNITY_EDITOR
            
            return AssetDatabase
                .FindAssets("t:LocalizationConfig")
                .Select(path => AssetDatabase.GUIDToAssetPath(path))
                .Select(path => AssetDatabase.LoadAssetAtPath<LocalizationConfig>(path))
                .ToList()
                .FirstOrDefault()
                .LocalizationIds;
#endif
            return new List<string>();
        }

        public static List<LocalizationPhrase> FindAllLocalizationPhrases()
        {
#if UNITY_EDITOR
            return AssetDatabase
                .FindAssets("t:LocalizationPhrase")
                .Select(path => AssetDatabase.GUIDToAssetPath(path))
                .Select(path => AssetDatabase.LoadAssetAtPath<LocalizationPhrase>(path))
                .ToList();
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
    }
}