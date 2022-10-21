using UnityEngine;

namespace StateMachine.Boss
{
    public class Stationary : IState
    {
        private readonly Animator _animator;
        private static readonly int Idle = Animator.StringToHash("Idle");
        public Stationary(Animator animator)
        {
            _animator = animator;
        }
        
        public void Tick()
        {

        }

        public void OnEnter()
        {

            _animator.SetTrigger(Idle);
        }

        public void OnExit()
        {
            
        }
    }
}