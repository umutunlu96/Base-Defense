﻿using System;
using Abstract;
using Data.UnityObject;
using Data.ValueObject;
using DG.Tweening;
using Enums;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
namespace StateMachine.Enemy
{
    public class EnemyAI : MonoBehaviour, IDamageable
    {
        #region Variables

        #region Public

        public EnemyType EnemyType;
        public Transform CurrentTarget;

        #endregion

        #region Serialized

        [SerializeField] private Renderer renderer;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private NavMeshObstacle navMeshObstacle;
        [SerializeField] private Animator animator;

        #endregion

        #region Private

        private EnemyData _enemyData;
        private StateMachine _stateMachine;
        private Transform _baseTarget;
        private Transform _playerTarget;
        
        [ShowInInspector] private int _health;
        [ShowInInspector] private int _damage;
        [ShowInInspector] private float _chaseRange;
        [ShowInInspector] private float _attackRange;
        [ShowInInspector] private float _chaseUpdateSpeed;
        [ShowInInspector] private float _walkSpeed;
        [ShowInInspector] private float _runSpeed;
        
        private bool _canAttack;
        private bool _reachedAtTheBase;
        private bool _attacked;
        private bool _attackAnimEnded;
        // private bool _deathAnimEnded;
        private bool _isDeath;
        private static readonly int Idle = Animator.StringToHash("Idle");

        #endregion
        
        #endregion

        #region Props
        
        public bool CanAttack { get { return _canAttack; } set { _canAttack = value; } }
        
        public bool Attacked { get { return _attacked; } set { _attacked = value; } }
        
        public bool AttackAnimEnded { get { return _attackAnimEnded; } set { _attackAnimEnded = value; } }
        
        public bool IsDeath { get { return _isDeath; } set { _isDeath = value; } }
        
        public bool ReachedAtBase { get { return _reachedAtTheBase; } set { _reachedAtTheBase = value; } }
        
        public Transform BaseTarget { get { return _baseTarget; } set { _baseTarget = value; } }
        
        public Transform PlayerTarget { get { return _playerTarget; } set { _playerTarget = value; } }
        
        public float WalkSpeed { get { return _walkSpeed; } set { _walkSpeed = value; } }
        
        public float RunSpeed { get { return _runSpeed; } set { _runSpeed = value; } }
        
        public float AttackRange { get { return _attackRange; } set { _attackRange = value; } }
        
        public float ChaseRange { get { return _chaseRange; } set { _chaseRange = value; } }

        #endregion

        private void Awake()
        {
            _enemyData = GetData();
            InitEnemyDatas();
        }

        private EnemyData GetData() => Resources.Load<CD_Enemy>("Data/CD_Enemy").EnemyDatas[EnemyType];

        private void InitEnemyDatas()
        {
            _health = _enemyData.Health;
            _damage = _enemyData.Damage;
            _attackRange = _enemyData.AttackRange;
            _chaseRange = _enemyData.ChaseRange;
            _chaseUpdateSpeed = _enemyData.ChaseUpdateSpeed;
            _walkSpeed = _enemyData.WalkSpeed;
            _runSpeed = _enemyData.RunSpeed;
        }
        
        private void Start()
        {
            InitAI();
        }
        
        private void InitAI()
        {
            _stateMachine = new StateMachine();
            BaseTarget = AiSignals.Instance.onGetBaseAttackPoint();

            var moveToBase = new MoveToBase(this, animator, navMeshAgent, BaseTarget);
            var chasePlayer = new Chase(this, animator, navMeshAgent, _chaseUpdateSpeed);
            var attack = new Attack(this, animator, navMeshAgent, navMeshObstacle);
            var reachedBase = new ReachedBase(this, animator, navMeshAgent, navMeshObstacle);
            var death = new Death(this, animator, renderer, EnemyType);

            At(moveToBase, chasePlayer, CanChasePlayer());
            At(chasePlayer, attack, IsInAttackRange());
            At(attack, chasePlayer, CanChasePlayer());
            At(chasePlayer, moveToBase, GoBase());
            At(moveToBase, reachedBase, ReachedBase());
            
            _stateMachine.AddAnyTransition(death, IsDeath());
            _stateMachine.AddAnyTransition(chasePlayer, CanChasePlayer());
            _stateMachine.AddAnyTransition(attack, IsInAttackRange());
            
            _stateMachine.SetState(moveToBase);
            
            void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);
            
            Func<bool> GoBase() => () => PlayerTarget == null && !CanAttack && !ReachedAtBase && _baseTarget != null;
            Func<bool> CanChasePlayer() => () => PlayerTarget != null && !CanAttack;
            Func<bool> IsInAttackRange() => () => PlayerTarget != null && CanAttack;
            Func<bool> ReachedBase() => () => ReachedAtBase && _baseTarget != null && PlayerTarget == null;
            Func<bool> IsDeath() => () => _health <= 0;
        }

        private void Update()
        {
            if(_isDeath) return;
                _stateMachine.Tick();
        }

        public void AttackedToPlayer()
        {
            if (Attacked)
            {
                //PlayerTakeDamage
                // Debug.Log("PlayerTookDamage");
                Attacked = false;
            }
        }

        public void Death()
        {
            _isDeath = true;
            AiSignals.Instance.onEnemyDead?.Invoke(transform);
            PoolSignals.Instance.onGetPoolObject?.Invoke("Money", transform);
        }
        
        private void OnEnable()
        {
            if (_isDeath)
            {
                // print("Death but now alive");
                _health = _enemyData.Health;
                animator.SetTrigger(Idle);
                // renderer.material.SetFloat("_Saturation", 1);
                navMeshAgent.enabled = true;
            }
            _isDeath = false;
        }

        private void OnDisable()
        {
            if (_isDeath)
            {
                
            }
        }

        public void TakeDamage(int damage) => _health -= damage;

        public Transform GetTransform() => transform;
        public bool AmIDeath() => IsDeath;
    }
}