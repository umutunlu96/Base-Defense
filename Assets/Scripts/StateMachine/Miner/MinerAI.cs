using System;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.Miner
{
    public class MinerAI : MonoBehaviour
    {
        [SerializeField] private int _maxCarried = 1;
        private int _gathered;
        
        public GameObject Gem;

        private StateMachine _stateMachine;
        public Transform StockpileArea;
        public Transform GemArea;
        
        [SerializeField] private Transform _pickAxeTransform;
        [SerializeField] private Transform _diamondTransform;
        
        private bool _isReachedGemArea;
        private bool _isReachedStockpileArea;

        public bool ReachedGemArea { get { return _isReachedGemArea; } set { _isReachedGemArea = value; } }
        
        public bool ReachedStockpileArea { get { return _isReachedStockpileArea; } set { _isReachedStockpileArea = value; } }
        
        public Transform PickAxeTransform { get { return _pickAxeTransform; } set { _pickAxeTransform = value; } }
        
        public Transform DiamondTransform { get { return _diamondTransform; } set { _diamondTransform = value; } }
        
        
        private void Start()
        {
            var navMeshAgent = GetComponent<NavMeshAgent>();
            var animator = GetComponentInChildren<Animator>();
            var navMeshObstacle = GetComponent<NavMeshObstacle>();
            
            navMeshAgent.enabled = true;
            
            StockpileArea = AiSignals.Instance.onGetGatherArea();
            GemArea = AiSignals.Instance.onGetResourceArea();
            
            _stateMachine = new StateMachine();
            
            var moveToSelectedResource = new MoveToSelectedResource(this, navMeshAgent, animator, GemArea);
            var harvest = new HarvestMine(this, animator, GemArea, navMeshObstacle);
            var returnToGatherArea = new ReturnToStockpileArea(this, navMeshAgent, animator);
            var placeResourcesInStockpile = new PlaceDiamondToStockpileArea(this);

            
            At(moveToSelectedResource, harvest, ReachedResource());
            At(harvest, returnToGatherArea, InventoryFull());
            At(returnToGatherArea, placeResourcesInStockpile, ReachedStockpile());
            At(placeResourcesInStockpile, moveToSelectedResource, () => _gathered == 0);
            
            _stateMachine.SetState(moveToSelectedResource);

            void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

            Func<bool> ReachedResource() => () => GemArea != null && ReachedGemArea;
            Func<bool> InventoryFull() => () => _gathered >= _maxCarried;
            Func<bool> ReachedStockpile() => () => StockpileArea != null && ReachedStockpileArea;
        }
        
        private void Update() => _stateMachine.Tick();

        public void TakeFromTarget() => _gathered++;

        public void PlaceDiamondToGatherArea()
        {
            if (Gem == null) return;
            AiSignals.Instance.onPlaceDiamondToGatherArea?.Invoke(Gem);
            Gem = null;
            _gathered--;
        }
    }
}