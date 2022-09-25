using System;
using Enums;

namespace Data.ValueObject
{
    [Serializable]
    public class EnemySpawnData
    {
        public EnemyType EnemyType;
        public int MaxSpawnAmount;
        public int CurrentSpawnAmount;
    }
}