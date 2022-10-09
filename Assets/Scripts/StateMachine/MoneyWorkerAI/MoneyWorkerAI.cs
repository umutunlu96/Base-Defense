using System;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.MoneyWorkerAI
{
    public class MoneyWorkerAI : MonoBehaviour
    {
        #region Variables

        #region Public

        public StackType StackType;
        public Transform MoneyTransform;

        public bool isBougth;
        public int SearchRange = 5;
        public float CollectRange = 0.5f;
        
        #endregion

        #region Serialized

        [SerializeField] private MoneyCollector moneyCollector;
        [SerializeField] private MoneyFinder moneyFinder;
        [SerializeField] private CollectableStackManager stackManager;
        [SerializeField] private float speed = 2f;
        [SerializeField] private int _collectedMoney = 0;
        
        #endregion

        #region Private

        private StateMachine _stateMachine;
        private StackData _stackData;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private Transform _baseTransform;
        private bool _cantFindAnyMoney;
        private int _capacity;
        private bool _isAtBase = true;
        private bool _isFull = false;

        #endregion

        #endregion
        
        public bool IsFull { get { return _isFull; } set { _isFull = value; } }
        
        public bool IsAtBase { get { return _isAtBase; } set { _isAtBase = value; } }
        
        public bool IsBougth { get { return isBougth; } set { isBougth = value; } }
        
        public float Speed { get { return speed; } private set { speed = value; } }
        
        public bool CantFindMoney { get { return _cantFindAnyMoney; } set { _cantFindAnyMoney = value; } }

        public Transform BaseTransform { get { return _baseTransform; } private set { _baseTransform = value; } }
        
        private StackData GetStackData() => Resources.Load<CD_StackData>("Data/CD_StackData").StackDatas[StackType];
        
        private void SetReferances()
        {
            _stackData = GetStackData();
            _capacity = _stackData.Capacity;
            stackManager.SetStackData(_stackData);
            BaseTransform = AiSignals.Instance.onGetBaseTransform();
        }
        
        private void Start()
        {
            SetReferances();
            InitAI();
        }

        private void InitAI()
        {
            _animator = GetComponentInChildren<Animator>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _stateMachine = new StateMachine();

            var stationary = new Stationary(this, _animator, _navMeshAgent);
            var moveBase = new MoveToBase(this, _animator, _navMeshAgent, _baseTransform);
            var moveToMoney = new MoveToMoney(this, _animator, _navMeshAgent);
            var search = new Search(this, moneyFinder, _navMeshAgent, _animator);

            At(stationary, moveBase, HasBougth());
            At(moveBase, search, HasAtBase());
            At(search, moveToMoney, HasFoundMoney());
            At(moveToMoney, search, HasPickedMoney());
            At(search, moveBase, IsBackPackFull());

            _stateMachine.SetState(stationary);
            
            _stateMachine.AddAnyTransition(moveToMoney, HasFoundMoney());
            _stateMachine.AddAnyTransition(moveBase, CantFindAnyMoney());
            _stateMachine.AddAnyTransition(search, SearchOverAgain());
            
            void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

            Func<bool> HasBougth() => () => IsBougth;
            Func<bool> HasAtBase() => () => IsAtBase;
            Func<bool> HasFoundMoney() => () => _collectedMoney < _capacity && MoneyTransform != null;
            Func<bool> HasPickedMoney() => () => MoneyTransform == null || MoneyTransform.CompareTag("Collected");
            Func<bool> IsBackPackFull() => () => _collectedMoney == _capacity;
            Func<bool> CantFindAnyMoney() => () => CantFindMoney && !IsAtBase;
            Func<bool> SearchOverAgain() => () => CantFindMoney && IsAtBase;
        }
        private void Update() => _stateMachine.Tick();

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("GateInside"))
            {
                IsAtBase = true;
                DropMoney();
            }
            
            if (other.CompareTag("GateOutside")) IsAtBase = false;
        }


        public void TakeMoney(Transform money)
        {
            if(_collectedMoney == _capacity) return;
            stackManager.AddStack(money);
            _collectedMoney++;
            money.tag = "Collected";
            MoneyTransform = null;
        }

        public void DropMoney()
        {
            if(_collectedMoney == 0 && !IsAtBase) return;
            stackManager.RemoveStackAll();
            _collectedMoney = 0;
        }
    }
}