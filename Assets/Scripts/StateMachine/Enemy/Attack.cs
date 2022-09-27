using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.Enemy
{
    public class Attack : IState
    {
        private readonly EnemyAI _enemyAI;
        private readonly Animator _animator;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly NavMeshObstacle _navMeshObstacle;
        
        private static readonly int attack = Animator.StringToHash("Attack");
        private float _timer;

        public Attack(EnemyAI enemyAI, Animator animator, NavMeshAgent navMeshAgent, NavMeshObstacle navMeshObstacle)
        {
            _enemyAI = enemyAI;
            _animator = animator;
            _navMeshAgent = navMeshAgent;
            _navMeshObstacle = navMeshObstacle;
        }
        
        public void Tick()
        {
            _enemyAI.AttackedToPlayer();
            
            if (_enemyAI.AttackAnimEnded)
            {
                AttackToTarget();
                _enemyAI.AttackAnimEnded = false;
            }
        }

        public void OnEnter()
        {
            AttackToTarget();
        }

        public void OnExit()
        {
            
        }

        private void AttackToTarget()
        {
            _animator.SetTrigger(attack);
            // Debug.Log("Attack");
        }
    }
}