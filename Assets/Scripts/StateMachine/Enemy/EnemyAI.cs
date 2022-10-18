using System;
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

        [SerializeField] private EnemyAnimationController animationController;
        [SerializeField] private SkinnedMeshRenderer sMeshRenderer;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private NavMeshObstacle navMeshObstacle;
        [SerializeField] private Animator animator;

        #endregion

        #region Private

        private EnemyData _enemyData;
        private StateMachine _stateMachine;
        private Transform _baseTarget;
        private Transform _playerTarget;
        
        [ShowInInspector] private float _health;
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
        private bool _isDeath = false;
        private bool _isAlive = true;
        private static readonly int Idle = Animator.StringToHash("Idle");

        #endregion
        
        #endregion

        #region Props
        
        public bool CanAttack { get { return _canAttack; } set { _canAttack = value; } }
        
        public bool Attacked { get { return _attacked; } set { _attacked = value; } }
        
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
            var death = new Death(this, animator, EnemyType);

            At(moveToBase, chasePlayer, CanChasePlayer());
            At(chasePlayer, attack, IsInAttackRange());
            At(attack, chasePlayer, CanChasePlayer());
            At(chasePlayer, moveToBase, GoBase());
            At(moveToBase, reachedBase, ReachedBase());
            At(death, moveToBase, IsAlive());
            
            _stateMachine.AddAnyTransition(death, IsDeath());
            _stateMachine.AddAnyTransition(chasePlayer, CanChasePlayer());
            _stateMachine.AddAnyTransition(attack, IsInAttackRange());
            
            _stateMachine.SetState(moveToBase);
            
            void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);
            
            Func<bool> GoBase() => () => PlayerTarget == null && !CanAttack && !ReachedAtBase && _baseTarget != null;
            Func<bool> CanChasePlayer() => () => PlayerTarget != null && !CanAttack;
            Func<bool> IsInAttackRange() => () => PlayerTarget != null && CanAttack;
            Func<bool> ReachedBase() => () => ReachedAtBase && _baseTarget != null && PlayerTarget == null;
            Func<bool> IsDeath() => () => _isDeath;
            Func<bool> IsAlive() => () => _isAlive;
            
        }
        
        private void Update()
        {
            if(!_isAlive) return;
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
        
        public void DeathAnimCompleted()
        {
            PoolSignals.Instance.onReleasePoolObject?.Invoke($"{EnemyType}", gameObject);
        }
        
        private void OnEnable()
        {
            if (_isDeath)
            {
                OnAlive();
            }
        }

        private void OnDisable()
        {
            if (_isDeath)
            {
                _isAlive = false;
            }
        }

        private void OnAlive()
        {
            _health = _enemyData.Health;
            animator.SetTrigger(Idle);
            ChangeSaturation(1, 1, .1f);
            navMeshAgent.enabled = true;
            _isDeath = false;
            _isAlive = true;
            ReachedAtBase = false;
        }

        private void ChangeSaturation(float saturation, float brightness, float duration)
        {
            sMeshRenderer.material.DOFloat(saturation,"_Saturation", duration);
            sMeshRenderer.material.DOFloat(brightness,"_Brightness", duration);
        }
        
        private void OnDeath()
        {
            _isDeath = true;
            navMeshAgent.enabled = false;
            transform.DOMoveY(-.5f, .2f);
            ChangeSaturation(.25f, .25f, .5f);
        }
        
        public void TakeDamage(float damage)
        {
            _health -= damage;
            if (_health <= 0)
                OnDeath();
        }

        public Transform GetTransform() => transform;
        
        public bool AmIDeath() => IsDeath;
    }
}