namespace Sources.Frameworks.MyGameCreator.Core.Runtime.Common
{
    public interface ISearchable
    {
        string SearchText  { get; }
        int SearchPriority { get; }
    }
}