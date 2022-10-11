using System;
using Abstract;
using Data.UnityObject;
using Data.ValueObject.AI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.Soldier
{
    public class SoldierAI : MonoBehaviour, IDamageable
    {
        [SerializeField] private SoldierAimController aimController;
        [SerializeField] private EnemyFinder enemyFinder;
        [SerializeField] private NavMeshAgent navMeshAgent;
        private SoldierData _soldierData;
        private StateMachine _stateMachine;
        private Transform _baseTarget;
        
        [ShowInInspector] private int _health;
        [ShowInInspector] private int _damage;
        [ShowInInspector] private int _attackRate;
        [ShowInInspector] private float _attackRange;
        [ShowInInspector] private float _chaseRange;
        [ShowInInspector] private float _chaseUpdateSpeed;
        [ShowInInspector] private float _runSpeed;
        private bool _isAlive = true;
        
        /// Wait State - player attack butonuna basana kadar bu state de kalacak.
        /// Search - Attack basıldıktan sonra enemy search çalışacak.
        /// Move - bulunca enemy e doğru yürüyecek.
        /// Attack - attack rangesine girince enemy durup ateş edecek
        /// Death - can<= 0 sa ölecek.
        ///
        ///

        private void Awake()
        {
            _soldierData = GetData();
            InitEnemyDatas();
        }

        private SoldierData GetData() => Resources.Load<CD_Soldier>("Data/CD_Soldier").SoldierData;

        private void InitEnemyDatas()
        {
            _health = _soldierData.Health;
            _damage = _soldierData.Damage;
            _attackRate = _soldierData.AttackRate;
            _attackRange = _soldierData.AttackRange;
            _chaseRange = _soldierData.ChaseRange;
            _chaseUpdateSpeed = _soldierData.ChaseUpdateSpeed;
            _runSpeed = _soldierData.RunSpeed;
        }
        
        private void Start()
        {
            InitAI();
        }

        private void Update()
        {
            _stateMachine.Tick();
        }
        
        private void InitAI()
        {
            _stateMachine = new StateMachine();
            
            
            
            
            
            
            
            
            
            void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);
        }


        
        
        
        
        private void OnDeath()
        {
            _isAlive = false;
            navMeshAgent.enabled = false;
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
                OnDeath();
        }

        public Transform GetTransform() => transform;

        public bool AmIDeath() => !_isAlive;
    }
}