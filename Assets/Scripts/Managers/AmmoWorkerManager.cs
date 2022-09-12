using System;
using Abstract;
using Data.UnityObject;
using Data.ValueObject.Base;
using Signals;
using UnityEngine;

namespace Managers
{
    public class AmmoWorkerManager : MonoBehaviour, ISaveable
    {
        #region Variables

        #region Public

        public AmmoWorkerData Data;
        
        #endregion

        #region Serialized
        
        //Controllers
        
        #endregion

        #region Private

        private int _levelID;
        private string _uniqueIdString;

        #endregion

        #endregion
        
        private int GetLevelID => LevelSignals.Instance.onGetLevelID();
        
        private AmmoWorkerData GetAmmoWorkerData() => Resources.Load<CD_Level>("Data/CD_Level").Levels[GetLevelID-1].
            BaseData.AmmoWorkerData;
        
        private void Start()
        {
            SetData();
        }
        
        private void SetData()
        {
            _levelID = GetLevelID;

            if (!ES3.FileExists($"AmmoWorkerData{_levelID}.es3"))
            {
                if (!ES3.KeyExists("AmmoWorkerData"))
                {
                    Data = GetAmmoWorkerData();
                    Save(_levelID);
                }
            }
            Load(_levelID);
            // SetDataToControllers();
        }
        
        #region Save-Load

        private void OnSave()
        {
            Save(_levelID);
        }

        private void OnLoad()
        {
            Load(_levelID);
        }

        public void Save(int uniqueId)
        {
            Data = new AmmoWorkerData(Data.BuyState,Data.AmmoWorkerCost,Data.AmmoWorkerPayedAmount,Data.AmmoWorkerLevel);
            
            SaveLoadSignals.Instance.onSaveAmmoWorkerData?.Invoke(Data, uniqueId);
        }
        
        public void Load(int uniqueId)
        {
            AmmoWorkerData data = SaveLoadSignals.Instance.onLoadAmmoWorkerData?.Invoke(Data.Key, uniqueId);
            
            Data.BuyState = data.BuyState;
            Data.AmmoWorkerCost = data.AmmoWorkerCost;
            Data.AmmoWorkerLevel = data.AmmoWorkerLevel;
            Data.AmmoWorkerPayedAmount = data.AmmoWorkerPayedAmount;
        }

        #endregion
    }
}