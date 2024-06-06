using System;

namespace Sources.DomainInterfaces.Models.Gameplay
{
    public interface IKillEnemyCounter
    {
        event Action KillZombiesCountChanged;
        
        public int KillZombies { get; }
    }
}