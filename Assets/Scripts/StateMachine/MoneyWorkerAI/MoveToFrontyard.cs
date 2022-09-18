using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.MoneyWorkerAI
{
    public class MoveToFrontyard : IState
    {
        private readonly MoneyWorkerAI _moneyWorkerAI;
        private readonly Animator _animator;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly Transform _target;

        
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Walk = Animator.StringToHash("Walk");
        
        public MoveToFrontyard(MoneyWorkerAI workerAI, Animator animator, NavMeshAgent agent, Transform target)
        {
            _moneyWorkerAI = workerAI;
            _animator = animator;
            _navMeshAgent = agent;
            _target = target;
        }
        
        public void Tick()
        {
            _animator.SetFloat(Speed,_navMeshAgent.velocity.magnitude);
        }

        public void OnEnter()
        {
            
            _navMeshAgent.enabled = true;
            _navMeshAgent.speed = _moneyWorkerAI.Speed;
            _navMeshAgent.SetDestination(_target.position);
            _animator.SetTrigger(Walk);
        }

        public void OnExit()
        {
            
        }
    }
}