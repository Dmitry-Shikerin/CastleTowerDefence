using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sources.Domain.Models.Constants;
using Sources.Frameworks.UiFramework.Texts.Services.Localizations.Configs;
using Sources.Frameworks.UiFramework.Texts.Services.Localizations.Phrases;
using UnityEditor;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.Texts.Extensions
{
    public static class LocalizationExtension
    {
        public static List<string> GetTranslateId()
        {
#if UNITY_EDITOR
            return FindAssets<LocalizationDataBase>(LocalizationConst.LocalizationDatabaseAsset)
                .FirstOrDefault()
                ?.Phrases.Select(phrase => phrase.LocalizationId).ToList();
#else
            return new List<string>();
#endif
        }

        public static List<LocalizationPhrase> FindAllLocalizationPhrases()
        {
#if UNITY_EDITOR
            return FindAssets<LocalizationPhrase>(LocalizationConst.LocalizationPhraseAsset);
#else
            return new List<LocalizationPhrase>();
#endif
        }

        public static LocalizationPhrase CreateLocalizationPhrase(string name)
        {
#if UNITY_EDITOR
            LocalizationPhrase phrase = ScriptableObject.CreateInstance<LocalizationPhrase>();

            AssetDatabase.CreateAsset(phrase, LocalizationConst.LocalisationPhraseAssetPath);
            RenameAsset(phrase, name);
            AssetDatabase.SaveAssets();
            return phrase;
#else
            return null;
#endif
        }

        public static void RenameAsset(Object asset, string newName)
        {
#if UNITY_EDITOR
            string path = AssetDatabase.GetAssetPath(asset);
            AssetDatabase.RenameAsset(path, newName);
#endif
        }

        public static List<T> FindAssets<T>(string assetName) where T : Object
        {
#if UNITY_EDITOR
            return AssetDatabase
                .FindAssets(assetName)
                .Select(path => AssetDatabase.GUIDToAssetPath(path))
                .Select(path => AssetDatabase.LoadAssetAtPath<T>(path))
                .ToList();
#else
            return new List<T>();
#endif
        }
    }
}