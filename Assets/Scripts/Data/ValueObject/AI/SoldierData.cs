using System;

namespace Data.ValueObject.AI
{
    [Serializable]
    public class SoldierData
    {
        public int Health;
        public int Damage;
        public int AttackRate;
        public float AttackRange;
        public float ChaseRange;
        public float ChaseUpdateSpeed = .2f;
        public float RunSpeed;
    }
}