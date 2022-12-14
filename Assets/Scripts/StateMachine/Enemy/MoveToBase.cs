using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.Enemy
{
    public class MoveToBase : IState
    {
        private readonly EnemyAI _enemyAI;
        private readonly Animator _animator;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly Transform _baseTarget;
        
        private static readonly int Speed = Animator.StringToHash("Speed");
        
        public MoveToBase(EnemyAI enemyAI,Animator animator, NavMeshAgent agent, Transform baseTarget)
        {
            _enemyAI = enemyAI;
            _animator = animator;
            _navMeshAgent = agent;
            _baseTarget = baseTarget;
        }
        public void Tick()
        {
            _animator.SetFloat(Speed,_navMeshAgent.velocity.magnitude);
        }

        public void OnEnter()
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.speed = _enemyAI.WalkSpeed;
            if (_baseTarget == null)
            {
                _navMeshAgent.SetDestination(AiSignals.Instance.onGetBaseTransform().position);
                return;
            }
            _navMeshAgent.SetDestination(_baseTarget.position);
        }

        public void OnExit()
        {

        }
    }
}