using System;
using Sources.Frameworks.UiFramework.AudioSources.Domain.Groups;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation.Types;
using Sources.Utils.Dictionaries;

namespace Sources.Frameworks.UiFramework.AudioSources.Domain.Dictionaries
{
    [Serializable]
    public class AudioClipGroupDictionary : SerializedDictionary<AudioGroupId, AudioGroup>
    {
    }
}