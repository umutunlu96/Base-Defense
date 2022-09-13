using System;
using Abstract;

namespace Data.ValueObject.Base
{
    [Serializable]
    public class RoomData : ISaveableEntity
    {
        public string Key = "RoomData";
        public string GetKey() => Key;
        
        public int Cost;

        public int PayedAmount;
        
        public TurretData TurretData;

        public RoomData(){}

        public RoomData(int cost, int payedAmount, TurretData turretData)
        {
            Cost = cost;
            PayedAmount = payedAmount;
            TurretData = turretData;
        }
    }
}