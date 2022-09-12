using System;
using Abstract;

namespace Data.ValueObject.AI
{
    [Serializable]
    public class AIMoneyWorkerData : Worker
    {
        public AIMoneyWorkerData(float speed, int capacity) : base(speed, capacity)
        {
        }
    }
}