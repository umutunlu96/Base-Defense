using System;
using Abstract;

namespace Data.ValueObject.AI
{
    [Serializable]
    public class AIMineWorkerData : Worker
    {
        public AIMineWorkerData(float speed, int capacity) : base(speed, capacity)
        {
        }
    }
}