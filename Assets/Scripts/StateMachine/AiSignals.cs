using System;
using Extentions;
using UnityEngine;

namespace StateMachine
{
    public class AiSignals : MonoSingleton<AiSignals>
    {
        public Func<Transform> onGetResourceArea;
        public Func<Transform> onGetGatherArea;

        public Action<Transform> onMinerJoinedResourceArea;
    }
}