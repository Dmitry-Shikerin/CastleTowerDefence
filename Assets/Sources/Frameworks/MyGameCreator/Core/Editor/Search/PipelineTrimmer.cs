using System.Text.RegularExpressions;

namespace Sources.Frameworks.MyGameCreator.Core.Editor.Search
{
    public class PipelineTrimmer : IPipeline
    {
        private static readonly Regex TrimStartExpression = new Regex(@"^\W+");
        private static readonly Regex TrimEndExpression = new Regex(@"\W+$");

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public string Run(string term)
        {
            term = TrimStartExpression.Replace(term, string.Empty);
            return TrimEndExpression.Replace(term, string.Empty);
        }
    }
}