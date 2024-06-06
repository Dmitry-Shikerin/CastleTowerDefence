using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sources.Frameworks.UiFramework.Texts.Services.Localizations.Phrases;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.Texts.Services.Localizations.Configs
{
    public class LocalizationScope : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private LocalizationDataBase _parent;
        [SerializeField] private List<LocalizationPhrase> _phrases;
        
        public IReadOnlyList<LocalizationPhrase> Phrases => _phrases;
        public string Id => _id;
        
        public void SetParent(LocalizationDataBase parent) => 
            _parent = parent ?? throw new NullReferenceException(nameof(parent));

        public void SetId(string id) =>
            _id = id;

        [Button]
        private void Remove()
        {
            _parent.RemoveScope(this);
        }
    }
}