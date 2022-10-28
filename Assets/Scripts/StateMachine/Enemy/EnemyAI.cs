using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstract;
using Data.UnityObject;
using Data.ValueObject;
using DG.Tweening;
using Enums;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace StateMachine.Enemy
{
    public class EnemyAI : MonoBehaviour, IDamageable
    {
        #region Variables

        #region Public

        public EnemyType EnemyType;
        public Transform CurrentTarget;
        public Transform PlayerTarget;
        public Transform BaseTarget;
        public bool CanAttackToBase;
        public bool IsInGroundMineArea = false;
        public Transform GroundMineTarget;

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
        
        [ShowInInspector] private float _health;
        [ShowInInspector] private int _damage;
        [ShowInInspector] private float _chaseRange;
        [ShowInInspector] private float _attackRange;
        [ShowInInspector] private float _walkSpeed;
        [ShowInInspector] private float _runSpeed;
        
        private bool _reachedAtTheBase;
        private bool _isDeath = false;
        private bool _isAlive = true;
        private bool _isPlayerDead = false;
        private bool _isGroundMineActivated = false;
        private static readonly int Idle = Animator.StringToHash("Idle");
        

        #endregion
        
        #endregion

        #region Props
        
        public bool IsDeath { get { return _isDeath; } set { _isDeath = value; } }
        
        public bool ReachedAtBase { get { return _reachedAtTheBase; } set { _reachedAtTheBase = value; } }
        
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
            CurrentTarget = BaseTarget;
            
            var moveToBase = new MoveToBase(this, animator, navMeshAgent, BaseTarget);
            var chasePlayer = new Chase(this, animator, navMeshAgent);
            var attack = new Attack(this, animator, navMeshAgent, navMeshObstacle, _damage);
            var death = new Death(this, animator);
            var moveToBomb = new MoveGroundMine(this, animator, navMeshAgent);
            
            At(moveToBase, chasePlayer, CanChasePlayer());
            At(chasePlayer, attack, IsInAttackRange());
            At(attack, chasePlayer, CanChasePlayer());
            At(attack, moveToBase, IsPlayerDead());
            At(chasePlayer, moveToBase, GoBase());
            At(moveToBase, attack, IsAtBase());
            At(death, moveToBase, IsAlive());
            At(moveToBomb, attack, IsInGroundMineArea());
            At(moveToBomb, moveToBase, IsMineExplode());
            
            _stateMachine.AddAnyTransition(death, IsDeath());
            _stateMachine.AddAnyTransition(moveToBomb, IsGroundMineActivated());
            
            _stateMachine.SetState(moveToBase);
            
            void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);

            Func<bool> GoBase() => () => CurrentTarget == BaseTarget;
            Func<bool> CanChasePlayer() => () => CurrentTarget == PlayerTarget && Vector3.Distance(transform.position, CurrentTarget.position) > navMeshAgent.stoppingDistance;
            Func<bool> IsInAttackRange() => () =>
                CurrentTarget == PlayerTarget && Vector3.Distance(transform.position, CurrentTarget.position) <= navMeshAgent.stoppingDistance;
            Func<bool> IsAtBase() => () => CanAttackToBase && !_isDeath;
            Func<bool> IsDeath() => () => _health <= 0;
            Func<bool> IsAlive() => () => _health > 0;
            Func<bool> IsPlayerDead() => () => _isPlayerDead;
            Func<bool> IsGroundMineActivated() => () => _isGroundMineActivated;
            Func<bool> IsInGroundMineArea() => () => Vector3.Distance(transform.position, GroundMineTarget.position) < navMeshAgent.stoppingDistance * 3;
            Func<bool> IsMineExplode() => () =>  CurrentTarget != GroundMineTarget && _isGroundMineActivated == false;
            
        }
        
        private void Update()
        {
            if(!_isAlive) return;
            _stateMachine.Tick();
        }

        public void SetTarget(Transform target)
        {
            CurrentTarget = target;
        }
        
        private void OnEnable()
        {
            PlayerSignals.Instance.onPlayerDead += OnPlayerDead;
            PlayerSignals.Instance.onPlayerAlive += OnPlayerAlive;
            AiSignals.Instance.onGroundMinePlanted += OnGroundMinePlanted;
            AiSignals.Instance.onGroundMineExplode += OnGroundMineExplode;
            if (_isDeath)
            {
                OnAlive();
            }
        }

        private void OnDisable()
        {
            PlayerSignals.Instance.onPlayerDead -= OnPlayerDead;
            PlayerSignals.Instance.onPlayerAlive -= OnPlayerAlive;
            AiSignals.Instance.onGroundMinePlanted -= OnGroundMinePlanted;
            AiSignals.Instance.onGroundMineExplode -= OnGroundMineExplode;
            
            if (_isDeath)
            {
                _isAlive = false;
            }
        }

        private void OnGroundMinePlanted(Transform groundMine)
        {
            _isGroundMineActivated = true;
            GroundMineTarget = groundMine;
        }

        private void OnGroundMineExplode()
        {
            CurrentTarget = BaseTarget;
            _isGroundMineActivated = false;
            if(!IsInGroundMineArea) return;
            TakeDamage(100);
        }

        private void OnPlayerDead()
        {
            _isPlayerDead = true;
            CurrentTarget = BaseTarget;
        }

        private void OnPlayerAlive()
        {
            _isPlayerDead = false;
        }
        
        private void OnAlive()
        {
            _health = _enemyData.Health;
            IsInGroundMineArea = false;
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
        
        private async void OnDeath()
        {
            DropMoney();
            AiSignals.Instance.onEnemyDead?.Invoke(transform);
            AiSignals.Instance.onEnemyAIDead?.Invoke(this);
            _isDeath = true;
            navMeshAgent.enabled = false;
            CanAttackToBase = false;
            transform.DOMoveY(-.5f, .2f);
            ChangeSaturation(.25f, .25f, .65f);
            await Task.Delay(2500);
            PoolSignals.Instance.onReleasePoolObject?.Invoke($"{EnemyType}", gameObject);
            await Task.Delay(100);
            animator.SetTrigger(Idle);
        }
        
        private void DropMoney()
        {
            Vector3 enemyTransform = transform.position;

            for (int i = 0; i < 3; i++)
            {
                GameObject money = PoolSignals.Instance.onGetPoolObject?.Invoke("Money", transform);
                Vector3 pos1 = new Vector3(enemyTransform.x + Random.Range(-1.4f, 1.4f), 0, enemyTransform.z + Random.Range(-1.4f, 1.4f));
                money.transform.DOJump(pos1, .5f, 2, .2f);
            }
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