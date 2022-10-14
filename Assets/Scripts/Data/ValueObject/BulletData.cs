using System;

namespace Data.ValueObject
{
    [Serializable]
    public class BulletData
    {
        public bool Bought;
        public int Level = 1;
        public float AutoDestroyTime;
        public float SpawnDelay;
        public float MoveSpeed;
        public float Damage;
    }
}