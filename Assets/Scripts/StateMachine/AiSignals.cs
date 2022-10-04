using System;
using Abstract;
using Enums;
using Extentions;
using StateMachine.Enemy;
using UnityEngine;

namespace StateMachine
{
    public class AiSignals : MonoSingleton<AiSignals>
    {
        #region Hostage

        public Action<Transform> onHostageRescued;

        #endregion
        
        #region Miner
        
        public Func<Transform> onGetMineBaseArea;
        public Func<MineWorkerType,Transform> onGetResourceArea;
        public Func<Transform> onGetGatherArea;
        public Func<bool> onCanPlaceDiamondToStockpileArea;
        public Action<GameObject> onPlaceDiamondToStockpileArea;
        public Action onPlayerCollectedAllGems;
        #endregion

        #region Soldier

        public Func<Transform> onGetSoldierTentArea;
        public Func<Transform> onGetWaitSlotArea;

        #endregion

        #region Enemy

        public Action<Transform> onEnemyDead;
        public Func<Transform> onGetBaseAttackPoint;
        public Action<IDamageable> onEnemyAIDead;

        #endregion

        #region MoneyWorker
        
        public Func<Transform> onGetBaseTransform;
        public Func<Transform> onGetOutsideTransform;

        #endregion

        #region AmmoWorker

        public Func<Transform> onGetAmmoWarehouseTransform;

        #endregion
    }
}