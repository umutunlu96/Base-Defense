using System;
using Abstract;

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
        
        public TurretData(){}

        public TurretData(int cost, int payedAmount, int ammoCapacity)
        {
            Cost = cost;
            PayedAmount = payedAmount;
            AmmoCapacity = ammoCapacity;
        }
    }
}