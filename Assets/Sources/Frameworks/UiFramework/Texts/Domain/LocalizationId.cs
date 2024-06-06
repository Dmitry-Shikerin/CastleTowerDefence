using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sources.Frameworks.UiFramework.Texts.Services.Localizations.Configs;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.Texts.Domain
{
    [Serializable]
    public class LocalizationId
    {
        [ValueDropdown("GetDropdownValues")] [HideLabel]
        [SerializeField] private string _localizationId;
        
        public string Id => _localizationId;
        
        // private IEnumerable<string> GetDropdownValues() =>
        //     LocalizationDataBase.Instance.LocalizationIds;
    }
}