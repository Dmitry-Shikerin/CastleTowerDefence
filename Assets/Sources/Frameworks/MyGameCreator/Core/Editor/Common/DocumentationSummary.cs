using System;

namespace Sources.Frameworks.MyGameCreator.Core.Editor.Common
{
    public class DocumentationSummary : DocumentationBaseElement
    {
        public DocumentationSummary(Type type) : base(type)
        {
            this.IncludeHeader(this, false);
            this.IncludeDescription(this, true);
        }
    }
}