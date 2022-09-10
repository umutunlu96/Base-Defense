using System;
using Data.ValueObject.Base;

namespace Data.ValueObject
{
    [Serializable]
    public class LevelData
    {
        public FrondYardData FrondYardData;
        public BaseData BaseData;
    }
}