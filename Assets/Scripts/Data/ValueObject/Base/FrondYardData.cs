using System;
using System.Collections.Generic;

namespace Data.ValueObject.Base
{   
    [Serializable]
    public class FrondYardData
    {
        public List<ForceFieldData> ForceFieldData;
        public List<FrondYardItemsData> FrondYardItemsDatas;
    }
}