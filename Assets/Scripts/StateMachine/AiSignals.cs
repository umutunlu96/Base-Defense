using System;
using System.Collections.Generic;
using Abstract;
using Enums;
using Extentions;
using Managers;
using StateMachine.Boss;
using StateMachine.Soldier;
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

        public Func<Transform> onGetMilitaryBaseTentEnterenceTransform;
        public Func<Transform> onGetMilitaryBaseTentTransform;
        public Action<Transform> onCandidateEnteredMilitaryArea;
        public Func<Transform> onGetSoldierWaitTransform;
        public Func<Transform> onGetOutsideTransform;
        public Func<int> onGetCurrentEmptySlotForCandidate;
        public Action onAttackAllSoldiers;

        #endregion

        #region Enemy
        
        public Action<Transform> onEnemyDead;
        public Func<Transform> onGetBaseAttackPoint;
        public Action<IDamageable> onEnemyAIDead;
        public Action<bool> onPlayerIsAtOutside;
        public Action<Transform> onGroundMinePlanted;
        public Action onGroundMineExplode;
            
        #endregion

        #region MoneyWorker
        
        public Func<Transform> onGetBaseTransform;

        #endregion

        #region AmmoWorker

        public Func<Transform> onGetAmmoWarehouseTransform;
        public Func<List<TurretManager>> onGetTurretManagers;
        public Func<int> onGetTurretAmmoStock;

        #endregion

        #region Boss

        public Action onBossDead;
        public Action onPlayerEnterBossArea;
        public Action onPlayerLeaveBossArea;
        public Action<Grenade> onGrenadeSpawned;
        public Action onGrenadeThrowed;
        public Action onGrenadeExplode;

        #endregion
    }
}