﻿using System;
using Sources.Frameworks.MyGameCreator.Core.Runtime.Common;

namespace Sources.Frameworks.MyGameCreator.Core.Editor.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class DescriptionAttribute : Attribute, ISearchable
    {
        public string Description { get; }

        public DescriptionAttribute(string description)
        {
            this.Description = description.Trim();
        }

        public string SearchText => this.Description;
        public int SearchPriority => 2;
    }
}