using System;

namespace Data.ValueObject
{
    [Serializable]
    public class BulletData
    {
        public float AutoDestroyTime;
        public float SpawnDelay;
        public float MoveSpeed;
        public int Damage;
    }
}