using System;
using Sources.Frameworks.MyGameCreator.Core.Runtime.Common;

namespace Sources.Frameworks.MyGameCreator.Core.Editor.Common
{
    internal class TypeChapter
    {
        public TypePage First { get; }

        public TypeChapter(Type type)
        {
            Trie<Type> trie = TypeUtils.GetTypesTree(type);
            this.First = new TypePage(trie, false);
        }
    }
}