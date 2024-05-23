using System;
using System.Collections.Generic;

namespace Sources.Utils.CustomCollections
{
    public interface ICustomCollection<T> : IReadOnlyList<T>
    {
        event Action CountChanged;
        event Action Added;
        event Action Removed;
    }
}