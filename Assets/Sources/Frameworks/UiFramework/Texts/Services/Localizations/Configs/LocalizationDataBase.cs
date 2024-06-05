using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sources.Domain.Models.Constants;
using Sources.Frameworks.UiFramework.Core.Domain.Constants;
using Sources.Frameworks.UiFramework.Texts.Extensions;
using Sources.Frameworks.UiFramework.Texts.Services.Localizations.Phrases;
using UnityEditor;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.Texts.Services.Localizations.Configs
{
    public class LocalizationDataBase : ScriptableObject
    {
        [DisplayAsString(false)] [HideLabel] [SerializeField]
        private string _headere = UiConstant.UiLocalizationDataBaseLabel;

        [Space(10)] [SerializeField] private List<string> _localizationIds;
        [Space(10)] [SerializeField] private List<LocalizationPhrase> _phrases;

        private static LocalizationDataBase s_instance;

        public static LocalizationDataBase Instance
        {
            get
            {
                if (s_instance != null)
                    return s_instance;

                s_instance = Resources.Load<LocalizationDataBase>(LocalizationConst.LocalizationDataBaseAssetPath);

                if (s_instance != null)
                    return s_instance;

                s_instance = CreateInstance<LocalizationDataBase>();

#if UNITY_EDITOR
                AssetDatabase.CreateAsset(s_instance,
                    "Assets/Resources/Services/Localizations/ " + LocalizationConst.LocalizationDatabaseAsset);
#endif

                return s_instance;
            }
        }

        public List<string> LocalizationIds => _localizationIds;
        public List<LocalizationPhrase> Phrases => _phrases;

        public void RemovePhrase(LocalizationPhrase phrase)
        {
            AssetDatabase.RemoveObjectFromAsset(phrase);
            _phrases.Remove(phrase);
            AssetDatabase.SaveAssets();
        }

        public LocalizationPhrase CreatePhrase(string id)
        {
#if UNITY_EDITOR
            if (_localizationIds.Contains(id))
                return null;
            
            LocalizationPhrase phrase = CreateInstance<LocalizationPhrase>();
            phrase.SetParent(this);
            AssetDatabase.AddObjectToAsset(phrase, this);
            phrase.SetId(id);
            phrase.name = id;
            _phrases.Add(phrase);
            AssetDatabase.SaveAssets();

            return phrase;
#else
            return null;
#endif
        }

        [Button(ButtonSizes.Large)]
        [ResponsiveButtonGroup]
        public void AddAllPhrases()
        {
            _phrases.Clear();

            LocalizationExtension
                .FindAllLocalizationPhrases()
                .ForEach(phrase => _phrases.Add(phrase));

            FillIds();
        }

        private void FillIds()
        {
            _localizationIds.Clear();
            _phrases.ForEach(phrase => _localizationIds.Add(phrase.LocalizationId));
        }

        [OnInspectorGUI]
        private void ValidateIds()
        {
            if (_localizationIds.Count == _phrases.Count)
                return;

            AddAllPhrases();
            FillIds();
        }
    }
}