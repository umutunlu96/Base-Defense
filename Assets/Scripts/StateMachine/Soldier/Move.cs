using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.Soldier
{
    public class Move : IState
    {
        private readonly SoldierAI _soldierAI;
        private readonly Animator _animator;
        private readonly NavMeshAgent _navMeshAgent;

        private readonly int Speed = Animator.StringToHash("Speed");
        
        public Move(SoldierAI soldierAI, Animator animator, NavMeshAgent navMeshAgent)
        {
            _soldierAI = soldierAI;
            _animator = animator;
            _navMeshAgent = navMeshAgent;
        }
        
        public void Tick()
        {
            _animator.SetFloat(Speed,_navMeshAgent.velocity.magnitude);
            Debug.Log("Move");
        }

        public void OnEnter()
        {
            _navMeshAgent.speed = _soldierAI.RunSpeed;
            _animator.SetFloat(Speed, _navMeshAgent.speed);
            _navMeshAgent.SetDestination(_soldierAI.enemyTarget.position);
        }

        public void OnExit()
        {
            _soldierAI.enemyTarget = null;
        }
    }
}