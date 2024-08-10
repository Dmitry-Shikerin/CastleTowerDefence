using System;

namespace Sources.Frameworks.MyGameCreator.Core.Runtime.Common
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HideLabelsInEditorAttribute : Attribute
    {
        public bool Hide { get; }

        public HideLabelsInEditorAttribute(bool hide = true)
        {
            this.Hide = hide;
        }
    }
}