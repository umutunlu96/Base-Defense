using System;
using System.Collections.Generic;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using StateMachine.MoneyWorkerAI;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.AmmoWorker
{
    public class AmmoWorkerAI : MonoBehaviour
    {
        #region Variables

        #region Public

        public StackType StackType;
        public List<Transform> collectedAmmoList;

        public StackData StackData;
        
        #endregion

        #region Serialized
        
        [SerializeField] private CollectableStackManager stackManager;
        [SerializeField] private float speed = 2f;
        [SerializeField] private int capacity = 10;
        [SerializeField] private int _collectedAmmo = 0;
        
        #endregion

        #region Private

        private StateMachine _stateMachine;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        
        private Transform _baseTransform;
        
        #endregion

        #endregion

        public float Speed { get { return speed; } private set { speed = value; } }
        
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

            
            void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);
            
        }
        private void Update() => _stateMachine.Tick();
        
        public void TakeMoney(Transform money)
        {
            stackManager.AddStack(money);
            collectedAmmoList.Add(money);
            _collectedAmmo++;
        }

        public void DropAmmo()
        {
            if(_collectedAmmo == 0) return;
            stackManager.RemoveStackAll();
            _collectedAmmo = 0;
        }
    }
}