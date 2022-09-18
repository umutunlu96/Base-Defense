using System;
using Extentions;
using StateMachine.Enemy;
using UnityEngine;

namespace StateMachine
{
    public class AiSignals : MonoSingleton<AiSignals>
    {
        public Action<bool> onGateOpenState;


        #region Miner
        
        public Func<Transform> onGetResourceArea;
        public Func<Transform> onGetGatherArea;
        
        #endregion

        #region Enemy

        public Func<AttackSide, Transform> onGetBaseAttackPoint;

        #endregion

        #region MoneyWorker
        
        public Func<Transform> onGetBaseTransform;
        public Func<Transform> onGetOutsideTransform;

        #endregion


    }
}