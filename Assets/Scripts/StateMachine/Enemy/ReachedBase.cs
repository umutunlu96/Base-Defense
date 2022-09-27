using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.Enemy
{
    public class ReachedBase : IState
    {
        private readonly EnemyAI _enemyAI;
        private readonly Animator _animator;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly NavMeshObstacle _navMeshObstacle;
        
        private static readonly int Idle = Animator.StringToHash("Idle");
        
        public ReachedBase(EnemyAI enemyAI, Animator animator, NavMeshAgent navMeshAgent, NavMeshObstacle navMeshObstacle)
        {
            _enemyAI = enemyAI;
            _animator = animator;
            _navMeshAgent = navMeshAgent;
            _navMeshObstacle = navMeshObstacle;
        }
        
        public void Tick()
        {
            
        }

        public void OnEnter()
        {
            _animator.SetTrigger(Idle);
            _navMeshAgent.speed = 0;
            _navMeshAgent.enabled = false;
            _navMeshObstacle.enabled = true;
        }

        public void OnExit()
        {
            _navMeshObstacle.enabled = false;
            _navMeshAgent.enabled = true;
        }
    }
}