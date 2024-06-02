using System;
using Sirenix.OdinInspector;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation.Types;
using Sources.Utils.Dictionaries;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.AudioSources.Domain
{
    [Serializable] [DictionaryDrawerSettings(KeyLabel = "Id",ValueLabel = "AudioClip")]
    public class AudioClipDictionary : SerializedDictionary<AudioClipId, AudioClip>
    {
    }
}