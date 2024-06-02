using System;
using Sirenix.OdinInspector;
using Sources.Frameworks.UiFramework.Core.Presentation.CommonTypes;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.Texts.Domain
{
    [Serializable]
    public class LocalizationPhraseClass
    {
        [FoldoutGroup("Value")]
        [FoldoutGroup("Value/Russian")] [EnumToggleButtons]
        [SerializeField] private Enable _editRussian;
        [FoldoutGroup("Value/Russian")][TextArea(1, 20)] 
        [EnableIf("_editRussian", Enable.Enable)]
        [SerializeField] private string _russian;
        
        [FoldoutGroup("Value/English")] [EnumToggleButtons]
        [SerializeField] private Enable _editEnglish;
        [EnableIf("_editEnglish", Enable.Enable)]
        [FoldoutGroup("Value/English")] [TextArea(1, 20)]
        [SerializeField] private string _english;
        
        [FoldoutGroup("Value/Turkish")] [EnumToggleButtons]
        [SerializeField] private Enable _editTurkish;
        [EnableIf("_editTurkish", Enable.Enable)]
        [FoldoutGroup("Value/Turkish")][TextArea(1, 20)]
        [SerializeField] private string _turkish;

        public string Russian => _russian;
        public string English => _english;
        public string Turkish => _turkish;
        
    }
}