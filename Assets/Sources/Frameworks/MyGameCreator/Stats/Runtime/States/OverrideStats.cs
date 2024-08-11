﻿using System;
using System.Collections.Generic;
using Sources.Frameworks.MyGameCreator.Core.Editor.Common;
using Sources.Frameworks.MyGameCreator.Core.Runtime.Common;
using Sources.Frameworks.MyGameCreator.Stats.Runtime.Classes;

namespace Sources.Frameworks.MyGameCreator.Stats.Runtime
{
    [Serializable]
    public class OverrideStats : TSerializableDictionary<IdString, OverrideStatData>
    {
        // internal void SyncWithClass(Class traitsClass)
        // {
        //     Dictionary<IdString, OverrideStatData> data = new Dictionary<IdString, OverrideStatData>();
        //
        //     for (int i = 0; i < traitsClass.StatsLength; ++i)
        //     {
        //         StatItem stat = traitsClass.GetStat(i);
        //         if (stat?.Stat == null || stat.IsHidden) continue;
        //
        //         this.m_Dictionary.TryGetValue(stat.Stat.ID, out OverrideStatData entry);
        //         data[stat.Stat.ID] = entry ?? new OverrideStatData();
        //     }
        //
        //     this.m_Dictionary = data;
        // }
    }
}