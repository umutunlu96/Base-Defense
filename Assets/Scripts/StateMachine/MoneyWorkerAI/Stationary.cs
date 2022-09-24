using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.MoneyWorkerAI
{
    public class Stationary : IState
    {
        
        private readonly MoneyWorkerAI _moneyWorkerAI;
        private readonly Animator _animator;
        private readonly NavMeshAgent _navMeshAgent;
        
        private static readonly int Idle = Animator.StringToHash("Idle");

        public Stationary(MoneyWorkerAI workerAI, Animator animator, NavMeshAgent agent)
        {
            _moneyWorkerAI = workerAI;
            _animator = animator;
            _navMeshAgent = agent;
        }
        
        public void Tick()
        {
            
        }

        public void OnEnter()
        {
            _navMeshAgent.enabled = false;
            _animator.SetTrigger(Idle);
        }

        public void OnExit()
        {
            _navMeshAgent.enabled = true;
        }
    }
}