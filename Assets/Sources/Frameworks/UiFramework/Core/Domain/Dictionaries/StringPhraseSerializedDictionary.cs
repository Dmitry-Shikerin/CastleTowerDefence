using System;
using Sirenix.OdinInspector;
using Sources.Frameworks.UiFramework.Domain.Localizations.Phrases;
using Sources.Utils.Dictionaries;

namespace Sources.Frameworks.UiFramework.Domain.Dictionaries
{
    [Serializable] [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
    public class StringPhraseSerializedDictionary : SerializedDictionary<string, LocalizationPhraseClass>
    {
    }
}