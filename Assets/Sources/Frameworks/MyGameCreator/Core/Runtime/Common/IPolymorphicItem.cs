using UnityEngine;

namespace Sources.Frameworks.MyGameCreator.Core.Runtime.Common
{
    public interface IPolymorphicItem
    {
        string Title { get; }
        Color Color  { get; }
    }
}