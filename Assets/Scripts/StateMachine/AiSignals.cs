using System;
using Extentions;
using StateMachine.Enemy;
using UnityEngine;

namespace StateMachine
{
    public class AiSignals : MonoSingleton<AiSignals>
    {
        #region Miner
        
        public Func<Transform> onGetMineBaseArea;
        public Func<Transform> onGetResourceArea;
        public Func<Transform> onGetGatherArea;

        public Action<GameObject> onPlaceDiamondToGatherArea;

        #endregion

        #region Soldier

        public Func<Transform> onGetSoldierTentArea;
        public Func<Transform> onGetWaitSlotArea;

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