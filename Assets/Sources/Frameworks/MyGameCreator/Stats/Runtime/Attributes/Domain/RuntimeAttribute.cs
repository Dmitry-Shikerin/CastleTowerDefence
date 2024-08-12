namespace Sources.Frameworks.MyGameCreator.Stats.Runtime
{
    public class RuntimeAttribute
    {
        private readonly string _id;
        private readonly AttributeData _data;
        private readonly AttributeInfo _info ;

        public RuntimeAttribute(Attribute attribute)
        {
            _id = attribute.ID;
            _info = attribute.Info;
            _data = attribute.Data;
        }

        public string ID => _id;
        public AttributeInfo Info => _info;
        public AttributeData Data => _data;
    }
}