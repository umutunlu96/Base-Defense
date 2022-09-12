using System;
using Extentions;

namespace Signals
{
    public class ScoreSignals : MonoSingleton<ScoreSignals>
    {
        public Action<int> onSetMoneyAmount;
        public Action<int> onSetDiamondAmount;
        
        public Func<int> onGetMoneyAmount;
        public Func<int> onGetDiamondAmount;
    }
}