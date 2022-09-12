using System;
using Data.ValueObject.Base;
using Extentions;
using UnityEngine.Events;

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
    }
}