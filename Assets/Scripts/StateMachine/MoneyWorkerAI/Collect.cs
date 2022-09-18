using UnityEngine;

namespace StateMachine.MoneyWorkerAI
{
    public class Collect : IState
    {
        private readonly MoneyWorkerAI _moneyWorkerAI;
        private readonly Animator _animator;
        private readonly Transform _money;
        
        public Collect(MoneyWorkerAI workerAI, Animator animator, Transform money)
        {
            _moneyWorkerAI = workerAI;
            _animator = animator;
            _money = money;
        }
        
        public void Tick()
        {
            
        }

        public void OnEnter()
        {
            
        }

        public void OnExit()
        {
            
        }
    }
}