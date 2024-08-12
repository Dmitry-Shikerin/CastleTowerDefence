using System.Collections.Generic;
using UnityEngine;

namespace Sources.Frameworks.MyGameCreator.Stats.Runtime.Classes
{
    public class RuntimeClass
    {
        private readonly string _id = "ID";
        private readonly string _description = "Description";
        private readonly Sprite _sprite;
        private readonly Color _color;
        private List<RuntimeAttribute> _attributes = new List<RuntimeAttribute>();
        private List<RuntimeStat> _stats = new List<RuntimeStat>();

        public RuntimeClass(Class @class)
        {
            _id = @class.ID;
            _description = @class.Description;
            _sprite = @class.Sprite;
            _color = @class.Color;
        }

        public string ID => _id;
        public string Description => _description;
        public Sprite Sprite => _sprite;
        public Color Color => _color;
        public List<RuntimeAttribute> Attributes => _attributes;
        public List<RuntimeStat> Stats => _stats;
    }
}