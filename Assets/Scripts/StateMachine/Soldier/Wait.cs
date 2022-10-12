using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.Soldier
{
    public class Wait : IState
    {
        
        private readonly SoldierAI _soldierAI;
        private readonly Animator _animator;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly Transform _waitTransform;
        private float _timer;
        private readonly int Speed = Animator.StringToHash("Speed");
        
        public Wait(SoldierAI soldierAI, Animator animator, NavMeshAgent navMeshAgent, Transform waitTransform)
        {
            _soldierAI = soldierAI;
            _animator = animator;
            _navMeshAgent = navMeshAgent;
            _waitTransform = waitTransform;
        }
        
        public void Tick()
        {
            _animator.SetFloat(Speed, _navMeshAgent.speed);
            if (Vector3.Distance(_navMeshAgent.transform.position, _waitTransform.position) <= .1f)
            {
                _soldierAI.transform.rotation = Quaternion.Slerp(_soldierAI.transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 20);
                _navMeshAgent.speed = 0;
            }
        }

        public void OnEnter()
        {
            _navMeshAgent.SetDestination(_waitTransform.position);
        }

        public void OnExit()
        {
            
        }
    }
}