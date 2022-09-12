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
        
        
        public MineBaseData(){}

        public MineBaseData(int maxWorkerAmount, int currentWorkerAmount, int diamondCapacity, int currentDiamondAmount, int mineCardCapacity)
        {
            MaxWorkerAmount = maxWorkerAmount;
            CurrentWorkerAmount = currentWorkerAmount;
            DiamondCapacity = diamondCapacity;
            CurrentDiamondAmount = currentDiamondAmount;
            MineCardCapacity = mineCardCapacity;
        }
        
        public string Key = "MineBaseData";

        public string GetKey() => Key;
    }
}