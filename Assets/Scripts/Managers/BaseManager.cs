using System.Collections.Generic;
using Data.UnityObject;
using Data.ValueObject.Base;
using Signals;
using StateMachine;
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
        public FrontYardData FrontYardData;
        
        #endregion

        #region Serialized

        [SerializeField] private GameObject portal;
        [SerializeField] private List<RoomManager> roomManagers;
        [SerializeField] private List<TurretManager> turretManagers;
        [SerializeField] private List<Transform> baseAttackPoints;
        [SerializeField] private List<ForceFieldManager> forceFieldManagers;

        [SerializeField] private TextMeshPro baseText;
        [SerializeField] private Transform mineBaseTransform;
        [SerializeField] private Transform baseTransform;
        [SerializeField] private Transform outDoorTransform;
        [SerializeField] private Transform ammoWarehouse;
        [SerializeField] private Transform outsideTransform;
        
        
        #endregion

        #region Private

        private int _levelID;
        private int _uniqueId;

        #endregion

        #endregion
        
        private int GetLevelID => LevelSignals.Instance.onGetLevelCount();
        
        private void GetBaseData() => BaseData =  Resources.Load<CD_Level>("Data/CD_Level").Levels[GetLevelID - 1].
            BaseData;

        private void GetFrontyardData() =>
            FrontYardData = Resources.Load<CD_Level>("Data/CD_Level").Levels[GetLevelID - 1].
                frontYardData;

        private void Start()
        {
            GetBaseData();
            GetFrontyardData();
            SetDataToManagers();
        }
        
        private void SetDataToManagers()
        {
            for (int i = 0; i < roomManagers.Count; i++)
            {
                roomManagers[i].SetData(BaseData.BaseRoomData.RoomDatas[i], i);
            }

            for (int i = 0; i < forceFieldManagers.Count; i++)
            {
                forceFieldManagers[i].SetData(FrontYardData.ForceFieldData[i], i);
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
            AiSignals.Instance.onGetOutsideTransform += OnGetOutsideTransform;
            AiSignals.Instance.onBossDead += OnBossDead;
        }
        
        private void UnSubscribeEvents()
        {
            AiSignals.Instance.onGetMineBaseArea -= OnGetMineBaseTransform;
            AiSignals.Instance.onGetBaseAttackPoint -= OnReturnBaseAttackPoint;
            AiSignals.Instance.onGetBaseTransform -= OnGetBaseTransform;
            AiSignals.Instance.onGetAmmoWarehouseTransform -= OnGetAmmoWarehouseTransform;
            AiSignals.Instance.onGetTurretManagers -= OnGetTurretManagers;
            AiSignals.Instance.onGetOutsideTransform -= OnGetOutsideTransform;
            AiSignals.Instance.onBossDead += OnBossDead;
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

        private Transform OnGetOutsideTransform() => outsideTransform;

        private void OnBossDead() => portal.SetActive(true);

        #endregion
    }
}