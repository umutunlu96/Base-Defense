using System;
using Abstract;

namespace Data.ValueObject.AI
{
    [Serializable]
    public class AISoldierData : Damageable
    {
        public int AttackRate;
        
        protected AISoldierData(int damage, int health, int speed) : base(damage, health, speed)
        {
        }
    }
}