using System.Collections;
using System.Threading.Tasks;
using Abstract;
using Controllers;
using Data.ValueObject.Base;
using Signals;
using UnityEngine;

namespace Managers
{
    public class TurretManager : MonoBehaviour, ISaveable
    {
        #region Variables

        [SerializeField] private RoomManager roomManager;
        [SerializeField] private TurretAreaPhysicController buyAreaController;
        [SerializeField] private GameObject soldier;
        [SerializeField] private float buyDelay = 0.05f;

        public TurretData Data;
        
        #endregion
        
        #region Private

        private int _levelID;
        private int _uniqueID;
        private bool _playerEntered;

        #endregion

        public void SetData()
        {
            _uniqueID = roomManager.ReturnUniqueId();
            
            if (!ES3.FileExists($"TurretData{_uniqueID}.es3"))
            {
                if (!ES3.KeyExists("TurretData"))
                {
                    Save(_uniqueID);
                }
            }
            Load(_uniqueID);
            CheckData();
        }
        
        
        private void CheckData()
        {
            if (Data.PayedAmount < Data.Cost) return;
            soldier.SetActive(true); buyAreaController.gameObject.SetActive(false); Save(_uniqueID);
        }
        
        public void OnPlayerEnter()
        {
            int playerMoney = ScoreSignals.Instance.onGetMoneyAmount();
            int moneyToPay = Data.Cost - Data.PayedAmount;
            
            if(!PlayerSignals.Instance.canBuy() && playerMoney < Data.Cost) return;
            if (playerMoney >= Data.Cost && moneyToPay > 0)
            {
                if (!_playerEntered)
                {
                    _playerEntered = true;
                    StartCoroutine(Buy());
                }
            }
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

        public void OnPlayerExit()
        {
            _playerEntered = false;
            StopCoroutine(Buy());
            roomManager.OnSave(Data);
        }

        #region Save-Load
        
        public void Save(int uniqueId)
        {
            Data = new TurretData(Data.Cost, Data.PayedAmount, Data.AmmoCapacity, Data.AmmoDamage);
            SaveLoadSignals.Instance.onSaveTurretData?.Invoke(Data,uniqueId);
        }

        public void Load(int uniqueId)
        {
            TurretData turretData = SaveLoadSignals.Instance.onLoadTurretData?.Invoke(Data.Key, uniqueId);
            
            // if(turretData==null) return;
            Data.Cost = turretData.Cost;
            Data.PayedAmount = turretData.PayedAmount;
            Data.AmmoCapacity = turretData.AmmoCapacity;
            Data.AmmoDamage = turretData.AmmoDamage;
        }
        #endregion
    }
}