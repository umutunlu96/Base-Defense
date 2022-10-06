using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.AmmoWorker
{
    public class Stationary : IState
    {
        private readonly AmmoWorkerAI _ammoWorkerAI;
        private readonly Animator _animator;
        private readonly NavMeshAgent _navMeshAgent;
        
        private static readonly int Idle = Animator.StringToHash("Idle");

        public Stationary(AmmoWorkerAI workerAI, Animator animator, NavMeshAgent agent)
        {
            _ammoWorkerAI = workerAI;
            _animator = animator;
            _navMeshAgent = agent;
        }
        
        public void Tick()
        {
            Debug.Log("stationary");
        }

        public void OnEnter()
        {
            _navMeshAgent.enabled = false;
            _animator.SetTrigger(Idle);
        }

        public void OnExit()
        {
            _navMeshAgent.enabled = true;
        }
    }
}