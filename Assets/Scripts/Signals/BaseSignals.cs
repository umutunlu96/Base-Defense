using System;
using Extentions;

namespace Signals
{
    public class BaseSignals : MonoSingleton<BaseSignals>
    {
        public Func<int> onGetMineBaseEmptySlotCount;
    }
}