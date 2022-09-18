using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.MoneyWorkerAI
{
    public class MoveToMoney : IState
    {
        private readonly MoneyWorkerAI _moneyWorkerAI;
        private readonly Animator _animator;
        private readonly NavMeshAgent _navMeshAgent;

        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Walk = Animator.StringToHash("Walk");
        
        public MoveToMoney(MoneyWorkerAI workerAI, Animator animator, NavMeshAgent agent)
        {
            _moneyWorkerAI = workerAI;
            _animator = animator;
            _navMeshAgent = agent;
        }
        
        public void Tick()
        {
            _animator.SetFloat(Speed,_navMeshAgent.velocity.magnitude);
        }

        public void OnEnter()
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.speed = _moneyWorkerAI.Speed;
            _navMeshAgent.SetDestination(_moneyWorkerAI.MoneyTransform.position);
            _animator.SetTrigger(Walk);
        }

        public void OnExit()
        {
            
        }
    }
}