using System;
using System.Collections.Generic;
using Abstract;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.MoneyWorkerAI
{
    public class MoneyWorkerAI : Worker
    {
        #region Variables

        #region Public

        public bool isBougth;
        public StackType StackType;
        public List<Transform> collectedMoneyList;

        public StackData StackData;
        public Transform MoneyTransform;

        public int SearchRange = 25;
        public int CollectRange = 3;
        #endregion

        #region Serialized

        [SerializeField] private MoneyFinder moneyFinder;
        [SerializeField] private CollectableStackManager stackManager;
        [SerializeField] private float speed = 2f;
        [SerializeField] private int capacity = 10;
        [SerializeField] private int _collectedMoney = 0;
        
        #endregion

        #region Private

        private StateMachine _stateMachine;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private Transform _baseTransform;
        private bool _cantFindAnyMoney;

        #endregion

        #endregion
        
        public MoneyWorkerAI(float speed, int capacity) : base(speed, capacity)
        {
            this.speed = speed;
            this.capacity = capacity;
        }
        
        public bool IsBougth { get { return isBougth; } set { isBougth = value; } }
        
        public float Speed { get { return speed; } private set { speed = value; } }
        
        public bool CantFindMoney { get { return _cantFindAnyMoney; } set { _cantFindAnyMoney = value; } }

        public Transform BaseTransform { get { return _baseTransform; } private set { _baseTransform = value; } }
        
        private StackData GetStackData() => Resources.Load<CD_StackData>("Data/CD_StackData").StackDatas[(int)StackType];
        
        private void SetReferances()
        {
            StackData = GetStackData();
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
            var search = new Search(this, moneyFinder);

            At(stationary, moveBase, HasBougth());
            At(moveBase, search, HasAtBase());
            At(search, moveToMoney, HasFoundMoney());
            At(moveToMoney, search, HasPickedMoney());
            At(search, moveBase, IsBackPackFull());

            _stateMachine.SetState(stationary);
            
            _stateMachine.AddAnyTransition(moveToMoney, HasFoundMoney());
            _stateMachine.AddAnyTransition(moveBase, CantFindAnyMoney());
            
            void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

            Func<bool> HasBougth() => () => IsBougth;
            Func<bool> HasAtBase() => () => Vector3.Distance(transform.position, BaseTransform.position) < 1f;
            Func<bool> HasFoundMoney() => () => _collectedMoney < capacity && MoneyTransform != null;
            Func<bool> HasPickedMoney() => () => MoneyTransform == null;
            Func<bool> IsBackPackFull() => () => _collectedMoney == capacity;
            Func<bool> CantFindAnyMoney() => () => CantFindMoney;
        }
        private void Update() => _stateMachine.Tick();
        
        public void TakeMoney(Transform money)
        {
            stackManager.AddStack(money);
            collectedMoneyList.Add(money);
            _collectedMoney++;
            MoneyTransform = null;
        }

        public void DropMoney()
        {
            if(_collectedMoney == 0) return;
            stackManager.RemoveStackAll();
            _collectedMoney = 0;
        }
    }
}