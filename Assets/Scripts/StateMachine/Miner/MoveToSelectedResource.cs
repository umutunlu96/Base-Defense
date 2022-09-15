using UnityEngine;
using UnityEngine.AI;

namespace StateMachine
{
    public class MoveToSelectedResource : IState
    {
        private readonly MinerAI _minerAI;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly Animator _animator;
        private readonly Transform _resourceArea;
        private static readonly int Speed = Animator.StringToHash("Speed");
        
        public MoveToSelectedResource(MinerAI minerAI, NavMeshAgent navMeshAgent, Animator animator, Transform resourceArea)
        {
            _minerAI = minerAI;
            _navMeshAgent = navMeshAgent;
            _animator = animator;
            _resourceArea = resourceArea;
        }
        
        public void Tick()
        {

        }

        public void OnEnter()
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(_resourceArea.position);
            _animator.SetFloat(Speed, 1f);
        }

        public void OnExit()
        {
            _navMeshAgent.enabled = false;
            _animator.SetFloat(Speed, 0f);
            // Debug.Log("Reached to the mine area");
        }
    }
}