using StateMachine.Miner;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachine
{
    public class ReturnToGatherArea : IState
    {
        private readonly MinerAI _minerAI;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly Animator _animator;
        private static readonly int Carry = Animator.StringToHash("Carry");

        public ReturnToGatherArea(MinerAI minerAI, NavMeshAgent navMeshAgent, Animator animator)
        {
            _minerAI = minerAI;
            _navMeshAgent = navMeshAgent;
            _animator = animator;
        }

        public void Tick()
        {
        }

        public void OnEnter()
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(_minerAI.GatherArea.position);
            _animator.SetTrigger(Carry);
            _minerAI.PickAxeTransform.gameObject.SetActive(false);
        }

        public void OnExit()
        {
            _navMeshAgent.enabled = false;
        }
    }
}