using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.Enemy
{
    public class Move : IState
    {
        private readonly EnemyAI _enemyAI;
        private readonly Animator _animator;
        private readonly NavMeshAgent _navMeshAgent;
        
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Walk = Animator.StringToHash("Walk");
        
        public Move(EnemyAI enemyAI,Animator animator, NavMeshAgent agent)
        {
            _enemyAI = enemyAI;
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
            _navMeshAgent.speed = _enemyAI.WalkSpeed;
            _navMeshAgent.SetDestination(_enemyAI.CurrentTarget.position);
            _animator.SetTrigger(Walk);
        }

        public void OnExit()
        {

        }
    }
}