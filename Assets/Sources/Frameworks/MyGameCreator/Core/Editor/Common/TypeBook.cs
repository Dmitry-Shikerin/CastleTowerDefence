using System;
using System.Collections.Generic;

namespace Sources.Frameworks.MyGameCreator.Core.Editor.Common
{
    internal static class TypeBook
    {
        private static readonly Dictionary<Type, TypeChapter> Books =
            new Dictionary<Type, TypeChapter>();

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public static void Awake(Type type)
        {
            if (!Books.ContainsKey(type))
            {
                Books.Add(type, new TypeChapter(type));
            }
        }

        public static TypeChapter Fetch(Type type)
        {
            Awake(type);
            return Books[type];
        }
    }
}