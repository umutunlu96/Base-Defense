using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.Enemy
{
    public class MoveGroundMine : IState
    {
        private readonly EnemyAI _enemyAI;
        private readonly Animator _animator;
        private readonly NavMeshAgent _navMeshAgent;
        
        private static readonly int Speed = Animator.StringToHash("Speed");

        public MoveGroundMine(EnemyAI enemyAI, Animator animator, NavMeshAgent navMeshAgent)
        {
            _enemyAI = enemyAI;
            _animator = animator;
            _navMeshAgent = navMeshAgent;
        }
        
        public void Tick()
        {
            _animator.SetFloat(Speed,_navMeshAgent.speed);
        }

        public void OnEnter()
        {
            _navMeshAgent.speed = _enemyAI.RunSpeed;
            _navMeshAgent.SetDestination(_enemyAI.GroundMineTarget.position);
        }

        public void OnExit()
        {
            
        }
    }
}