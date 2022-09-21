using System.Collections.Generic;
using Abstract;
using Data.UnityObject;
using Data.ValueObject.Base;
using Signals;
using StateMachine;
using UnityEngine;

namespace Managers
{
    public class MineBaseManager : MonoBehaviour, ISaveable
    {
        #region Variables

        #region Public

        public MineBaseData Data;

        #endregion

        #region Serialized
        [SerializeField] private int Identifier = 0;

        [SerializeField] private List<Transform> resourceArea;
        [SerializeField] private Transform gatherArea;
        
        
        
        #endregion

        #region Private

        private int _levelID;
        private int _uniqueId;

        #endregion

        #endregion
        
        private int GetLevelID => LevelSignals.Instance.onGetLevelID();
        
        private MineBaseData GetMineBaseData() => Resources.Load<CD_Level>("Data/CD_Level").Levels[GetLevelID-1].
            BaseData.MineBaseData;
        
        private void Start()
        {
            SetData();
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
        
        #region EventSubscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            AiSignals.Instance.onGetResourceArea += OnGetResourceArea;
            AiSignals.Instance.onGetGatherArea += OnGetGatherArea;
        }
        
        private void UnSubscribeEvents()
        {
            AiSignals.Instance.onGetResourceArea -= OnGetResourceArea;
            AiSignals.Instance.onGetGatherArea -= OnGetGatherArea;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion

        #region Event Functions

        private Transform OnGetResourceArea()
        {
            int disperseWorker = Data.CurrentWorkerAmount % resourceArea.Count;
            Data.CurrentWorkerAmount++;
            return resourceArea[disperseWorker];
        }

        private Transform OnGetGatherArea() => gatherArea;
        
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