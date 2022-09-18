using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.MoneyWorkerAI
{
    //navmesh random point finder
    public class Search : IState
    {
        private readonly MoneyWorkerAI _moneyWorkerAI;

        public Search(MoneyWorkerAI workerAI)
        {
            _moneyWorkerAI = workerAI;
        }
        
        public void Tick()
        {
            
        }

        public void OnEnter()
        {
            SearchMoney();
        }
        
        public void OnExit()
        {
            
        }

        private void SearchMoney()
        {
            Collider[] hitColliders = new Collider[25];
            hitColliders = Physics.OverlapSphere(_moneyWorkerAI.transform.position, _moneyWorkerAI.SearchRange, 1<<8);
            
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i] != null && hitColliders[i].CompareTag("Money"))
                {
                    _moneyWorkerAI.MoneyTransform = hitColliders[i].transform;
                }
            }
        }
    }
}