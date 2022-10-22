using UnityEngine;

namespace StateMachine.Boss
{
    public class Death : IState
    {
        private readonly Animator _animator;
        private readonly BossAI _bossAI;
        
        private static readonly int death = Animator.StringToHash("Death");
        public Death(BossAI bossAI, Animator animator)
        {
            _bossAI = bossAI;
            _animator = animator;
        }
        
        public void Tick()
        {

        }

        public void OnEnter()
        {
            _animator.SetTrigger(death);
            _bossAI.OnDeath();
        }

        public void OnExit()
        {
            
        }
    }
}