using System;
using System.Collections.Generic;
using Abstract;
using Controllers;
using Data.UnityObject;
using Data.ValueObject.Base;
using DG.Tweening;
using Enums;
using Signals;
using Sirenix.OdinInspector;
using StateMachine;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class MineBaseManager : MonoBehaviour, ISaveable
    {
        #region Variables

        #region Public

        public MineBaseData Data;

        #endregion

        #region Serialized

        [SerializeField] private MineBaseTextController textController;
        
        [SerializeField] private int Identifier = 0;
        [Header("Resource and Gather Areas")]
        
        [SerializeField] private List<Transform> resourceAreaTransforms;

        [SerializeField] private List<Transform> gatherAreaTransforms;

        [SerializeField] private Transform stockpileAreaTransform;
        
        [Header("Gem Placement Settings")]//Datalastir
        
        [SerializeField] private Transform gemPlaceTransform;
        [SerializeField] private int maxLineAmount = 5;
        [SerializeField] private float placementDistanceXZ = 0.5f;
        [SerializeField] private float placementDistanceY = 0.2f;
        [SerializeField] private float placementDuration = .5f;
        
        #endregion

        #region Private

        [ShowInInspector] private List<Transform> collectedGemsList = new List<Transform>();
        private int _levelID;
        private int _uniqueId;
        private int _placedGemCount;
        private Vector3 _initialGemPlacePosition;
        #endregion

        #endregion
        
        private int GetLevelID => LevelSignals.Instance.onGetLevelID();
        
        private MineBaseData GetMineBaseData() => Resources.Load<CD_Level>("Data/CD_Level").Levels[GetLevelID-1].
            BaseData.MineBaseData;
        
        private void Start()
        {
            SetData();
            SetText();
            _initialGemPlacePosition = gemPlaceTransform.localPosition;
        }
        
        private void SetData()
        {
            _levelID = GetLevelID;

            _uniqueId = _levelID * 10 + Identifier;

            if (!ES3.FileExists($"MineBaseData{_uniqueId}.es3"))
            {
                if (!ES3.KeyExists("MineBaseData"))
                {
                    Data = GetMineBaseData();
                    Save(_uniqueId);
                }
            }
            Load(_uniqueId);
            // SetDataToControllers();
        }
        
        private void SetText() => textController.SetText(Data.CurrentWorkerAmount,Data.MaxWorkerAmount);

        
        #region EventSubscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            AiSignals.Instance.onGetResourceArea += OnGetResourceArea;
            AiSignals.Instance.onGetGatherArea += OnGetGatherArea;
            AiSignals.Instance.onPlaceDiamondToGatherArea += OnPlaceDiamondToGatherArea;
            PlayerSignals.Instance.onPlayerEnterDiamondArea += OnPlayerCollectAllDiamonds;
            BaseSignals.Instance.onGetMineBaseEmptySlotCount += OnGetMineBaseEmptySlotCount;
        }
        
        private void UnSubscribeEvents()
        {
            AiSignals.Instance.onGetResourceArea -= OnGetResourceArea;
            AiSignals.Instance.onGetGatherArea -= OnGetGatherArea;
            AiSignals.Instance.onPlaceDiamondToGatherArea -= OnPlaceDiamondToGatherArea;
            PlayerSignals.Instance.onPlayerEnterDiamondArea -= OnPlayerCollectAllDiamonds;
            BaseSignals.Instance.onGetMineBaseEmptySlotCount -= OnGetMineBaseEmptySlotCount;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion

        #region Event Functions

        private int OnGetMineBaseEmptySlotCount() => Data.MaxWorkerAmount - Data.CurrentWorkerAmount;

        private Transform OnGetResourceArea(MineWorkerType workerType)
        {
            if (Data.CurrentWorkerAmount == Data.MaxWorkerAmount) return null;
            
            if (workerType == MineWorkerType.Miner)
            {
                int disperseResourceArea = Data.CurrentWorkerAmount % resourceAreaTransforms.Count;
                Data.CurrentWorkerAmount++;
                SetText();
                return resourceAreaTransforms[disperseResourceArea];
            }
            
            int disperseGatherArea = Data.CurrentWorkerAmount % gatherAreaTransforms.Count;
            Data.CurrentWorkerAmount++;
            SetText();
            return gatherAreaTransforms[disperseGatherArea];
        }

        private Transform OnGetGatherArea() => stockpileAreaTransform;

        private void OnPlaceDiamondToGatherArea(GameObject diamond)
        {
            _placedGemCount++;
            diamond.transform.SetParent(stockpileAreaTransform);
            diamond.transform.rotation = Quaternion.Euler(180, 0, 0);
            var position = gemPlaceTransform.localPosition;
            collectedGemsList.Add(diamond.transform);
            diamond.transform.DOLocalMove(position, placementDuration);
            
            gemPlaceTransform.localPosition = new Vector3(position.x + placementDistanceXZ, position.y, position.z);
            if (_placedGemCount % maxLineAmount != 0) return;
            gemPlaceTransform.localPosition = new Vector3(_initialGemPlacePosition.x, position.y, position.z + placementDistanceXZ);
            if (_placedGemCount % Mathf.Pow(maxLineAmount, 2) != 0) return;
            gemPlaceTransform.localPosition = new Vector3(_initialGemPlacePosition.x, _initialGemPlacePosition.y + placementDistanceY,
                _initialGemPlacePosition.z);
        }

        private void OnPlayerCollectAllDiamonds(Transform player)
        {
            if(collectedGemsList.Count == 0) return;

            ScoreSignals.Instance.onSetDiamondAmount?.Invoke(collectedGemsList.Count);

            _placedGemCount = 0;
            gemPlaceTransform.localPosition = _initialGemPlacePosition;
            
            for (int i = 0; i < collectedGemsList.Count; i++)
            {
                Vector3 randomPos = new Vector3(Random.Range(-3, 3),Random.Range(0, 5),Random.Range(-3, 3));

                Transform gem = collectedGemsList[i];
                
                gem.SetParent(player);

                gem.DOLocalMove(gem.localPosition + randomPos, placementDuration);
                
                gem.DOLocalMove(Vector3.zero, placementDuration).SetDelay(.2f).OnComplete(()=>
                    {
                        PoolSignals.Instance.onReleasePoolObject?.Invoke(PoolType.Gem, gem.gameObject);
                        collectedGemsList.Remove(gem);
                        collectedGemsList.TrimExcess();
                    });
            }
        }
        
        #endregion


        #region Save-Load

        private void OnSave()
        {
            Save(_uniqueId);
        }

        private void OnLoad()
        {
            Load(_uniqueId);
        }

        public void Save(int uniqueId)
        {
            Data = new MineBaseData(Data.MaxWorkerAmount,Data.CurrentWorkerAmount,Data.DiamondCapacity,Data.CurrentDiamondAmount,Data.MineCardCapacity);
            
            SaveLoadSignals.Instance.onSaveMineBaseData?.Invoke(Data, uniqueId);
        }
        
        public void Load(int uniqueId)
        {
            MineBaseData data = SaveLoadSignals.Instance.onLoadMineBaseData?.Invoke(Data.Key, uniqueId);
            
            Data.MaxWorkerAmount = data.MaxWorkerAmount;
            Data.CurrentWorkerAmount = data.CurrentWorkerAmount;
            Data.DiamondCapacity = data.DiamondCapacity;
            Data.CurrentDiamondAmount = data.CurrentDiamondAmount;
            Data.MineCardCapacity = data.MineCardCapacity;
        }

        #endregion
    }
}