using System;
using System.Collections.Generic;
using Data.UnityObject;
using Data.ValueObject;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.MoneyWorkerAI
{
    public class MoneyWorkerAI : MonoBehaviour
    {
        #region Variables

        #region Public

        public List<Transform> collectedMoneyList;
        public StackData StackData;
        public Transform CurrentTarget;
        public Transform MoneyTransform;

        public int SearchRange = 25;
        public int CollectRange = 3;
        #endregion

        #region Serialized
        
        [SerializeField] private float speed = 2f;
        [SerializeField] private int capacity = 10;
        [SerializeField] private int _collectedMoney = 0;
        
        #endregion

        #region Private

        private StateMachine _stateMachine;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        
        private Transform _baseTransform;
        private Transform _outsideTransform;
        
        // private bool _moneyInRange;
        
        #endregion

        #endregion

        public float Speed { get { return speed; } private set { speed = value; } }

        public Transform BaseTransform { get { return _baseTransform; } private set { _baseTransform = value; } }
        
        public Transform OutsideTransform { get { return _outsideTransform; } private set { _outsideTransform = value; } }
        
        // public bool MoneyInRange { get { return _moneyInRange; } set { _moneyInRange = value; } }
        
        
        private StackData GetStackData() => Resources.Load<CD_StackData>("Data/CD_StackData").MoneyWorkerStackData;
        
        private void SetReferances()
        {
            StackData = GetStackData();
            BaseTransform = AiSignals.Instance.onGetBaseTransform();
            OutsideTransform = AiSignals.Instance.onGetOutsideTransform();
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
            var moveOutside = new MoveToFrontyard(this, _animator, _navMeshAgent, _outsideTransform);
            var moveToMoney = new MoveToMoney(this, _animator, _navMeshAgent);
            
            var search = new Search(this);
            var collect = new Collect(this, _animator, MoneyTransform);


            At(moveBase, moveOutside, HasAtBase());
            At(moveOutside, search, HasAtOutside());
            At(search, moveToMoney, HasFoundMoney());
            At(moveToMoney, search, HasPickedMoney());
            At(moveOutside, moveBase, IsBackPackFull());
            
            _stateMachine.SetState(moveBase);
            
            _stateMachine.AddAnyTransition(moveToMoney, HasFoundMoney());
            _stateMachine.AddAnyTransition(moveBase, IsBackPackFull());

            
            void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);
            
            Func<bool> HasAtBase() => () => Vector3.Distance(transform.position, BaseTransform.position) < 1f;
            Func<bool> HasAtOutside() => () => Vector3.Distance(transform.position, OutsideTransform.position) < 1f;
            Func<bool> HasFoundMoney() => () => MoneyTransform != null;
            Func<bool> HasPickedMoney() => () => MoneyTransform == null;
            Func<bool> IsBackPackFull() => () => _collectedMoney == capacity;
        }
        private void Update() => _stateMachine.Tick();
        
        public void TakeMoney(Transform money)
        {
            collectedMoneyList.Add(money);
            _collectedMoney++;
            money.transform.SetParent(transform);
            MoneyTransform = null;
        }

        public void GiveMoney()
        {
            _collectedMoney = 0;
        }
    }
}