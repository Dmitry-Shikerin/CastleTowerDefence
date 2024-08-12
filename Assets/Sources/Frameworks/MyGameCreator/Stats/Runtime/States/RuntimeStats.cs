using System;
using System.Collections.Generic;
using Sources.Frameworks.MyGameCreator.Core.Editor.Common;
using Sources.Frameworks.MyGameCreator.Stats.Runtime.Traitses;

namespace Sources.Frameworks.MyGameCreator.Stats.Runtime
{
    [Serializable]
    public class RuntimeStats
    {
                // MEMBERS: -------------------------------------------------------------------------------

        // private Traits m_Traits;
        // private Dictionary<int, RuntimeStatData> m_Stats;
        //
        // // PROPERTIES: ----------------------------------------------------------------------------
        //
        // public int Count => this.m_Stats.Count;
        //
        // /// <summary>
        // /// Returns the difference between the the previous and new value from the last
        // /// modified Stat.
        // /// </summary>
        // public double LastChange { get; private set; }
        //
        // internal List<int> StatsKeys => new List<int>(this.m_Stats.Keys);
        //
        // // EVENTS: --------------------------------------------------------------------------------
        //
        // public event Action<IdString> EventChange;
        //
        // // CONSTRUCTOR: ---------------------------------------------------------------------------
        //
        // internal RuntimeStats(Traits traits)
        // {
        //     this.m_Traits = traits;
        //     this.m_Stats = new Dictionary<int, RuntimeStatData>();
        // }
        //
        //  
        // // PRIVATE METHODS: -----------------------------------------------------------------------
        //
        // private void ExecuteEventChange(IdString statID, double change)
        // {
        //     this.LastChange = change;
        //     this.EventChange?.Invoke(statID);
        // }
        //
        // // PUBLIC METHODS: ------------------------------------------------------------------------
        //
        // /// <summary>
        // /// Returns the RuntimeStatData instance of a requested stat. Throws an exception if the
        // /// requested stat cannot be found.
        // /// </summary>
        // /// <param name="statID"></param>
        // /// <returns></returns>
        // public RuntimeStatData Get(IdString statID)
        // {
        //     if (this.m_Stats == null) return null;
        //     if (this.m_Stats.TryGetValue(statID.Hash, out RuntimeStatData stat))
        //     {
        //         return stat;
        //     }
        //
        //     string objectName = this.m_Traits.gameObject.name;
        //     throw new Exception($"Cannot find Stat '{statID.String}' in {objectName}");
        // }
        //
        // /// <summary>
        // /// Returns the RuntimeStatData instance of a requested stat. Throws an exception if the
        // /// requested stat cannot be found.
        // /// </summary>
        // /// <param name="statID"></param>
        // /// <returns></returns>
        // public RuntimeStatData Get(string statID)
        // {
        //     return this.Get(new IdString(statID));
        // }
        //
        // /// <summary>
        // /// Clears all Stat Modifiers applied to any Stat of the Traits component.
        // /// </summary>
        // public void ClearModifiers()
        // {
        //     foreach (KeyValuePair<int, RuntimeStatData> entry in this.m_Stats)
        //     {
        //         entry.Value.ClearModifiers();
        //     }
        // }
        //
        // // INTERNAL METHODS: ----------------------------------------------------------------------
        //
        // /// <summary>
        // /// Returns the RuntimeStatData instance of a requested stat.
        // /// </summary>
        // /// <param name="statHash"></param>
        // /// <returns></returns>
        // internal RuntimeStatData Get(int statHash)
        // {
        //     return this.m_Stats.TryGetValue(statHash, out RuntimeStatData stat) ? stat : null;
        // }
    }
}