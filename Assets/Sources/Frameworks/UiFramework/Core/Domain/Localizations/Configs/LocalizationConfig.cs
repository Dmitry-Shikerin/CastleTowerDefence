using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sources.Frameworks.UiFramework.Domain.Dictionaries;
using Sources.Frameworks.UiFramework.Domain.Localizations.Phrases;
using Sources.Frameworks.UiFramework.Extensions;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.Domain.Localizations.Configs
{
    [CreateAssetMenu(
        fileName = "LocalizationConfigSecond", 
        menuName = "Configs/LocalizationConfigSecond", 
        order = 51)]
    public class LocalizationConfig : ScriptableObject
    {
        [SerializeField] private List<string> _localizationIds;
        [SerializeField] private List<LocalizationPhrase> _localizationPhrases;
        [ValueDropdown("GetDropdownValues")]
        [SerializeField] private StringPhraseSerializedDictionary _localizationPhrase;
        
        public List<string> LocalizationIds => _localizationIds;
        public List<LocalizationPhrase> LocalizationPhrases => _localizationPhrases;
        
        [Button]
        private void AddAllPhrases()
        {
            _localizationPhrases.Clear();
            
                LocalizationExtension
                    .FindAllLocalizationPhrases()
                    .ForEach(phrase => _localizationPhrases.Add(phrase));
        }
        
        [Button]
        private void FillIds()
        {
            _localizationIds.Clear();
            _localizationPhrases.ForEach(phrase => _localizationIds.Add(phrase.LocalizationId));
        }
        
        [Button]
        private void CreateLocalizationPhrase() =>
            LocalizationExtension.CreateLocalizationPhrase();

        [UsedImplicitly]
        private List<string> GetDropdownValues() =>
            LocalizationExtension.GetTranslateId();
    }
}