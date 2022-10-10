using System;
using System.Collections.Generic;

namespace Data.ValueObject.Base
{   
    [Serializable]
    public class FrontYardData
    {
        public List<ForceFieldData> ForceFieldData;
        public List<FrontYardItemsData> FrondYardItemsDatas;
    }
}