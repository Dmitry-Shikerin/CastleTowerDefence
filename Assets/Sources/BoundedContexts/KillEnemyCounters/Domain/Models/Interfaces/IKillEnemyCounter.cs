using System;

namespace Sources.BoundedContexts.KillEnemyCounters.Domain.Models.Interfaces
{
    public interface IKillEnemyCounter
    {
        event Action KillZombiesCountChanged;
        
        public int KillZombies { get; }
    }
}