using UnityEngine;

namespace StateMachine.Enemy
{
    public class Death : IState
    {
        private readonly EnemyAI _enemyAI;
        private readonly Animator _animator;
        private static readonly int death = Animator.StringToHash("Death");

        public Death(EnemyAI enemyAI, Animator animator)
        {
            _enemyAI = enemyAI;
            _animator = animator;
        }
        
        public void Tick()
        {
            Debug.Log("DEATHG");
        }

        public void OnEnter()
        {
            _animator.applyRootMotion = false;
            _animator.SetTrigger(death);
        }

        public void OnExit()
        {

        }
    }
}