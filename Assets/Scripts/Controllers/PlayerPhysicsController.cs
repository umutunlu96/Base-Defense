using System;
using Enums;
using ES3Types;
using Managers;
using Signals;
using StateMachine;
using UnityEngine;

namespace Controllers
{
    public class PlayerPhysicsController : MonoBehaviour
    {
        [SerializeField] PlayerManager _manager;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Hostage"))
            {
                other.gameObject.tag = "Rescued";
                StackSignals.Instance.onAddStack?.Invoke(other.transform.parent);
            }

            if (other.CompareTag("MineArea"))
            {
                PlayerSignals.Instance.onPlayerEnterMineArea?.Invoke();
            }

            if (other.CompareTag("GateOutside"))
            {
                _manager.OnExitBase();
            }

            if (other.CompareTag("GateInside"))
            {
                _manager.OnEnterBase();
                _manager.DropMoneyToBase();
            }
            
            if (other.CompareTag("StockpileSpot"))
            {
                PlayerSignals.Instance.onPlayerEnterDiamondArea?.Invoke(_manager.transform);
            }

            if (other.CompareTag("Turret"))
            {
                PlayerSignals.Instance.onPlayerEnterTurretArea?.Invoke();
            }
            
            if (other.CompareTag("Money"))
            {
                _manager.StackMoney(other.transform);
            }
            
            if(other.CompareTag("AmmoWarehouse"))
            {
                _manager.StackAmmo();
            }
            
            if (other.CompareTag("TurretAmmoHolder"))
            {
                if(other.transform.parent.TryGetComponent(out TurretManager turretManager))
                {
                    _manager.DropAmmoToTurret(turretManager);
                }
            }

            if (other.CompareTag("Boss"))
            {
                AiSignals.Instance.onPlayerEnterBossArea?.Invoke();
                print("Player Entered Boss Area");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("AmmoWarehouse"))
            {
                _manager.StopStackAmmo();
            }
            
            if (other.CompareTag("TurretAmmoHolder"))
            {
                _manager.StopDropAmmoToTurret();
            }
            
            if (other.CompareTag("Boss"))
            {
                AiSignals.Instance.onPlayerLeaveBossArea?.Invoke();
                print("Player Exited Boss Area");
            }
        }
    }
}