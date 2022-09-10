using System;
using Abstract;
using Enums;

namespace Data.ValueObject.Base
{   
    [Serializable]
    public class BuyablesData : ISaveableEntity
    {
        public int MoneyWorkerCost;
        public int MoneyWorkerPayedAmount;
        
        public int AmmoWorkerCost;
        public int AmmoWorkerPayedAmount;  //Workers

        public int MoneyWorkerLevel;
        public int AmmoWorkerLevel;
        
        public BuyState BuyState;

        public string Key = "BuyablesData";
        
        public string GetKey() => Key;
    }
}