namespace Sources.PresentationsInterfaces.Views.Constructors
{
    public interface IConstruct<in T>
    {
        void Construct(T playerWallet);
    }
}