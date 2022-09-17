using System;
using Extentions;
using StateMachine.Enemy;
using UnityEngine;

namespace StateMachine
{
    public class AiSignals : MonoSingleton<AiSignals>
    {
        #region Miner
        
        public Func<Transform> onGetResourceArea;
        public Func<Transform> onGetGatherArea;
        
        #endregion

        #region Enemy

        public Func<AttackSide, Transform> onGetBaseAttackPoint;

        #endregion


    }
}