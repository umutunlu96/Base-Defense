﻿using System;

namespace Data.ValueObject
{
    [Serializable]
    public class HostageSpawnData
    {
        public float SpawnDelay;
        public int MaxSpawnAmount;
        public int CurrentSpawnAmount;
    }
}