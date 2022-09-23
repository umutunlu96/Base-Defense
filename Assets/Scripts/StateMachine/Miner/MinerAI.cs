using System;
using Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace StateMachine.Miner
{
    public class MinerAI : MonoBehaviour
    {
        
        public GameObject Gem;

        public Transform StockpileArea;
        public Transform GemArea;
        
        [SerializeField] private Transform _pickAxeTransform;
        [SerializeField] private Transform _diamondTransform;

        [ShowInInspector] private MineWorkerType _workerType;
        private StateMachine _stateMachine;
        private bool _isReachedGemArea;
        private bool _isReachedStockpileArea;
        private int _gathered;

        public bool ReachedGemArea { get { return _isReachedGemArea; } set { _isReachedGemArea = value; } }
        
        public bool ReachedStockpileArea { get { return _isReachedStockpileArea; } set { _isReachedStockpileArea = value; } }
        
        public Transform PickAxeTransform { get { return _pickAxeTransform; } set { _pickAxeTransform = value; } }
        
        public Transform DiamondTransform { get { return _diamondTransform; } set { _diamondTransform = value; } }
        
        
        private void Start()
        {
            var navMeshAgent = GetComponent<NavMeshAgent>();
            var animator = GetComponentInChildren<Animator>();
            var navMeshObstacle = GetComponent<NavMeshObstacle>();

            RandomMineWorkerType();
            
            navMeshAgent.enabled = true;
            
            StockpileArea = AiSignals.Instance.onGetGatherArea();
            GemArea = AiSignals.Instance.onGetResourceArea(_workerType);
            
            _stateMachine = new StateMachine();
            
            var moveToSelectedResource = new MoveToSelectedResource(this, navMeshAgent, animator, _workerType, GemArea);
            var harvest = new HarvestMine(this, animator, _workerType, GemArea, navMeshObstacle);
            var returnToGatherArea = new ReturnToStockpileArea(this, navMeshAgent, animator);
            var placeResourcesInStockpile = new PlaceDiamondToStockpileArea(this);

            
            At(moveToSelectedResource, harvest, ReachedResource());
            At(harvest, returnToGatherArea, InventoryFull());
            At(returnToGatherArea, placeResourcesInStockpile, ReachedStockpile());
            At(placeResourcesInStockpile, moveToSelectedResource, () => _gathered == 0);
            
            _stateMachine.SetState(moveToSelectedResource);

            void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

            Func<bool> ReachedResource() => () => GemArea != null && ReachedGemArea;
            Func<bool> InventoryFull() => () => _gathered == 1;
            Func<bool> ReachedStockpile() => () => StockpileArea != null && ReachedStockpileArea;
        }

        private void RandomMineWorkerType()
        {
            int randomMinerType = Random.Range(0, 4);
            if (randomMinerType == 0)
                _workerType = MineWorkerType.Gatherer;
            else
                _workerType = MineWorkerType.Miner;
        }
        private void Update() => _stateMachine.Tick();

        public void TakeFromTarget() => _gathered++;

        public void PlaceDiamondToGatherArea()
        {
            if (Gem == null || !AiSignals.Instance.onCanPlaceDiamondToStockpileArea()) return;
            AiSignals.Instance.onPlaceDiamondToStockpileArea?.Invoke(Gem);
            Gem = null;
            _gathered--;
        }
    }
}