using System;
using Sources.Domain.Models.Data;
using Sources.Frameworks.Domain.Interfaces.Entities;

namespace Sources.BoundedContexts.KillEnemyCounters.Domain.Models.Implementation
{
    public class KillEnemyCounter : IEntity
    {
        // public KillEnemyCounter(
        //     string id,
        //     int killZombies)
        // {
        //     Id = id;
        //     KillZombies = killZombies;
        // }

        public event Action KillZombiesCountChanged;

        public string Id { get; set; }
        public Type Type => GetType();
        public int KillZombies { get; set; }

        public void IncreaseKillCount()
        {
            KillZombies++;
            KillZombiesCountChanged?.Invoke();
        }
    }
}