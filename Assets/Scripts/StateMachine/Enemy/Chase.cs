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
        
        private static readonly int Run = Animator.StringToHash("Run");
        private readonly float _chaseUpdateSpeed;
        private float _timer;
        
        public Chase(EnemyAI enemyAI, Animator animator, NavMeshAgent agent, float chaseUpdateSpeed)
        {
            _enemyAI = enemyAI;
            _animator = animator;
            _navMeshAgent = agent;
            _chaseUpdateSpeed = chaseUpdateSpeed;
        }
        
        public void Tick()
        {
            Debug.Log("Chase");

            if(_enemyAI.PlayerTarget == null) return;
            _timer += Time.deltaTime;
            if (!(_timer >= _chaseUpdateSpeed)) return;
            _navMeshAgent.SetDestination(_enemyAI.PlayerTarget.position);
            _timer = 0;
        }
        
        public void OnEnter()
        {
            // Debug.Log("Chasing Player");
            _animator.SetTrigger(Run);
            _navMeshAgent.SetDestination(_enemyAI.PlayerTarget.position);
            _navMeshAgent.speed = _enemyAI.RunSpeed;
        }

        public void OnExit()
        {
            // Debug.Log("Chase End");
        }
    }
}