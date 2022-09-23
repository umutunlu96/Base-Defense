using Enums;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.Miner
{
    public class MoveToSelectedResource : IState
    {
        private readonly MinerAI _minerAI;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly Animator _animator;
        private readonly MineWorkerType _mineWorkerType;
        private readonly Transform _resourceArea;
        private static readonly int Walk = Animator.StringToHash("Walk");
        
        public MoveToSelectedResource(MinerAI minerAI, NavMeshAgent navMeshAgent, Animator animator, MineWorkerType workerType, Transform resourceArea)
        {
            _minerAI = minerAI;
            _navMeshAgent = navMeshAgent;
            _animator = animator;
            _resourceArea = resourceArea;
            _mineWorkerType = workerType;
        }
        
        public void Tick()
        {

        }

        public void OnEnter()
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(_resourceArea.position);
            _animator.SetTrigger(Walk);
            _minerAI.PickAxeTransform.gameObject.SetActive(true);
        }

        public void OnExit()
        {
            _navMeshAgent.enabled = false;
        }
    }
}