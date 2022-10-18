using System.Collections;
using System.Collections.Generic;
using Abstract;
using Controllers;
using Data.UnityObject;
using Data.ValueObject;
using Data.ValueObject.Base;
using DG.Tweening;
using Enums;
using Extentions.Grid;
using Signals;
using StateMachine.TurretSoldier;
using UnityEngine;

namespace Managers
{
    public class TurretManager : MonoBehaviour, ISaveable
    {
        #region Variables

        [SerializeField] private TurretSoldierAI soldierAI;
        [SerializeField] private GridManager gridManager;
        [SerializeField] private RoomManager roomManager;
        [SerializeField] private TurretAreaPhysicController buyAreaController;
        [SerializeField] private GameObject soldier;
        [SerializeField] private Transform ammoHolderAreaTransform;
        [SerializeField] private Transform ammoHolderTransform;
        [SerializeField] private float buyDelay = 0.05f;
        
        public TurretData Data;
        
        #endregion
        
        #region Private

        private List<Transform> _ammoList = new List<Transform>();
        private Vector3 _ammoPlacerInitialPos;
        private StackType _stackType = StackType.TurretAmmmoHolder;
        private StackData _stackData;
        private int _uniqueID;
        private bool _playerEntered;
        private int _currentAmmoAmount;
        private bool _isAmmoLoadedTurret = false;
        
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
            _stackData = Resources.Load<CD_StackData>("Data/CD_StackData").StackDatas[_stackType];
        }
        
        private void CheckData()
        {
            if (Data.PayedAmount < Data.Cost) return;
            transform.GetComponent<BoxCollider>().enabled = false;
            soldier.SetActive(true); buyAreaController.gameObject.SetActive(false); Save(_uniqueID);
            soldierAI.HasSoldier = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                soldierAI.IsPlayerUsingTurret = true;
        }

        public void OnPlayerEnterBuyArea()
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

        public int GetCurrentEmptyAmmoCount() => _stackData.Capacity - _currentAmmoAmount;

        public int CurrentAmmoAmount { get => _currentAmmoAmount; set => _currentAmmoAmount += value; }
        
        public Transform AmmoHolderTransform { get => ammoHolderAreaTransform; private set => ammoHolderAreaTransform = value; }

        public void PlaceAmmoToGround(Transform ammo)
        {
            if(ammo == null) return;
            ammo.SetParent(ammoHolderTransform);
            ammo.transform.localRotation = Quaternion.Euler(0,0,0);
            ammo.DOMove(gridManager.GetPlacementVector(), .2f);
            
            _currentAmmoAmount++;
            _ammoList.Add(ammo);
            if (!_isAmmoLoadedTurret)
            {
                LoadAmmo(transform);
                _isAmmoLoadedTurret = true;
            }
        }
        public void LoadAmmo(Transform turret)
        {
            if(_currentAmmoAmount == 0) return;
            if (_ammoList.Count > 1)
                MoveAmmoToTurret(_ammoList[_ammoList.Count - 1], turret);
            if(_ammoList.Count == 1)
                MoveAmmoToTurret(_ammoList[0], turret);
        }

        private void MoveAmmoToTurret(Transform ammo, Transform turret)
        {
            _currentAmmoAmount--;
            gridManager.ReleaseObjectOnGrid();
            soldierAI.UpdateAmmo(1);
            ammo.DOJump(turret.position, 1, 1, 1f).OnComplete(() =>
            {
                ammo.transform.rotation = Quaternion.Euler(0,0,0);
                PoolSignals.Instance.onReleasePoolObject?.Invoke("Ammo", ammo.gameObject);
            });
            
            _ammoList.Remove(ammo);
            _ammoList.TrimExcess();
        }
        
        
        #region Save-Load
        
        public void Save(int uniqueId)
        {
            Data = new TurretData(Data.Cost, Data.PayedAmount, Data.AmmoCapacity);
            SaveLoadSignals.Instance.onSaveTurretData?.Invoke(Data,uniqueId);
        }

        public void Load(int uniqueId)
        {
            TurretData turretData = SaveLoadSignals.Instance.onLoadTurretData?.Invoke(Data.Key, uniqueId);
            
            if(turretData==null) return;
            Data.Cost = turretData.Cost;
            Data.PayedAmount = turretData.PayedAmount;
            Data.AmmoCapacity = turretData.AmmoCapacity;
        }
        #endregion
    }
}