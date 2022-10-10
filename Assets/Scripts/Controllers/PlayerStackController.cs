using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Managers;
using Signals;
using StateMachine;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Controllers
{
    public class PlayerStackController : MonoBehaviour
    {
        [SerializeField] private CollectableStackManager ammoStack;
        [SerializeField] private CollectableStackManager moneyStack;

        private StackData ammoStackData;
        private StackData moneyStackData;

        private int _ammoCount;
        private int _moneyCount;

        private bool _canStackAmmo;
        private bool _canDropAmmo;

        public bool CanStackAmmo {get => _canStackAmmo; set => _canStackAmmo = value;}
        
        public bool CanDropAmmo {get => _canDropAmmo; set => _canDropAmmo = value;}
        
        private void Start()
        {
            InitializeDatas();
        }

        private void InitializeDatas()
        {
            ammoStackData = Resources.Load<CD_StackData>("Data/CD_StackData").StackDatas[StackType.PlayerAmmo];
            moneyStackData = Resources.Load<CD_StackData>("Data/CD_StackData").StackDatas[StackType.PlayerMoney];
            ammoStack.SetStackData(ammoStackData);
            moneyStack.SetStackData(moneyStackData);
        }

        #region Money

        public void StackMoney(Transform money)
        {
            if(_moneyCount == moneyStackData.Capacity) return;
            moneyStack.AddStack(money);
            _moneyCount++;
            money.tag = "Collected";
        }

        public void DropAllMoney()
        {
            ScoreSignals.Instance.onSetMoneyAmount?.Invoke(_moneyCount * 10);
            _moneyCount = 0;
            moneyStack.RemoveStackAll();
        }
        
        #endregion

        #region Ammo

        private GameObject GetAmmo() => PoolSignals.Instance.onGetPoolObject?.Invoke("Ammo", transform);

        public void StackAmmo() 
        {
            CanStackAmmo = true;
            StackAmmos();
        }
            
        private async void StackAmmos()
        {
            if(_ammoCount < ammoStackData.Capacity && CanStackAmmo)
            { 
                _ammoCount++; 
                ammoStack.AddStack(GetAmmo().transform);
                await Task.Delay(200);
                StackAmmos();
            }
        }
            
        public void DropAmmo(TurretManager turretManager)
        {
            CanDropAmmo = true;
            DropAmmunition(turretManager);
        }
            
        private async void DropAmmunition(TurretManager turretManager)
        {
            if(turretManager.GetCurrentEmptyAmmoCount() > 0 && _ammoCount > 0 && CanDropAmmo)
            {
                turretManager.PlaceAmmoToGround(ammoStack.GetStackedObject());
                _ammoCount--;
                await Task.Delay(200);
                DropAmmunition(turretManager);
            }
        }
            
        public void DropAllAmmoToGround() 
        {
            if(_ammoCount <= 0) return;
            _ammoCount = 0;
            ammoStack.RemoveStackAll();
        }
        
        #endregion
    }
}
