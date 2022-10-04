using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.AmmoWorker
{
    public class AmmoWorkerAI : MonoBehaviour
    {
        #region Variables

        #region Public

        public StackType StackType;

        #endregion

        #region Serialized
        
        [SerializeField] private CollectableStackManager stackManager;
        [SerializeField] private float walkSpeed = 2f;
        [SerializeField] private int _collectedAmmo = 0;
        
        #endregion

        #region Private

        private StateMachine _stateMachine;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        
        private Transform _baseTransform;
        
        private int _capacity;
        private bool _isAtBase = true;
        private bool _isFull = false;
        
        #endregion

        #endregion

        public float WalkSpeed { get { return walkSpeed; } private set { walkSpeed = value; } }
        
        public Transform BaseTransform { get { return _baseTransform; } private set { _baseTransform = value; } }
        

        private void SetReferances()
        {
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
        
        public void TakeAmmo(Transform ammo)
        {
            for (int i = 0; i < _capacity; i++)
            {
                
            }
            
            if(_collectedAmmo == _capacity) return;
            
            stackManager.AddStack(ammo);
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