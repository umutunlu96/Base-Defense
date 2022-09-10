using System;
using System.Collections.Generic;

namespace Data.ValueObject.Base
{   
    [Serializable]
    public class BaseRoomData
    {
        public List<RoomData> RoomDatas = new List<RoomData>();
    }
}