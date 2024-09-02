namespace Sources.Frameworks.YandexSdkFramework.Leaderboards.Presentations.Interfaces.Views
{
    public interface ILeaderBoardElementView
    {
        void SetName(string playerName);
        void SetRank(string rank);
        void SetScore(string score);
    }
}