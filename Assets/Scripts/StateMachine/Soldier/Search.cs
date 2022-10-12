using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.Soldier
{
    public class Search : IState
    {
        private readonly SoldierAI _soldierAI;
        private readonly Animator _animator;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly EnemyFinder _enemyFinder;
        private float _timer;
        
        private readonly int Speed = Animator.StringToHash("Speed");
        
        public Search(SoldierAI soldierAI, Animator animator, NavMeshAgent navMeshAgent, EnemyFinder enemyFinder)
        {
            _soldierAI = soldierAI;
            _animator = animator;
            _navMeshAgent = navMeshAgent;
            _enemyFinder = enemyFinder;
        }
        
        public void Tick()
        {
            Debug.Log("Search");

            _animator.SetFloat(Speed,_navMeshAgent.velocity.magnitude);
            _timer += Time.deltaTime;
            if (_soldierAI.enemyTarget == null && _timer > _soldierAI.ChaseUpdateSpeed)
            {
                _enemyFinder.IncreaseRaius();
                _timer = 0;
                if (_enemyFinder.Radius > 50)
                {
                    _enemyFinder.ResetRadius();
                }
            }
        }

        public void OnEnter()
        {
            
        }

        public void OnExit()
        {
            
        }
    }
}