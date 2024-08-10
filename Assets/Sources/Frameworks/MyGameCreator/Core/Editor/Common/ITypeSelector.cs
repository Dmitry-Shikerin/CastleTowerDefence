using System;

namespace Sources.Frameworks.MyGameCreator.Core.Editor.Common
{
    public interface ITypeSelector
    {
        event Action<Type, Type> EventChange;
    }
}