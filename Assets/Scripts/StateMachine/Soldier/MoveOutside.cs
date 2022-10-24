using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.Soldier
{
    public class MoveOutside : IState
    {
        private readonly SoldierAI _soldierAI;
        private readonly Animator _animator;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly Transform _outsideTransform;
        
        private readonly int Speed = Animator.StringToHash("Speed");
        
        public MoveOutside(SoldierAI soldierAI, Animator animator, NavMeshAgent navMeshAgent, Transform outsideTransform)
        {
            _soldierAI = soldierAI;
            _animator = animator;
            _navMeshAgent = navMeshAgent;
            _outsideTransform = outsideTransform;
        }
        
        public void Tick()
        {
            _animator.SetFloat(Speed, _navMeshAgent.speed);

            if (Vector3.Distance(_navMeshAgent.transform.position , _outsideTransform.position) <= 1f)
            {
                _navMeshAgent.speed = 0;
                _soldierAI.IsReachedOutside = true;
            }
        }

        public void OnEnter()
        {
            _navMeshAgent.speed = _soldierAI.RunSpeed;
            _navMeshAgent.SetDestination(_outsideTransform.position);
        }

        public void OnExit()
        {
            
        }
    }
}