using System;
using Sirenix.OdinInspector;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation.Types;
using Sources.Frameworks.Utils.Dictionaries;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.AudioSources.Domain.Dictionaries
{
    [Serializable] [DictionaryDrawerSettings(KeyLabel = "Id",ValueLabel = "AudioClip")]
    public class AudioClipDictionary : SerializedDictionary<AudioClipId, AudioClip>
    {
    }
}