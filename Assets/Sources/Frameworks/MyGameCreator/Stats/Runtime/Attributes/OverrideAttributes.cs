using System;
using System.Collections.Generic;
using Sources.Frameworks.MyGameCreator.Core.Editor.Common;
using Sources.Frameworks.MyGameCreator.Core.Runtime.Common;
using Sources.Frameworks.MyGameCreator.Stats.Runtime.Classes;

namespace Sources.Frameworks.MyGameCreator.Stats.Runtime
{
    [Serializable]
    public class OverrideAttributes : TSerializableDictionary<IdString, OverrideAttributeData>
    {
        internal void SyncWithClass(Class traitsClass)
        {
            Dictionary<IdString, OverrideAttributeData> data = 
                new Dictionary<IdString, OverrideAttributeData>();

            // for (int i = 0; i < traitsClass.AttributesLength; ++i)
            // {
            //     AttributeItem attribute = traitsClass.GetAttribute(i);
            //     if (attribute?.Attribute == null || attribute.IsHidden) continue;
            //
            //     this.m_Dictionary.TryGetValue(attribute.Attribute.ID, out OverrideAttributeData entry);
            //     data[attribute.Attribute.ID] = entry ?? new OverrideAttributeData();
            // }

            this.m_Dictionary = data;
        }
    }
}