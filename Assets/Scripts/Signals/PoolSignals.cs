using System;
using Enums;
using Extentions;
using UnityEngine;

namespace Signals
{
    public class PoolSignals : MonoSingleton<PoolSignals>
    {
        public Func<PoolType, Transform, GameObject> onGetPoolObject;
        public Action<PoolType, GameObject> onReleasePoolObject;


        public Func<string, Transform, GameObject> onGetPoolObjectWithString;
        public Action<string, GameObject> onReleasePoolObjectWitString;
    }
}