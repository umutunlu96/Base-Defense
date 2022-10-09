using System.Collections;
using System.Threading.Tasks;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.MoneyWorkerAI
{
    public class Search : IState
    {
        private readonly MoneyWorkerAI _moneyWorkerAI;
        private readonly MoneyFinder _moneyFinder;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly Animator _animator;
        
        private static readonly int Speed = Animator.StringToHash("Speed");
        private float _delay = .1f;
        private float _timer;
        
        public Search(MoneyWorkerAI workerAI, MoneyFinder moneyFinder, NavMeshAgent navMeshAgent, Animator animator)
        {
            _moneyWorkerAI = workerAI;
            _moneyFinder = moneyFinder;
            _navMeshAgent = navMeshAgent;
            _animator = animator;
        }
        
        public void Tick()
        {
            _animator.SetFloat(Speed,_navMeshAgent.velocity.magnitude);
            _timer += Time.deltaTime;
            if (_moneyWorkerAI.MoneyTransform == null && _timer > _delay)
            {
                _moneyFinder.IncreaseRaius();
                _timer = 0;
                if (_moneyFinder.Radius > 150)
                {
                    _moneyWorkerAI.CantFindMoney = true;
                }
            }
        }

        public void OnEnter()
        {
            _moneyWorkerAI.CantFindMoney = false;
            _moneyFinder.gameObject.SetActive(true);
        }
        
        public void OnExit()
        {
            _moneyFinder.gameObject.SetActive(false);
            _moneyFinder.ResetRadius();
        }
    }
}