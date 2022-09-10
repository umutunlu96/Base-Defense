using System;
using Abstract;

namespace Data.ValueObject.Base
{   
    [Serializable]
    public class MineBaseData : ISaveableEntity
    {
        public int MaxWorkerAmount;
        
        public int CurrentWorkerAmount;
        
        public int DiamondCapacity;
        
        public int CurrentDiamondAmount;
        
        public int MineCardCapacity;
        
        public string Key = "MineBaseData";

        public string GetKey() => Key;
    }
}