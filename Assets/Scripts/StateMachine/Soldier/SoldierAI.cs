using System;
using Abstract;
using Data.UnityObject;
using Data.ValueObject.AI;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace StateMachine.Soldier
{
    public class SoldierAI : MonoBehaviour, IDamageable
    {
        [SerializeField] private SoldierAimController aimController;
        [SerializeField] private EnemyFinder enemyFinder;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private Animator animator;
        
        public IDamageable ChaseTarget;
        public IDamageable AttackTarget;
        private SoldierData _soldierData;
        private StateMachine _stateMachine;
        public Transform _soldierWaitTransform;
        public Vector3 _outsideTransform;
        
        [Header("Datas")]
        public float Health;
        public int Damage;
        public int AttackRate;
        public float AttackRange;
        public float SearchRange;
        public float ChaseUpdateSpeed;
        public float RunSpeed;
        public float WaitTime;
        

        private bool _isAlive = true;
        private bool _isPlayerCallForAttack;
        public bool IsReachedOutside;
        private float _timer;
        
        private void Awake()
        {
            _soldierData = GetData();
            InitDatas();
        }

        private SoldierData GetData() => Resources.Load<CD_Soldier>("Data/CD_Soldier").SoldierData;

        #region EventSubscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            AiSignals.Instance.onAttackAllSoldiers += OnPlayerCallForAttack;
        }
        
        private void UnSubscribeEvents()
        {
            AiSignals.Instance.onAttackAllSoldiers -= OnPlayerCallForAttack;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion

        #region Event Functions

        private void OnPlayerCallForAttack() => _isPlayerCallForAttack = true;

        #endregion
        
        private void InitDatas()
        {
            Health = _soldierData.Health;
            Damage = _soldierData.Damage;
            AttackRate = _soldierData.AttackRate;
            AttackRange = _soldierData.AttackRange;
            SearchRange = _soldierData.SearchRange;
            ChaseUpdateSpeed = _soldierData.ChaseUpdateSpeed;
            RunSpeed = _soldierData.RunSpeed;
            WaitTime = _soldierData.WaitTime;
        }
        
        private void Start()
        {
            GetMilitaryLocationTransforms();
            InitAI();
        }
        
        private void GetMilitaryLocationTransforms()
        {
            _soldierWaitTransform = AiSignals.Instance.onGetSoldierWaitTransform();
            // _outsideTransform = AiSignals.Instance.onGetOutsideTransform();
            int randomX = Random.Range(-20, 20);
            int randomZ = Random.Range(15, 45);
            _outsideTransform = new Vector3(randomX, 0, randomZ);
        }

        private void Update()
        {
            _stateMachine.Tick();
        }
        
        private void InitAI()
        {
            _stateMachine = new StateMachine();
            
            var wait = new Wait(this, animator, navMeshAgent, _soldierWaitTransform);
            var moveOutside = new MoveOutside(this, animator, navMeshAgent,_outsideTransform);
            var search = new Search(this, animator, navMeshAgent, enemyFinder);
            var move = new Move(this, animator, navMeshAgent, ChaseUpdateSpeed);
            var attack = new Attack(this, animator, navMeshAgent, enemyFinder);
            var death = new Death();
            
            At(wait, moveOutside, IsPlayerCalledForAttack());
            At(moveOutside, search, IsAtOutside());
            At(search, move, HasFoundEnemy());
            At(move, search, IsNoEnemyTarget());
            At(move, attack, EnemyInAttackRange());
            At(attack, search, IsNoEnemyTarget());
            At(attack, move, IsNoAttackTarget());
            
            _stateMachine.AddAnyTransition(death, IsDead());

            _stateMachine.SetState(wait);
            
            void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);
            
            Func<bool> IsPlayerCalledForAttack() => () => _isPlayerCallForAttack;
            Func<bool> IsAtOutside() => () => IsReachedOutside;
            Func<bool> HasFoundEnemy() => () => ChaseTarget != null;
            Func<bool> EnemyInAttackRange() => () => AttackTarget != null;
            Func<bool> IsNoEnemyTarget() => () => ChaseTarget == null || ChaseTarget.AmIDeath();
            Func<bool> IsNoAttackTarget() => () => AttackTarget == null;
            Func<bool> IsDead() => () => Health <= 0;
        }

        private void OnDeath()
        {
            _isAlive = false;
            navMeshAgent.enabled = false;
        }
        
        public void TakeDamage(float damage)
        {
            Health -= damage;
            if (Health <= 0)
                OnDeath();
        }

        public Transform GetTransform() => transform;

        public bool AmIDeath() => !_isAlive;
    }
}