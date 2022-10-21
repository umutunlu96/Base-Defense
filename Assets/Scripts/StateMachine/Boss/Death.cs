using UnityEngine;

namespace StateMachine.Boss
{
    public class Death : IState
    {
        private readonly Animator _animator;
        private static readonly int death = Animator.StringToHash("Death");
        public Death(Animator animator)
        {
            _animator = animator;
        }
        
        public void Tick()
        {
            Debug.Log("Death");
        }

        public void OnEnter()
        {
            Debug.Log("Death");
            _animator.SetTrigger(death);
        }

        public void OnExit()
        {
            
        }
    }
}