using System;
using System.Collections.Generic;
using Data.UnityObject;
using Data.ValueObject.Base;
using Signals;
using StateMachine;
using StateMachine.Enemy;
using TMPro;
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
        [SerializeField] private List<TurretManager> turretManagers;
        [SerializeField] private List<Transform> baseAttackPoints;
        
        [SerializeField] private TextMeshPro baseText;
        [SerializeField] private Transform mineBaseTransform;
        [SerializeField] private Transform baseTransform;
        [SerializeField] private Transform outDoorTransform;
        [SerializeField] private Transform ammoWarehouse;
        
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
            baseText.text = $"Base {GetLevelID}";
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
            AiSignals.Instance.onGetAmmoWarehouseTransform += OnGetAmmoWarehouseTransform;
            AiSignals.Instance.onGetTurretManagers += OnGetTurretManagers;
        }
        
        private void UnSubscribeEvents()
        {
            AiSignals.Instance.onGetMineBaseArea -= OnGetMineBaseTransform;
            AiSignals.Instance.onGetBaseAttackPoint -= OnReturnBaseAttackPoint;
            AiSignals.Instance.onGetBaseTransform -= OnGetBaseTransform;
            AiSignals.Instance.onGetAmmoWarehouseTransform -= OnGetAmmoWarehouseTransform;
            AiSignals.Instance.onGetTurretManagers -= OnGetTurretManagers;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
        
        #endregion

        #region Event Functions
        
        private Transform OnReturnBaseAttackPoint()
        {
            int randomAttackPointIndex = Random.Range(0, baseAttackPoints.Count);
            return baseAttackPoints[randomAttackPointIndex];
        }

        private Transform OnGetBaseTransform() => baseTransform;
        
        private Transform OnGetOutDoorTransform() => outDoorTransform;

        private Transform OnGetMineBaseTransform() => mineBaseTransform;

        private Transform OnGetAmmoWarehouseTransform() => ammoWarehouse;

        private List<TurretManager> OnGetTurretManagers() => turretManagers;

        #endregion
    }
}