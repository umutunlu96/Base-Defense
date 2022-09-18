using System.Collections;
using System.Threading.Tasks;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.MoneyWorkerAI
{
    //navmesh random point finder
    public class Search : IState
    {
        private readonly MoneyWorkerAI _moneyWorkerAI;
        private readonly MoneyFinder _moneyFinder;
        private float _delay = .1f;
        private float _timer;
        
        public Search(MoneyWorkerAI workerAI, MoneyFinder moneyFinder)
        {
            _moneyWorkerAI = workerAI;
            _moneyFinder = moneyFinder;
        }
        
        public void Tick()
        {
            _timer += Time.deltaTime;
            if (_moneyWorkerAI.MoneyTransform == null && _timer > _delay)
            {
                _moneyFinder.IncreaseRaius();
                _timer = 0;
                if (_moneyFinder.Radius > 150)
                    _moneyWorkerAI.CantFindMoney = true;
            }
        }

        public void OnEnter()
        {
            _moneyFinder.gameObject.SetActive(true);
        }
        
        public void OnExit()
        {
            _moneyFinder.gameObject.SetActive(false);
            _moneyFinder.ResetRadius();
        }
    }
}