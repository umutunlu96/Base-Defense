using System;

namespace Data.ValueObject
{
    [Serializable]
    public class BombSpawnData
    {
        public float BombSpawnDelay;
        public int MaxSpawnAmount;
        public int CurrentSpawnAmount;
    }
}