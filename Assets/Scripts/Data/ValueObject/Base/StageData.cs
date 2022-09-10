using System;
using Enums;

namespace Data.ValueObject.Base
{
    [Serializable]
    public class StageData
    {
        public BuyState BuyState;
        
        public int Cost;

        public int PayedAmount;
    }
}