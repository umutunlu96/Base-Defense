using System;
using System.Collections.Generic;
using Data.UnityObject;
using Data.ValueObject.Base;
using Signals;
using StateMachine;
using StateMachine.Enemy;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class BaseManager : MonoBehaviour
    {
        #region Variables

        #region Public

        public BaseData BaseData;
        
        #endregion

        #region Serialized

        [SerializeField] private List<RoomManager> roomManagers;
        [SerializeField] private List<Transform> baseLeftAttackPoints;
        [SerializeField] private List<Transform> baseRightAttackPoints;

        [SerializeField] private Transform mineBaseTransform;
        [SerializeField] private Transform baseTransform;
        [SerializeField] private Transform outDoorTransform;
        
        #endregion

        #region Private

        private int _levelID;
        private int _uniqueId;

        #endregion

        #endregion
        
        private int GetLevelID => LevelSignals.Instance.onGetLevelID();
        
        private void GetBaseData() => BaseData =  Resources.Load<CD_Level>("Data/CD_Level").Levels[GetLevelID-1].
            BaseData;

        private void Start()
        {
            GetBaseData();
            SetDataToManagers();
        }
        
        private void SetDataToManagers()
        {
            for (int i = 0; i < roomManagers.Count; i++)
            {
                roomManagers[i].SetData(BaseData.BaseRoomData.RoomDatas[i], i);
            }
        }
        
        #region EventSubscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            AiSignals.Instance.onGetMineBaseArea += OnGetMineBaseTransform;
            AiSignals.Instance.onGetBaseAttackPoint += OnReturnBaseAttackPoint;
            AiSignals.Instance.onGetBaseTransform += OnGetBaseTransform;
            AiSignals.Instance.onGetOutsideTransform += OnGetOutDoorTransform;
        }
        
        private void UnSubscribeEvents()
        {
            AiSignals.Instance.onGetMineBaseArea -= OnGetMineBaseTransform;
            AiSignals.Instance.onGetBaseAttackPoint -= OnReturnBaseAttackPoint;
            AiSignals.Instance.onGetBaseTransform -= OnGetBaseTransform;
            AiSignals.Instance.onGetOutsideTransform -= OnGetOutDoorTransform;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
        
        #endregion

        #region Event Functions
        
        private Transform OnReturnBaseAttackPoint(AttackSide attackSide)
        {
            switch (attackSide)
            {
                case AttackSide.Left:
                    var enemyAttackTransformCountLeft = baseLeftAttackPoints.Count;
                    var randomsLeft = Random.Range(0, enemyAttackTransformCountLeft);
                    return baseLeftAttackPoints[randomsLeft];

                case AttackSide.Right:
                    var enemyAttackTransformCountRight = baseRightAttackPoints.Count;
                    var randomsRight = Random.Range(0, enemyAttackTransformCountRight);
                    return baseRightAttackPoints[randomsRight];
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(attackSide), attackSide, null);
            }
        }

        private Transform OnGetBaseTransform() => baseTransform;
        
        private Transform OnGetOutDoorTransform() => outDoorTransform;

        private Transform OnGetMineBaseTransform() => mineBaseTransform;

        #endregion
    }
}