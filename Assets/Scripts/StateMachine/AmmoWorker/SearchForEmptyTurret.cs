using UnityEngine;

namespace StateMachine.AmmoWorker
{
    public class SearchForEmptyTurret : IState
    {
        private readonly AmmoWorkerAI _ammoWorkerAI;
        private float timer;


        public SearchForEmptyTurret(AmmoWorkerAI workerAI)
        {
            _ammoWorkerAI = workerAI;
        }
        
        
        public void Tick()
        {
            timer += Time.deltaTime;
            if (timer >= 5)
            {
                _ammoWorkerAI.GetAvaibleTurretTarget();
                timer = 0;
            }
        }

        public void OnEnter()
        {

        }

        public void OnExit()
        {

        }
    }
}