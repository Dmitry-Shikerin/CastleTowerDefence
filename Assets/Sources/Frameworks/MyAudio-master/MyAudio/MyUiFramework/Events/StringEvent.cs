using System;
using UnityEngine.Events;

namespace MyAudios.MyUiFramework.Events
{
    /// <inheritdoc />
    /// <summary> UnityEvent used to send strings </summary>
    [Serializable]
    public class StringEvent : UnityEvent<string> { }
}