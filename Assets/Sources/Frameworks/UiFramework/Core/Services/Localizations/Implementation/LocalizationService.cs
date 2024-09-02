using System;
using System.Collections.Generic;
using System.Linq;
using Agava.WebUtility;
using Agava.YandexGames;
using Sources.Domain.Models.Constants;
using Sources.Frameworks.UiFramework.Core.Services.Localizations.Interfaces;
using Sources.Frameworks.UiFramework.Texts.Presentations.Views.Implementation;
using Sources.Frameworks.UiFramework.Texts.Presentations.Views.Interfaces;
using Sources.Frameworks.UiFramework.Texts.Services.Localizations.Configs;
using Sources.Frameworks.UiFramework.Views.Presentations.Implementation;
using Sources.Frameworks.UiFramework.Views.Presentations.Implementation.Types;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.Core.Services.Localizations.Implementation
{
    public class LocalizationService : ILocalizationService
    {
        private readonly UiCollector _uiCollector;
        private readonly List<IUiLocalizationText> _textViews = new List<IUiLocalizationText>();
        private readonly Dictionary<string, IReadOnlyDictionary<string, string>> _textDictionary;
        private IReadOnlyDictionary<string, string> _currentLanguageDictionary;
        private readonly Dictionary<string, IReadOnlyDictionary<string, Sprite>> _spriteDictionary;
        private IReadOnlyDictionary<string, Sprite> _currentLanguageSprites;

        public LocalizationService(UiCollector uiCollector, LocalizationDataBase localizationDataBase)
        {
            _uiCollector = uiCollector ? uiCollector : throw new ArgumentNullException(nameof(uiCollector));

            AddTextViews(uiCollector);

            _textDictionary = new Dictionary<string, IReadOnlyDictionary<string, string>>()
            {
                [LocalizationConst.RussianCode] = localizationDataBase.Phrases
                    .ToDictionary(phrase => phrase.LocalizationId, phrase => phrase.Russian),
                [LocalizationConst.EnglishCode] = localizationDataBase.Phrases
                    .ToDictionary(phrase => phrase.LocalizationId, phrase => phrase.English),
                [LocalizationConst.TurkishCode] = localizationDataBase.Phrases
                    .ToDictionary(phrase => phrase.LocalizationId, phrase => phrase.Turkish),
            };
            _spriteDictionary = new Dictionary<string, IReadOnlyDictionary<string, Sprite>>()
            {
                [LocalizationConst.RussianCode] = localizationDataBase.Phrases
                    .ToDictionary(phrase => phrase.LocalizationId, phrase => phrase.RussianSprite),
                [LocalizationConst.EnglishCode] = localizationDataBase.Phrases
                    .ToDictionary(phrase => phrase.LocalizationId, phrase => phrase.EnglishSprite),
                [LocalizationConst.TurkishCode] = localizationDataBase.Phrases
                    .ToDictionary(phrase => phrase.LocalizationId, phrase => phrase.TurkishSprite),
            };
        }
        
        public void Translate()
        {
            //todo вынести в отдельный сервис
            if(WebApplication.IsRunningOnWebGL)
                ChangeSdcLanguage();
            else
                ChangeCollectorLanguage();
        }

        public string GetText(string key)
        {
            if(_currentLanguageDictionary.ContainsKey(key) == false)
                throw new KeyNotFoundException(nameof(key));
            
            return _currentLanguageDictionary[key];
        }

        public Sprite GetSprite(string key)
        {
            if(_currentLanguageSprites.ContainsKey(key) == false)
                throw new KeyNotFoundException(nameof(key));
            
            return _currentLanguageSprites[key];
        }

        private void TranslateViews(string key)
        {
            _currentLanguageDictionary = _textDictionary[key];

            foreach (IUiLocalizationText textView in _textViews)
            {
                if (string.IsNullOrWhiteSpace(textView.Id))
                {
                    if (textView is not MonoBehaviour concrete)
                        throw new NullReferenceException();
                    
                    Debug.Log($"LocalizationService: {concrete.gameObject.name} has empty id");
                }

                if (_currentLanguageDictionary.ContainsKey(textView.Id) == false)
                {
                    Debug.Log($"LocalizationService: {textView.Id} not found in LocalizationData");
                    
                    continue;
                }

                textView.SetText(_currentLanguageDictionary[textView.Id]);
            }
        }

        private void TranslateSprites(string key)
        {
            _currentLanguageSprites = _spriteDictionary[key];
            
            foreach (UiLocalizationSprite localizationSprite in _uiCollector.UiLocalizationSprites)
            {
                if (string.IsNullOrWhiteSpace(localizationSprite.Id))
                {
                    Debug.Log($"LocalizationService: {localizationSprite.gameObject.name} has empty id");
                }

                if (_currentLanguageDictionary.ContainsKey(localizationSprite.Id) == false)
                {
                    Debug.Log($"LocalizationService: {localizationSprite.Id} not found in LocalizationData");
                    
                    continue;
                }
                
                localizationSprite.SetSprite(_currentLanguageSprites[localizationSprite.Id]);
            }
        }

        private void AddTextViews(UiCollector uiCollector)
        {
            foreach (IUiLocalizationText textView in uiCollector.UiTexts)
                _textViews.Add(textView);
        }

        private void ChangeCollectorLanguage()
        {
            string key = _uiCollector.Localization switch
            {
                Localization.English => LocalizationConst.EnglishCode,
                Localization.Turkish => LocalizationConst.TurkishCode,
                Localization.Russian => LocalizationConst.RussianCode,
                _ => LocalizationConst.EnglishCode,
            };

            TranslateViews(key);
            TranslateSprites(key);
        }
        
        private void ChangeSdcLanguage()
        {
            if (WebApplication.IsRunningOnWebGL == false)
                return;

            string languageCode = YandexGamesSdk.Environment.i18n.lang switch
            {
                LocalizationConst.English => LocalizationConst.EnglishCode,
                LocalizationConst.Turkish => LocalizationConst.TurkishCode,
                LocalizationConst.Russian => LocalizationConst.RussianCode,
                _ => LocalizationConst.EnglishCode
            };

            TranslateViews(languageCode);
        }
    }
}