using System;
using Enums;
using Extentions;
using UnityEngine;

namespace Signals
{
    public class StackSignals : MonoSingleton<StackSignals>
    {
        public Action<Transform> onAddStack;
        public Action<Transform> onRemoveStack;

        public Action<HostageType> onRemoveAllStack;
    }
}