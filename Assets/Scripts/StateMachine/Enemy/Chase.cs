using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.Enemy
{
    public class Chase : IState
    {
        private readonly EnemyAI _enemyAI;
        private readonly Animator _animator;
        private readonly NavMeshAgent _navMeshAgent;
        
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Run = Animator.StringToHash("Run");
        private float _timer;
        
        public Chase(EnemyAI enemyAI, Animator animator, NavMeshAgent agent)
        {
            _enemyAI = enemyAI;
            _animator = animator;
            _navMeshAgent = agent;
        }
        
        public void Tick()
        {
            if(Vector3.Distance(_enemyAI.transform.position, _enemyAI.CurrentTarget.position) < 1f) return;
            _navMeshAgent.SetDestination(_enemyAI.PlayerTarget.position);
            _animator.SetFloat(Speed, _navMeshAgent.velocity.magnitude);
        }
        
        public void OnEnter()
        {
            _animator.SetTrigger(Run);
            _navMeshAgent.SetDestination(_enemyAI.PlayerTarget.position);
            _navMeshAgent.speed = _enemyAI.RunSpeed;
        }

        public void OnExit()
        {
            _navMeshAgent.speed = _enemyAI.WalkSpeed;
        }
    }
}