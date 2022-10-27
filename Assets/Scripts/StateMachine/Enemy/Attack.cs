using Signals;
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
        private readonly int _damage;
        
        private static readonly int attack = Animator.StringToHash("Attack");
        private static readonly int Speed = Animator.StringToHash("Speed");

        private float _timer = 1.1f;
        public Attack(EnemyAI enemyAI, Animator animator, NavMeshAgent navMeshAgent, NavMeshObstacle navMeshObstacle, int damage)
        {
            _enemyAI = enemyAI;
            _animator = animator;
            _navMeshAgent = navMeshAgent;
            _navMeshObstacle = navMeshObstacle;
            _damage = damage;
        }
        
        public void Tick()
        {
            Debug.Log("ATTACK");

            _timer -= Time.deltaTime;
            if(_timer >= 0) return;
            
            _animator.SetTrigger(attack);
            AttackToTarget();
            // _animator.SetFloat(Speed, 0);
            _timer = 1.1f;
        }

        public void OnEnter()
        {
            _navMeshAgent.velocity = Vector3.zero;
            AttackToTarget();
        }

        public void OnExit()
        {
            _animator.SetFloat(Speed, _navMeshAgent.speed);
        }

        private void AttackToTarget()
        {
            _animator.SetTrigger(attack);
            if(Vector3.Distance(_enemyAI.transform.position, PlayerSignals.Instance.onGetPlayerTransfrom().position) <= _enemyAI.AttackRange)
                PlayerSignals.Instance.onTakeDamage?.Invoke(_damage);
        }
    }
}