using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sources.Domain.Models.Constants;
using Sources.Frameworks.UiFramework.Core.Domain.Constants;
using Sources.Frameworks.UiFramework.Core.Presentation.CommonTypes;
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

        [TabGroup("GetId", "Scopes")] 
        [SerializeField] private string _scopeId;
        
        [TabGroup("GetId", "DataBase")] [Space(10)] 
        [SerializeField] private List<string> _localizationIds;
        [TabGroup("GetId", "DataBase")] [Space(10)] 
        [SerializeField] private List<LocalizationPhrase> _phrases;
        [TabGroup("GetId", "DataBase")] [Space(10)] 
        [SerializeField] private List<LocalizationScope> _scopes;
        [TabGroup("GetId", "DataBase")] [Space(10)] 
        [SerializeField] private List<string> _scopeIds;
        
        [TabGroup("GetId", "CreatePhrase")] 
        [EnumToggleButtons] [Space(10)] [LabelText("TextId")]
        [SerializeField] private Enable _enableTextId;
        [TabGroup("GetId", "CreatePhrase")]
        [HideLabel] [ValidateInput("ValidateTextId", "TextId contains in DataBase")]
        [EnableIf("_enableTextId", Core.Presentation.CommonTypes.Enable.Enable)]
        [SerializeField] private string _textId;
        [TabGroup("GetId", "CreatePhrase")] 
        [EnumToggleButtons] [Space(10)] [LabelText("Russian")]
        [SerializeField] private Enable _enableRussian;
        [TabGroup("GetId", "CreatePhrase")]
        [TextArea(1, 20)] [HideLabel] 
        [EnableIf("_enableRussian", Core.Presentation.CommonTypes.Enable.Enable)]
        [SerializeField] private string _russian;
        [TabGroup("GetId", "CreatePhrase")] 
        [EnumToggleButtons] [Space(10)] [LabelText("English")]
        [SerializeField] private Enable _enableEnglish;
        [TabGroup("GetId", "CreatePhrase")]
        [TextArea(1, 20)] [HideLabel]
        [EnableIf("_enableEnglish", Core.Presentation.CommonTypes.Enable.Enable)]
        [SerializeField] private string _english;
        [TabGroup("GetId", "CreatePhrase")] 
        [EnumToggleButtons] [Space(10)] [LabelText("Turkish")]
        [SerializeField] private Enable _enableTurkish;
        [TabGroup("GetId", "CreatePhrase")]
        [TextArea(1, 20)] [HideLabel]
        [EnableIf("_enableTurkish", Core.Presentation.CommonTypes.Enable.Enable)]
        [SerializeField] private string _turkish;

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

        [TabGroup("GetId", "Scopes")]
        [Button(ButtonSizes.Large)]
        public void CreateScope()
        {
#if UNITY_EDITOR
            if (_scopeIds.Contains(_scopeId))
                return;
            
            LocalizationScope scope = CreateInstance<LocalizationScope>();
            scope.SetParent(this);
            AssetDatabase.AddObjectToAsset(scope, this);
            scope.SetId(_scopeId);
            scope.name = _scopeId + "_Scope";
            _scopeIds.Add(_scopeId);
            _scopes.Add(scope);
            AssetDatabase.SaveAssets();
#else
            return null;
#endif
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

        [TabGroup("GetId", "DataBase")]
        [Button(ButtonSizes.Large)]
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

        public void RemoveScope(LocalizationScope localizationScope)
        {
            AssetDatabase.RemoveObjectFromAsset(localizationScope);
            _scopes.Remove(localizationScope);
            _scopeIds.Remove(localizationScope.Id);
            AssetDatabase.SaveAssets();
        }
        
        [TabGroup("GetId", "CreatePhrase")]
        [Button(ButtonSizes.Large)]
        private void CreatePhrase()
        {
            LocalizationPhrase phrase = LocalizationDataBase.Instance.CreatePhrase(_textId);

            if (phrase == null)
                return;
             
            phrase.SetRussian(_russian);
            phrase.SetEnglish(_english);
            phrase.SetTurkish(_turkish);
        }
        
        [UsedImplicitly]
        private bool ValidateTextId(string textId) =>
            LocalizationDataBase.Instance.LocalizationIds.Contains(textId) == false;
    }
}