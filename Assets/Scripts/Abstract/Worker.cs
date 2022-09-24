using UnityEngine;

namespace Abstract
{
    public abstract class Worker : MonoBehaviour
    {
        public int Capacity;

        public float Speed;

        protected Worker(float speed, int capacity)
        {
            Speed = speed;
            Capacity = capacity;
        }
    }
}