using System;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.Miner
{
    public class MinerAI : MonoBehaviour
    {
        
        [SerializeField] private int _maxCarried = 1;
        private int _gathered;
        
        private StateMachine _stateMachine;

        public Transform GatherArea;
        public Transform ResourceArea;
        
        
        private void Start()
        {
            var navMeshAgent = GetComponent<NavMeshAgent>();
            var animator = GetComponentInChildren<Animator>();

            navMeshAgent.enabled = true;
            
            GatherArea = AiSignals.Instance.onGetGatherArea();
            ResourceArea = AiSignals.Instance.onGetResourceArea();
            
            _stateMachine = new StateMachine();
            
            var moveToSelectedResource = new MoveToSelectedResource(this, navMeshAgent, animator, ResourceArea);
            var harvest = new HarvestMine(this, animator);
            var returnToGatherArea = new ReturnToGatherArea(this, navMeshAgent, animator);
            var placeResourcesInStockpile = new PlaceDiamondToGatherArea(this);

            
            At(moveToSelectedResource, harvest, ReachedResource());
            At(harvest, returnToGatherArea, InventoryFull());
            At(returnToGatherArea, placeResourcesInStockpile, ReachedStockpile());
            At(placeResourcesInStockpile, moveToSelectedResource, () => _gathered == 0);
            
            _stateMachine.SetState(moveToSelectedResource);

            void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

            Func<bool> ReachedResource() => () => ResourceArea != null && 
                                                  Vector3.Distance(transform.position, ResourceArea.position) < 2f;
            Func<bool> InventoryFull() => () => _gathered >= _maxCarried;
            Func<bool> ReachedStockpile() => () => GatherArea != null && 
                                                   Vector3.Distance(transform.position, GatherArea.position) < 2f;
        }
        
        private void Update() => _stateMachine.Tick();

        public void TakeFromTarget() => _gathered++;

        public bool Take()
        {
            if (_gathered <= 0)
                return false;
            _gathered--;
            return true;
        }
    }
}