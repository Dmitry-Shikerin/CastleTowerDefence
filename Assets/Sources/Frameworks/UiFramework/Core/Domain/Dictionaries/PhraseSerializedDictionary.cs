using System;
using Sirenix.OdinInspector;
using Sources.Frameworks.UiFramework.Texts.Domain;
using Sources.Frameworks.Utils.Dictionaries;

namespace Sources.Frameworks.UiFramework.Core.Domain.Dictionaries
{
    [Serializable] [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine)]
    public class PhraseSerializedDictionary : SerializedDictionary<LocalizationId, LocalizationPhraseClass>
    {
    }
}