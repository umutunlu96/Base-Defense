using Enums;
using Managers;
using Signals;
using UnityEngine;

namespace Controllers
{
    public class MoneyWorkerAreaPhysicController : MonoBehaviour
    {
        public MoneyWorkerManager manager;
        [SerializeField] private float delay = 0.005f;
        private float _timer;
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if(PlayerSignals.Instance.onIsPlayerMoving.Invoke()) return;
                if (ScoreSignals.Instance.onGetMoneyAmount() < manager.Data.MoneyWorkerCost) return;
                
                _timer -= Time.deltaTime;

                if (!(_timer <= 0)) return;
                
                if (manager.Data.MoneyWorkerPayedAmount < manager.Data.MoneyWorkerCost)
                {
                    ScoreSignals.Instance.onSetMoneyAmount(-1);
                    manager.Data.MoneyWorkerPayedAmount++;
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
                manager.SetRadialFilletAmount(true);
            }
        }
    }
}