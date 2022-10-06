using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.AmmoWorker
{
    public class GoTurret : IState
    {
        private readonly AmmoWorkerAI _ammoWorkerAI;
        private readonly Animator _animator;
        private readonly NavMeshAgent _navMeshAgent;
        
        private static readonly int Speed = Animator.StringToHash("Speed");
        
        public GoTurret(AmmoWorkerAI workerAI, Animator animator, NavMeshAgent agent)
        {
            _ammoWorkerAI = workerAI;
            _animator = animator;
            _navMeshAgent = agent;
        }
        
        public void Tick()
        {
            Debug.Log("goturret");
            _animator.SetFloat(Speed,_navMeshAgent.velocity.magnitude);
        }

        public void OnEnter()
        {
            if(_ammoWorkerAI.IsCurrentTurretFull) _ammoWorkerAI.GetAvaibleTurretTarget();
            _navMeshAgent.enabled = true;
            _navMeshAgent.speed = _ammoWorkerAI.Speed;
            _navMeshAgent.SetDestination(_ammoWorkerAI.CurrentTarget.position);
        }

        public void OnExit()
        {
            
        }
    }
}