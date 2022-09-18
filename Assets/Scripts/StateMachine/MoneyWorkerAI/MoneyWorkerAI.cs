using System;
using System.Collections.Generic;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

//delete movetofrontyard
namespace StateMachine.MoneyWorkerAI
{
    public class MoneyWorkerAI : MonoBehaviour
    {
        #region Variables

        #region Public

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
        // private Transform _outsideTransform;

        private bool _cantFindAnyMoney;
        // private bool _moneyInRange;
        
        #endregion

        #endregion

        public float Speed { get { return speed; } private set { speed = value; } }
        
        public bool CantFindMoney { get { return _cantFindAnyMoney; } set { _cantFindAnyMoney = value; } }

        public Transform BaseTransform { get { return _baseTransform; } private set { _baseTransform = value; } }
        
        // public Transform OutsideTransform { get { return _outsideTransform; } private set { _outsideTransform = value; } }
        
        
        
        // public bool MoneyInRange { get { return _moneyInRange; } set { _moneyInRange = value; } }
        
        
        private StackData GetStackData() => Resources.Load<CD_StackData>("Data/CD_StackData").StackDatas[(int)StackType];
        
        private void SetReferances()
        {
            StackData = GetStackData();
            BaseTransform = AiSignals.Instance.onGetBaseTransform();
            // OutsideTransform = AiSignals.Instance.onGetOutsideTransform();
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

            var moveBase = new MoveToBase(this, _animator, _navMeshAgent, _baseTransform);
            // var moveOutside = new MoveToFrontyard(this, _animator, _navMeshAgent, _outsideTransform);
            var moveToMoney = new MoveToMoney(this, _animator, _navMeshAgent);
            
            var search = new Search(this, moneyFinder);
            
            // At(moveBase, moveOutside, HasAtBase());
            // At(moveOutside, search, HasAtOutside());
            // At(search, moveToMoney, HasFoundMoney());
            
            At(moveBase, search, HasAtBase());
            // At(search, moveOutside, GoOutsideWhenFoundMoney());
            // At(moveOutside, moveToMoney, HasFoundMoney());
            At(search, moveToMoney, HasFoundMoney());
            At(moveToMoney, search, HasPickedMoney());
            At(search, moveBase, IsBackPackFull());
            // At(moveOutside, moveBase, HasAtOutsideAndBackPackIsFull());
            
            _stateMachine.SetState(moveBase);
            
            _stateMachine.AddAnyTransition(moveToMoney, HasFoundMoney());
            _stateMachine.AddAnyTransition(moveBase, CantFindAnyMoney());
            
            // _stateMachine.AddAnyTransition(moveOutside, IsBackPackFull());

            
            void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);
            
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