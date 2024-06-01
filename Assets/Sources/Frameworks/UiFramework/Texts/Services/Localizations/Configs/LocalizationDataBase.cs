using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sources.Domain.Models.Constants;
using Sources.Frameworks.UiFramework.Domain.Dictionaries;
using Sources.Frameworks.UiFramework.Texts.Extensions;
using Sources.Frameworks.UiFramework.Texts.Services.Localizations.Phrases;
using UnityEditor;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.Texts.Services.Localizations.Configs
{
    [CreateAssetMenu(
        fileName = "LocalizationDataBase",
        menuName = "Configs/LocalizationDataBase",
        order = 51)]
    public class LocalizationDataBase : ScriptableObject
    {
        [SerializeField] private List<string> _localizationIds;
        [SerializeField] private List<LocalizationPhrase> _localizationPhrases;
        [SerializeField] private StringPhraseSerializedDictionary _localizationPhrase;

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
        public List<LocalizationPhrase> LocalizationPhrases => _localizationPhrases;

        [Button]
        public void AddAllPhrases()
        {
            _localizationPhrases.Clear();

            LocalizationExtension
                .FindAllLocalizationPhrases()
                .ForEach(phrase => _localizationPhrases.Add(phrase));
            
            FillIds();
        }

        [Button]
        private void FillIds()
        {
            _localizationIds.Clear();
            _localizationPhrases.ForEach(phrase => _localizationIds.Add(phrase.LocalizationId));
        }

        [Button]
        public void CreateLocalizationPhrase() =>
            LocalizationExtension.CreateLocalizationPhrase();

        [OnInspectorGUI]
        private void ValidateIds()
        {
            if (_localizationIds.Count != _localizationPhrase.Count)
                FillIds();
        }
    }
}