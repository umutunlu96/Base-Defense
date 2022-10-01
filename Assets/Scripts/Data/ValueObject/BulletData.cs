using System;
using UnityEngine;

namespace Data.ValueObject
{
    [Serializable]
    public class BulletData
    {
        [Range(1,10)] public float AutoDestroyTime;
        [Range(0, 10)] public float SpawnDelay;
        [Range(1,10)] public float MoveSpeed;
        [Range(1,10)] public int Damage;
    }
}