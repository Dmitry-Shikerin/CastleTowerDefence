using System;
using Sirenix.OdinInspector;
using Sources.Frameworks.UiFramework.AudioSources.Domain.Groups;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation.Types;
using Sources.Utils.Dictionaries;

namespace Sources.Frameworks.UiFramework.AudioSources.Domain.Dictionaries
{
    [Serializable] [DictionaryDrawerSettings(KeyLabel = "Id",ValueLabel = "AudioGroup")]
    public class AudioClipGroupDictionary : SerializedDictionary<AudioGroupId, AudioGroup>
    {
    }
}