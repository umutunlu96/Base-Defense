using System;
using Abstract;

namespace Data.ValueObject.AI
{   
    [Serializable]
    public class AIAmmoWorkerData : Worker
    {
        public AIAmmoWorkerData(float speed, int capacity) : base(speed, capacity)
        {
        }
    }
}