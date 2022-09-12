using Abstract;
using Data.UnityObject;
using Data.ValueObject.Base;
using Signals;
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
        
        //Controllers
        
        #endregion

        #region Private

        private int _levelID;
        private string _uniqueIdString;

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

            if (!ES3.FileExists($"MineBaseData{_levelID}.es3"))
            {
                if (!ES3.KeyExists("MineBaseData"))
                {
                    Data = GetMineBaseData();
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