using System;
using Abstract;
using Enums;

namespace Data.ValueObject.Base
{
    [Serializable]
    public class AmmoWorkerData : ISaveableEntity
    {
        public AmmoWorkerData(){}

        public AmmoWorkerData(BuyState buyState, int ammoWorkerCost, int ammoWorkerPayedAmount, int ammoWorkerLevel)
        {
            BuyState = buyState;
            AmmoWorkerCost = ammoWorkerCost;
            AmmoWorkerPayedAmount = ammoWorkerPayedAmount;
            AmmoWorkerLevel = ammoWorkerLevel;
        }
        
        public BuyState BuyState;
        public int AmmoWorkerCost;
        public int AmmoWorkerPayedAmount;
        public int AmmoWorkerLevel = 1;
        
        public string Key = "AmmoWorkerData";
        
        public string GetKey() => Key;
    }
}