using System;
using Sources.Domain.Models.Data;

namespace Sources.BoundedContexts.KillEnemyCounters.Domain
{
    public class KillEnemyCounter
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

        public int KillZombies { get; private set; }
        public string Id { get; }
        public Type Type => GetType();

        public void IncreaseKillCount()
        {
            KillZombies++;
            KillZombiesCountChanged?.Invoke();
        }
    }
}