using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sources.Domain.Models.Constants;
using Sources.Frameworks.UiFramework.Core.Domain.Constants;
using Sources.Frameworks.UiFramework.Core.Presentation.CommonTypes;
using Sources.Frameworks.UiFramework.Texts.Services.Localizations.Phrases;
using UnityEditor;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.Texts.Services.Localizations.Configs
{
    public class LocalizationDataBase : ScriptableObject
    {
        [DisplayAsString(false)] [HideLabel] [SerializeField]
        private string _headere = UiConstant.UiLocalizationDataBaseLabel;

        [TabGroup("GetId", "DataBase")] [Space(10)] 
        [SerializeField] private List<LocalizationPhrase> _phrases;
        [TabGroup("GetId", "DataBase")] [Space(10)] 
        [SerializeField] private List<LocalizationScope> _scopes;
        
        [TabGroup("GetId", "CreatePhrase")] 
        [EnumToggleButtons] [Space(10)] [LabelText("TextId")]
        [SerializeField] private Enable _enableTextId;
        [TabGroup("GetId", "CreatePhrase")]
        [HideLabel] [ValidateInput("ValidateTextId", "TextId contains in DataBase")]
        [EnableIf("_enableTextId", Enable.Enable)]
        [SerializeField] private string _textId;
        [Space(7)]
        [TabGroup("GetId", "CreatePhrase")] 
        [EnumToggleButtons] [Space(10)] [LabelText("Russian")]
        [SerializeField] private Enable _enableRussian;
        [TabGroup("GetId", "CreatePhrase")]
        [TextArea(1, 20)] [HideLabel] 
        [EnableIf("_enableRussian", Enable.Enable)]
        [SerializeField] private string _russian;
        [TabGroup("GetId", "CreatePhrase")] 
        [EnumToggleButtons] [Space(10)] [LabelText("English")]
        [SerializeField] private Enable _enableEnglish;
        [TabGroup("GetId", "CreatePhrase")]
        [TextArea(1, 20)] [HideLabel]
        [EnableIf("_enableEnglish", Enable.Enable)]
        [SerializeField] private string _english;
        [TabGroup("GetId", "CreatePhrase")] 
        [EnumToggleButtons] [Space(10)] [LabelText("Turkish")]
        [SerializeField] private Enable _enableTurkish;
        [TabGroup("GetId", "CreatePhrase")]
        [TextArea(1, 20)] [HideLabel]
        [EnableIf("_enableTurkish", Enable.Enable)]
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

        public List<LocalizationPhrase> Phrases => _phrases;
        
        public void RemovePhrase(LocalizationPhrase phrase)
        {
#if UNITY_EDITOR
            AssetDatabase.RemoveObjectFromAsset(phrase);
            _phrases.Remove(phrase);
            AssetDatabase.SaveAssets();
#endif
        }

        public void RemoveScope(LocalizationScope localizationScope)
        {
#if UNITY_EDITOR
            AssetDatabase.RemoveObjectFromAsset(localizationScope);
            _scopes.Remove(localizationScope);
            AssetDatabase.SaveAssets();
#endif
        }

        public LocalizationPhrase GetPhrase(string textId) =>
            _phrases.First(phrase => phrase.LocalizationId == textId);


        [TabGroup("GetId", "CreatePhrase")]
        [Button(ButtonSizes.Large)]
        private void CreatePhrase()
        {
#if UNITY_EDITOR
            if (_phrases.Any(phrase => phrase.LocalizationId == _textId))
                return;
            
            LocalizationPhrase phrase = CreateInstance<LocalizationPhrase>();
            
            AssetDatabase.AddObjectToAsset(phrase, this);
            AssetDatabase.Refresh();
            
            _phrases.Add(phrase);
            phrase.SetDataBase(this);
            phrase.SetId(_textId);
            phrase.name = _textId + "_Phrase";

            phrase.SetRussian(_russian);
            phrase.SetEnglish(_english);
            phrase.SetTurkish(_turkish);
            
            AssetDatabase.SaveAssets();
#endif
        }

        private List<string> GetScopes()
        {
            if (_scopes == null || _scopes.Count == 0)
                return new List<string>() { "Default", };
            
            return _scopes.Select(scope => scope.Id).ToList();
        }

        [UsedImplicitly]
        private bool ValidateTextId(string textId) =>
            _phrases.Any(phrase => phrase.LocalizationId == textId) == false;
    }
}