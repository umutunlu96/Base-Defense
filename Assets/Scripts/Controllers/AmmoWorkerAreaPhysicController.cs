using Abstract;
using Enums;
using Managers;
using Signals;
using UnityEngine;

namespace Controllers
{
    public class AmmoWorkerAreaPhysicController : MonoBehaviour
    {
        [SerializeField] private AmmoWorkerManager manager;
        [SerializeField] private float delay = 0.005f;
        private float _timer;
        
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (PlayerSignals.Instance.onIsPlayerMoving()) return;
                if (ScoreSignals.Instance.onGetMoneyAmount() < manager.Data.AmmoWorkerCost) return;
                
                _timer -= Time.deltaTime;

                if (!(_timer <= 0)) return;
                if (manager.Data.AmmoWorkerPayedAmount < manager.Data.AmmoWorkerCost)
                {
                    ScoreSignals.Instance.onSetMoneyAmount(-1);
                    manager.Data.AmmoWorkerPayedAmount++;
                    manager.UpdatePayedAmountText();
                    manager.CheckPayedAmount();
                    manager.SetRadialFilletAmount(false);
                }
                else
                {
                    manager.Data.BuyState = BuyState.Bought;
                }
                _timer = delay;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                manager.Save(manager.UniqueId);
            }
        }
    }
}