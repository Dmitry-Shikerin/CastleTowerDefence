﻿using System;
using Sirenix.OdinInspector;
using Sources.Frameworks.UiFramework.Texts.Domain;
using Sources.Frameworks.UiFramework.Texts.Services.Localizations.Phrases;
using Sources.Utils.Dictionaries;

namespace Sources.Frameworks.UiFramework.Core.Domain.Dictionaries
{
    [Serializable] [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine)]
    public class PhraseSerializedDictionary : SerializedDictionary<LocalizationId, LocalizationPhraseClass>
    {
    }
}