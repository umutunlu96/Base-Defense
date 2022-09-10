using System;
using System.Collections.Generic;

namespace Data.ValueObject.Base
{   
    [Serializable]
    public class FrondYardData
    {
        public List<StageData> StageDatas;
        public List<FrondYardItemsData> FrondYardItemsDatas;
    }
}