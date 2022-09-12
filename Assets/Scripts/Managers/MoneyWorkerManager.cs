using Abstract;
using Data.UnityObject;
using Data.ValueObject.Base;
using Signals;
using UnityEngine;

namespace Managers
{
    public class MoneyWorkerManager : MonoBehaviour, ISaveable
    {
        #region Variables

        #region Public

        public MoneyWorkerData Data;
        
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
        
        private MoneyWorkerData GetMoneyWorkerData() => Resources.Load<CD_Level>("Data/CD_Level").Levels[GetLevelID-1].
            BaseData.MoneyWorkerData;
        
        private void Start()
        {
            SetData();
        }
        
        private void SetData()
        {
            _levelID = GetLevelID;

            if (!ES3.FileExists($"MoneyWorkerData{_levelID}.es3"))
            {
                if (!ES3.KeyExists("MoneyWorkerData"))
                {
                    Data = GetMoneyWorkerData();
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
            Data = new MoneyWorkerData(Data.BuyState,Data.MoneyWorkerCost,Data.MoneyWorkerPayedAmount,Data.MoneyWorkerLevel);
            
            SaveLoadSignals.Instance.onSaveMoneyWorkerData?.Invoke(Data, uniqueId);
        }
        
        public void Load(int uniqueId)
        {
            MoneyWorkerData data = SaveLoadSignals.Instance.onLoadMoneyWorkerData?.Invoke(Data.Key, uniqueId);
            
            Data.BuyState = data.BuyState;
            Data.MoneyWorkerCost = data.MoneyWorkerCost;
            Data.MoneyWorkerPayedAmount = data.MoneyWorkerPayedAmount;
            Data.MoneyWorkerLevel = data.MoneyWorkerLevel;
        }

        #endregion
    }
}