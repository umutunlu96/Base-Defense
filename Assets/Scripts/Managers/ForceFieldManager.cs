using System.Collections;
using Abstract;
using Controllers;
using Data.ValueObject.Base;
using Signals;
using UnityEngine;

namespace Managers
{
    public class ForceFieldManager : MonoBehaviour, ISaveable
    {
        [SerializeField] private ForceFieldPhysicController buyAreaController;
        [SerializeField] private GameObject areaToClose;
        [SerializeField] private float buyDelay = 0.05f;
        [HideInInspector] public ForceFieldData Data;

        public int Identifier = 0; //Setted by BaseManager
        private int UniqueId;
        private int LevelId;
        private bool _playerEntered;
        
        private int GetLevelID => LevelSignals.Instance.onGetLevelID();
        
        public void SetData(ForceFieldData forceFieldData, int identifier)
        {
            Identifier = identifier;
            UniqueId = GetLevelID * 10 + Identifier;

            if (!ES3.FileExists($"ForceFieldData{UniqueId}.es3"))
            {
                if (!ES3.KeyExists("ForceFieldData"))
                {
                    Data = forceFieldData;
                    Save(UniqueId);
                }
            }
            Load(UniqueId);
            CheckData();
        }

        private void CheckData()
        {
            buyAreaController.UpdatePayedAmountText(Data.PayedAmount, Data.Cost);
            if (Data.PayedAmount < Data.Cost) return;
            areaToClose.SetActive(false); buyAreaController.gameObject.SetActive(false); Save(UniqueId);
        }
        
        public void OnPlayerEnter()
        {
            int playerDiamond = ScoreSignals.Instance.onGetDiamondAmount();
            int diamondToPay = Data.Cost - Data.PayedAmount;
            
            if(PlayerSignals.Instance.onIsPlayerMoving() && playerDiamond < Data.Cost) return;
            if (playerDiamond >= Data.Cost && diamondToPay > 0)
            {
                if (!_playerEntered)
                {
                    _playerEntered = true;
                    StartCoroutine(Buy());
                }
            }
        }

        public void OnPlayerExit()
        {
            _playerEntered = false;
            StopCoroutine(Buy());
            Save(UniqueId);
        }
        
        private IEnumerator Buy()
        {
            while (_playerEntered)
            {
                int moneyToPay = Data.Cost - Data.PayedAmount;
                if(moneyToPay == 0)
                {
                    CheckData();
                    break;
                }
                Data.PayedAmount++;
                buyAreaController.UpdatePayedAmountText(Data.PayedAmount,Data.Cost);
                buyAreaController.SetRadialFilletAmount(false,Data.PayedAmount,Data.Cost);
                ScoreSignals.Instance.onSetDiamondAmount?.Invoke(-1);
                yield return new WaitForSeconds(buyDelay);
            }
        }
        
        #region Save-Load

        public void OnSave()
        {
            Save(UniqueId);
        }

        public void OnLoad()
        {
            Load(UniqueId);
        }
        
        public void Save(int uniqueId)
        {
            Data = new ForceFieldData(Data.Cost, Data.PayedAmount);
            SaveLoadSignals.Instance.onSaveForceFieldData?.Invoke(Data,uniqueId);
        }

        public void Load(int uniqueId)
        {
            ForceFieldData forceFieldData = SaveLoadSignals.Instance.onLoadForceFieldData?.Invoke(Data.Key,uniqueId);
            Data.Cost = forceFieldData.Cost;
            Data.PayedAmount = forceFieldData.PayedAmount;
        }
        #endregion
    }
}