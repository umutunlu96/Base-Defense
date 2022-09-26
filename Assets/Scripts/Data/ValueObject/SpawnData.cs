using System;
using System.Collections.Generic;

namespace Data.ValueObject
{
    [Serializable]
    public class SpawnData
    {
        public float EnemySpawnDelay;
        public List<EnemySpawnData> EnemySpawnDatas;
        public HostageSpawnData HostageSpawnDatas;
        public BombSpawnData BombSpawnData;
    }
}