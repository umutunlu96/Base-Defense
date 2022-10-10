using System;
using Data.ValueObject.Base;

namespace Data.ValueObject
{
    [Serializable]
    public class LevelData
    {
        public FrontYardData frontYardData;
        public BaseData BaseData;
    }
}