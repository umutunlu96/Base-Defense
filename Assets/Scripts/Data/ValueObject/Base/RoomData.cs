using System;
using Abstract;

namespace Data.ValueObject.Base
{
    [Serializable]
    public class RoomData : ISaveableEntity
    {
        public TurretData TurretData;
        
        public int Cost;

        public int PayedAmount;
        
        public string Key = "RoomData";

        public string GetKey() => Key;
    }
}