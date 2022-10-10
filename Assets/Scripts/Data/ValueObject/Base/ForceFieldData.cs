using System;
using Abstract;

namespace Data.ValueObject.Base
{
    [Serializable]
    public class ForceFieldData : ISaveableEntity
    {
        public string Key = "ForceFieldData";
        
        public int Cost;

        public int PayedAmount;

        public ForceFieldData(){}
        
        public ForceFieldData(int cost, int payedAmount)
        {
            Cost = cost;
            PayedAmount = payedAmount;
        }
        
        public string GetKey() => Key;
    }
}