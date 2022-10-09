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

        public bool CanStackAmmo
        {
            get => _canStackAmmo;
            set => _canStackAmmo = value;
        }
        
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
                StartCoroutine(StackAmmos());
            }
            
            private IEnumerator StackAmmos()
            {
                while (_ammoCount < ammoStackData.Capacity && CanStackAmmo)
                {
                    _ammoCount++;
                    ammoStack.AddStack(GetAmmo().transform);
                    yield return new WaitForSeconds(.2f);
                }
            }
            
            public void DropAmmo(TurretManager turretManager)
            {
                StartCoroutine(DropAmmunition(turretManager));
            }
            
            private IEnumerator DropAmmunition(TurretManager turretManager)
            {
                while (turretManager.GetCurrentEmptyAmmoCount() > 0 && _ammoCount > 0)
                {
                    turretManager.PlaceAmmoToGround(ammoStack.GetStackedObject());
                    _ammoCount--;
                    yield return new WaitForSeconds(.2f);
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
