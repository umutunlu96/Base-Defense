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
            _timer += Time.deltaTime;

            if (_timer >= 5)
            {
                AttackToTarget();
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
            _navMeshObstacle.enabled = false;
            _navMeshAgent.enabled = true;
            _animator.SetTrigger(attack);
            Debug.Log("Attack");
            //Playersignals.instance.onPlayerGetDamage?.Invoke(_enemyAi.Damage);
            _navMeshAgent.enabled = false;
            _navMeshObstacle.enabled = true;
            _timer = 0;
        }
    }
}