using System.Collections;
using System.Collections.Generic;
using Abstract;
using Controllers;
using Data.ValueObject.Base;
using Signals;
using UnityEngine;

namespace Managers
{
    public class RoomManager : MonoBehaviour , ISaveable
    {
        [SerializeField] private TurretManager turretManager;
        [SerializeField] private RoomAreaPhysicController buyAreaController;
        [SerializeField] private List<GameObject> areaToOpen;
        [SerializeField] private List<GameObject> areaToClose;
        [SerializeField] private float buyDelay = 0.05f;
        
        public RoomData Data;
        public int Identifier = 0; //Setted by BaseManager
        private int UniqueId;
        private int LevelId;
        private bool _playerEntered;
        
        private int GetLevelID => LevelSignals.Instance.onGetLevelID();
        
        public void SetData(RoomData roomData, int identifier)
        {
            Identifier = identifier;
            UniqueId = GetLevelID * 10 + Identifier;

            if (!ES3.FileExists($"RoomData{UniqueId}.es3"))
            {
                if (!ES3.KeyExists("RoomData"))
                {
                    Data = roomData;
                    Save(UniqueId);
                }
            }
            Load(UniqueId);
            CheckData();
            turretManager.SetData();
        }

        public int ReturnUniqueId() => UniqueId;

        private void CheckData()
        {
            if (Data.PayedAmount < Data.Cost) return;
            foreach (var area in areaToOpen)
            {
                area.SetActive(true);
            }
            foreach (var area in areaToClose)
            {
                area.SetActive(false);
            }
            buyAreaController.gameObject.SetActive(false);
            Save(UniqueId);
        }
        
        public void OnPlayerEnter()
        {
            int playerMoney = ScoreSignals.Instance.onGetMoneyAmount();
            int moneyToPay = Data.Cost - Data.PayedAmount;
            
            if(PlayerSignals.Instance.onIsPlayerMoving() && playerMoney < Data.Cost) return;
            if (playerMoney >= Data.Cost && moneyToPay > 0)
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
                ScoreSignals.Instance.onSetMoneyAmount?.Invoke(-1);
                yield return new WaitForSeconds(buyDelay);
            }
        }
        

        #region Save-Load

        public void OnSave(TurretData turretData)
        {
            Data.TurretData = turretData;
            Save(UniqueId);
        }

        public void OnLoad()
        {
            Load(UniqueId);
        }
        
        public void Save(int uniqueId)
        {
            Data = new RoomData(Data.Cost, Data.PayedAmount, Data.TurretData);
            SaveLoadSignals.Instance.onSaveRoomData?.Invoke(Data,uniqueId);
        }

        public void Load(int uniqueId)
        {
            RoomData roomData = SaveLoadSignals.Instance.onLoadRoomData?.Invoke(Data.Key,uniqueId);
            Data.Cost = roomData.Cost;
            Data.PayedAmount = roomData.PayedAmount;
            Data.TurretData = roomData.TurretData;
            turretManager.Data = Data.TurretData;
        }
        #endregion
    }
}