using System;
using Abstract;
using Enums;

namespace Data.ValueObject.Base
{
    [Serializable]
    public class MoneyWorkerData: ISaveableEntity
    {
        public BuyState BuyState;
        public int MoneyWorkerCost;
        public int MoneyWorkerPayedAmount;
        public int MoneyWorkerLevel = 1;
        
        public MoneyWorkerData(){}

        public MoneyWorkerData(BuyState buyState, int moneyWorkerCost, int moneyWorkerPayedAmount, int moneyWorkerLevel)
        {
            BuyState = buyState;
            MoneyWorkerCost = moneyWorkerCost;
            MoneyWorkerPayedAmount = moneyWorkerPayedAmount;
            MoneyWorkerLevel = moneyWorkerLevel;
        }
        
        public string Key = "MoneyWorkerData";
        
        public string GetKey() => Key;
    }
}