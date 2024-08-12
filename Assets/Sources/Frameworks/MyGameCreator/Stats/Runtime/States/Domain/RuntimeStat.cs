namespace Sources.Frameworks.MyGameCreator.Stats.Runtime
{
    public class RuntimeStat
    {
        private string _id;
        private StatData _data;
        private StatInfo _info;

        public RuntimeStat(Stat stat)
        {
            _id = stat.Id;
            _data = stat.Data;
            _info = stat.Info;
        }

        public string ID => _id;
        public double Value => _data.BaseValue;
    }
}