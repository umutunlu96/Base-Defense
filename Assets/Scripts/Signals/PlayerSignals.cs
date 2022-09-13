using System;
using Extentions;

namespace Signals
{
    public class PlayerSignals : MonoSingleton<PlayerSignals>
    {
        public Func<bool> canBuy;
    }
}