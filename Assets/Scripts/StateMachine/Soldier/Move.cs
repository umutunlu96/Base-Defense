using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.Soldier
{
    public class Move : IState
    {
        private readonly SoldierAI _soldierAI;
        private readonly Animator _animator;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly float _chaseUpdateSpeed;
        private float _timer;
        private readonly int Speed = Animator.StringToHash("Speed");
        
        public Move(SoldierAI soldierAI, Animator animator, NavMeshAgent navMeshAgent, float chaseUpdateSpeed)
        {
            _soldierAI = soldierAI;
            _animator = animator;
            _navMeshAgent = navMeshAgent;
            _chaseUpdateSpeed = chaseUpdateSpeed;
        }
        
        public void Tick()
        {
            Debug.Log("Move");
            _animator.SetFloat(Speed,_navMeshAgent.velocity.magnitude);
            _timer += Time.deltaTime;
            if (_timer >= _chaseUpdateSpeed)
            {
                _navMeshAgent.SetDestination(_soldierAI.ChaseTarget.GetTransform().position);
                
                if (_soldierAI.ChaseTarget.AmIDeath())
                {
                    _soldierAI.ChaseTarget = null;
                }
            }
        }

        public void OnEnter()
        {
            _navMeshAgent.speed = _soldierAI.RunSpeed;
            _animator.SetFloat(Speed, _navMeshAgent.speed);
            _navMeshAgent.SetDestination(_soldierAI.ChaseTarget.GetTransform().position);
        }

        public void OnExit()
        {
            
        }
    }
}