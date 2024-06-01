using System;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation.Types;
using Sources.Utils.Dictionaries;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.AudioSources.Domain
{
    [Serializable]
    public class AudioClipDictionary : SerializedDictionary<AudioClipId, AudioClip>
    {
    }
}