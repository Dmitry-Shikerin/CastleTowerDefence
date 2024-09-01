namespace Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views.Constructors
{
    public interface IConstruct<in T>
    {
        void Construct(T leaderBoardElementViews);
    }
}