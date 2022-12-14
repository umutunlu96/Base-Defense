using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.Soldier
{
    public class Attack : IState
    {
        private readonly SoldierAI _soldierAI;
        private readonly Animator _animator;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly EnemyFinder _enemyFinder;
        
        private readonly int Speed = Animator.StringToHash("Speed");
        
        public Attack(SoldierAI soldierAI, Animator animator, NavMeshAgent navMeshAgent, EnemyFinder enemyFinder)
        {
            _soldierAI = soldierAI;
            _animator = animator;
            _navMeshAgent = navMeshAgent;
            _enemyFinder = enemyFinder;
        }
        
        public void Tick()
        {
            _animator.SetFloat(Speed,_navMeshAgent.velocity.magnitude);
        }

        public void OnEnter()
        {
            _navMeshAgent.speed = 0;
            _animator.SetFloat(Speed, _navMeshAgent.speed);
        }

        public void OnExit()
        {
            _navMeshAgent.speed = _soldierAI.RunSpeed;
            _animator.SetFloat(Speed, _navMeshAgent.speed);
        }
    }
}