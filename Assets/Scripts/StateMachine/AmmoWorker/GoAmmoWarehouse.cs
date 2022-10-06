using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.AmmoWorker
{
    public class GoAmmoWarehouse : IState
    {
        private readonly AmmoWorkerAI _ammoWorkerAI;
        private readonly Animator _animator;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly Transform _target;

        
        private static readonly int Speed = Animator.StringToHash("Speed");
        
        
        public GoAmmoWarehouse(AmmoWorkerAI workerAI, Animator animator, NavMeshAgent agent, Transform target)
        {
            _ammoWorkerAI = workerAI;
            _animator = animator;
            _navMeshAgent = agent;
            _target = target;
        }
        
        public void Tick()
        {
            Debug.Log("go wareaosue");
            _animator.SetFloat(Speed,_navMeshAgent.velocity.magnitude);
        }

        public void OnEnter()
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.speed = _ammoWorkerAI.Speed;
            _navMeshAgent.SetDestination(_target.position);
        }

        public void OnExit()
        {
            
        }
    }
}