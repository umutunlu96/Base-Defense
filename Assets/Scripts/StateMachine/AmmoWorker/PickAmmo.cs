using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.AmmoWorker
{
    public class PickAmmo : IState
    {
        private readonly AmmoWorkerAI _ammoWorkerAI;
        private readonly Animator _animator;
        
        private static readonly int Speed = Animator.StringToHash("Speed");
        
        
        public PickAmmo(AmmoWorkerAI workerAI, Animator animator)
        {
            _ammoWorkerAI = workerAI;
            _animator = animator;
        }
        
        public void Tick()
        {
            _animator.SetFloat(Speed, 0);
        }

        public void OnEnter()
        {
            _ammoWorkerAI.TakeAmmo();
            _ammoWorkerAI.GetAvaibleTurretTarget();
        }

        public void OnExit()
        {
            
        }
    }
}