using System;

namespace Sources.Frameworks.MyGameCreator.Core.Editor.Common
{
    [Serializable]
    public class Parameter
    {
        public string name;
        public string description;

        public Parameter()
        {
            this.name = string.Empty;
            this.description = string.Empty;
        }

        public Parameter(string name, string description)
        {
            this.name = name;
            this.description = description;
        }
    }
}