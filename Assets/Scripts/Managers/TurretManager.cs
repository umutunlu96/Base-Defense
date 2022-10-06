﻿using System.Collections;
using System.Collections.Generic;
using Abstract;
using Controllers;
using Data.UnityObject;
using Data.ValueObject;
using Data.ValueObject.Base;
using DG.Tweening;
using Enums;
using Signals;
using StateMachine.TurretSoldier;
using UnityEngine;

namespace Managers
{
    public class TurretManager : MonoBehaviour, ISaveable
    {
        #region Variables

        [SerializeField] private TurretSoldierAI _soldierAI;
        [SerializeField] private Transform ammoPlacerTransform;
        [SerializeField] private RoomManager roomManager;
        [SerializeField] private TurretAreaPhysicController buyAreaController;
        [SerializeField] private GameObject soldier;
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
            
            var localPosition = ammoPlacerTransform.localPosition;
            _ammoPlacerInitialPos = new Vector3(localPosition.x, localPosition.y, localPosition.z);
            _stackData = Resources.Load<CD_StackData>("Data/CD_StackData").StackDatas[_stackType];
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
        
        public Transform AmmoHolderTransform { get => ammoHolderTransform; private set => ammoHolderTransform = value; }

        public void PlaceAmmoToGround(Transform ammo)
        {
            if(ammo == null) return;
            ammo.SetParent(ammoHolderTransform);
            ammo.transform.localRotation = Quaternion.Euler(-90,0,0);
            ammo.DOLocalMove(ammoPlacerTransform.localPosition, _stackData.LerpSpeed).SetDelay(.1f);
            ammoPlacerTransform.localPosition = new Vector3(ammoPlacerTransform.localPosition.x - _stackData.OffsetY, ammoPlacerTransform.localPosition.y, 0);
            
            _currentAmmoAmount++;
            _ammoList.Add(ammo);
            _soldierAI.UpdateAmmo(1);
            
            if (_currentAmmoAmount % 3 == 0)
            {
                ammoPlacerTransform.localPosition = new Vector3(_ammoPlacerInitialPos.x,
                    ammoPlacerTransform.localPosition.y +_stackData.OffsetZ, 0);
            }
        }

        public void GetAmmo(Transform turret)
        {
            Sequence sequence = DOTween.Sequence();
            Transform ammo;
            _currentAmmoAmount--;
            if (_ammoList.Count > 1)
            {
                ammo = _ammoList[_ammoList.Count - 1];
                ammo.SetParent(null);
            }
            else
            {
                ammo = _ammoList[0];
                ammo.SetParent(null);
            }

            sequence.Append(ammo.DOMove(new Vector3(0, 2, 0), .2f));
            sequence.Join(ammo.DORotate(new Vector3(0, 0, 45), .2f));
            sequence.Append(ammo.DOMove(turret.position, .2f)).OnComplete(() =>
            {
                ammo.transform.rotation = Quaternion.Euler(0,0,0);
                PoolSignals.Instance.onReleasePoolObject?.Invoke("Ammo", ammo.gameObject);
            });

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