﻿using System;

namespace Data.ValueObject.Base
{   
    [Serializable]
    public class BaseData
    {
        public BaseRoomData BaseRoomData;
        public MineBaseData MineBaseData;
        public MilitaryBaseData MilitaryBaseData;
        public AmmoWorkerData AmmoWorkerData;
        public MoneyWorkerData MoneyWorkerData;
    }
}