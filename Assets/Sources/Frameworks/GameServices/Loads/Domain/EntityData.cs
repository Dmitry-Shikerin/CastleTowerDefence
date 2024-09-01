using System;

namespace Sources.Frameworks.GameServices.Loads.Domain
{
    public struct EntityData
    {
        public EntityData(
            string id,
            Type type,
            bool isDeleted)
        {
            ID = id;
            Type = type;
            IsDeleted = isDeleted;
        }

        public string ID { get; }
        public Type Type { get; }
        public bool IsDeleted { get; }
    }
}