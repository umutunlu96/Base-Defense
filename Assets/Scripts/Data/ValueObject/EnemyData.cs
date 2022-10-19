using System;
using Abstract;
using Enums;

namespace Data.ValueObject
{   
    [Serializable]
    public class EnemyData
    {
        public int Health;
        public int Damage;
        public float AttackRange;
        public float ChaseRange;
        public float WalkSpeed;
        public float RunSpeed;
    }
}