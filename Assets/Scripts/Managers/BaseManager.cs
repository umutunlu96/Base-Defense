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
        [SerializeField] private List<Transform> baseRighttAttackPoints;

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
            AiSignals.Instance.onGetBaseAttackPoint += ReturnBaseAttackPoint;
        }
        
        private void UnSubscribeEvents()
        {
            AiSignals.Instance.onGetBaseAttackPoint -= ReturnBaseAttackPoint;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
        
        #endregion

        #region Event Functions
        
        private Transform ReturnBaseAttackPoint(AttackSide attackSide)
        {
            switch (attackSide)
            {
                case AttackSide.Left:
                    var enemyAttackTransformCountLeft = baseLeftAttackPoints.Count;
                    var randomsLeft = Random.Range(0, enemyAttackTransformCountLeft);
                    return baseLeftAttackPoints[randomsLeft];

                case AttackSide.Right:
                    var enemyAttackTransformCountRight = baseRighttAttackPoints.Count;
                    var randomsRight = Random.Range(0, enemyAttackTransformCountRight);
                    return baseRighttAttackPoints[randomsRight];
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(attackSide), attackSide, null);
            }
        }
        
        #endregion
    }
}