using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.Soldier
{
    public class MoveOutside : IState
    {
        private readonly SoldierAI _soldierAI;
        private readonly Animator _animator;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly Vector3 _outsideTransform;
        
        private readonly int Speed = Animator.StringToHash("Speed");
        
        public MoveOutside(SoldierAI soldierAI, Animator animator, NavMeshAgent navMeshAgent, Vector3 outsideTransform)
        {
            _soldierAI = soldierAI;
            _animator = animator;
            _navMeshAgent = navMeshAgent;
            _outsideTransform = outsideTransform;
        }
        
        public void Tick()
        {
            _animator.SetFloat(Speed, _navMeshAgent.speed);

            if (Vector3.Distance(_navMeshAgent.transform.position , _outsideTransform) <= 1f)
            {
                _navMeshAgent.speed = 0;
                _soldierAI.IsReachedOutside = true;
            }
        }

        public void OnEnter()
        {
            _navMeshAgent.speed = _soldierAI.RunSpeed;
            _navMeshAgent.SetDestination(_outsideTransform);
        }

        public void OnExit()
        {
            
        }
    }
}