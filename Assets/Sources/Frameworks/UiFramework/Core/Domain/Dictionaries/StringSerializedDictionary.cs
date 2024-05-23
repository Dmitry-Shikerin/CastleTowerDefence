using System;
using Sirenix.OdinInspector;
using Sources.Utils.Dictionaries;

namespace Sources.Frameworks.UiFramework.Domain.Dictionaries
{
    [Serializable] [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
    public class StringSerializedDictionary : SerializedDictionary<string, string>
    {
    }
}