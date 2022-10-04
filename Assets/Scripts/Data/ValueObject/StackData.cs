using System;

namespace Data.ValueObject
{
    [Serializable]
    public class StackData
    {
        public int MaxHeight;
        public int Capacity;
        public float LerpSpeed;
        public float OffsetY;
        public float OffsetZ;
    }
}