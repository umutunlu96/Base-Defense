using System;
using Abstract;
using UnityEngine;

namespace Data.ValueObject.Base
{   
    [Serializable]
    public class TurretData : ISaveableEntity
    {
        public string Key = "TurretData";
        
        public string GetKey() => Key;
        
        public int Cost;

        public int PayedAmount;
        
        public int AmmoCapacity;

        public int AmmoDamage;
        
        public TurretData(){}

        public TurretData(int cost, int payedAmount, int ammoCapacity, int ammoDamage)
        {
            Cost = cost;
            PayedAmount = payedAmount;
            AmmoCapacity = ammoCapacity;
            AmmoDamage = ammoDamage;
        }
    }
}