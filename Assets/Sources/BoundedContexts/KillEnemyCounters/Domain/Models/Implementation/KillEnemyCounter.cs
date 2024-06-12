using System;
using Sources.Domain.Models.Data;
using Sources.Frameworks.Domain.Interfaces.Entities;

namespace Sources.BoundedContexts.KillEnemyCounters.Domain.Models.Implementation
{
    public class KillEnemyCounter : IEntity
    {
        public KillEnemyCounter(KillEnemyCounterDto dto)
        {
            Id = dto.Id;
            KillZombies = dto.KillZombies;
        }

        public KillEnemyCounter(
            string id,
            int killZombies)
        {
            Id = id;
            KillZombies = killZombies;
        }

        public event Action KillZombiesCountChanged;

        public string Id { get; }
        public Type Type => GetType();
        public int KillZombies { get; private set; }

        public void IncreaseKillCount()
        {
            KillZombies++;
            KillZombiesCountChanged?.Invoke();
        }
    }
}