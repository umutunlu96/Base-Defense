using System;
using Data.ValueObject.Base;
using Extentions;
using Keys;

namespace Signals
{
    public class SaveLoadSignals : MonoSingleton<SaveLoadSignals>
    {
        public Action<MoneyWorkerData, int> onSaveMoneyWorkerData;
        public Func<string, int, MoneyWorkerData> onLoadMoneyWorkerData;
        
        public Action<AmmoWorkerData, int> onSaveAmmoWorkerData;
        public Func<string, int, AmmoWorkerData> onLoadAmmoWorkerData;

        public Action<MineBaseData, int> onSaveMineBaseData;
        public Func<string, int, MineBaseData> onLoadMineBaseData;

        public Action<RoomData, int> onSaveRoomData;
        public Func<string, int, RoomData> onLoadRoomData;

        public Action<ScoreParams, int> onSaveScoreParams;
        public Func<string, int, ScoreParams> onLoadScoreParams;
    }
}