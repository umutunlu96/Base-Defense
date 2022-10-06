using UnityEngine;

namespace StateMachine.AmmoWorker
{
    public class PlaceAmmoToTurret : IState
    {
        private readonly AmmoWorkerAI _ammoWorkerAI;
        
        public PlaceAmmoToTurret(AmmoWorkerAI workerAI)
        {
            _ammoWorkerAI = workerAI;
        }
        
        public void Tick()
        {
            
        }

        public void OnEnter()
        {
            _ammoWorkerAI.DropAmmoToTurret();
        }

        public void OnExit()
        {
            
        }
    }
}