namespace Sources.Frameworks.UiFramework.ServicesInterfaces.Localizations
{
    public interface ILocalizationService
    {
        void Translate();
        string GetText(string key);
    }
}