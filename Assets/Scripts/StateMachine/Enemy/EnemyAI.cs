using System;
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
        public bool CanAttack;

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

            var moveToBase = new MoveToBase(this, animator, navMeshAgent, BaseTarget);
            var chasePlayer = new Chase(this, animator, navMeshAgent);
            var attack = new Attack(this, animator, navMeshAgent, navMeshObstacle, _damage);
            var death = new Death(this, animator, EnemyType);

            At(moveToBase, chasePlayer, CanChasePlayer());
            At(chasePlayer, attack, IsInAttackRange());
            At(attack, chasePlayer, CanChasePlayer());
            At(chasePlayer, moveToBase, GoBase());
            At(moveToBase, attack, IsAtBase());
            At(death, moveToBase, IsAlive());
            
            _stateMachine.AddAnyTransition(death, IsDeath());
            
            _stateMachine.SetState(moveToBase);
            
            void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);

            Func<bool> GoBase() => () => CurrentTarget == BaseTarget;
            Func<bool> CanChasePlayer() => () => CurrentTarget = PlayerTarget;
            Func<bool> IsInAttackRange() => () =>
                CurrentTarget == PlayerTarget && Vector3.Distance(transform.position, CurrentTarget.position) <= navMeshAgent.stoppingDistance;
            Func<bool> IsAtBase() => () => Vector3.Distance(transform.position, BaseTarget.position) < navMeshAgent.stoppingDistance;
            Func<bool> IsDeath() => () => _health <= 0;
            Func<bool> IsAlive() => () => _health > 0;
        }
        
        private void Update()
        {
            if(!_isAlive) return;
            _stateMachine.Tick();
            // if (CurrentTarget == PlayerTarget)
            // {
            //     print(Vector3.Distance(transform.position, PlayerTarget.position));
            // }
        }

        public void SetTarget(Transform target)
        {
            CurrentTarget = target;
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
        
        private async void OnDeath()
        {
            _isDeath = true;
            navMeshAgent.enabled = false;
            transform.DOMoveY(-.5f, .2f);
            ChangeSaturation(.25f, .25f, .5f);
            await Task.Delay(200);
            PoolSignals.Instance.onReleasePoolObject?.Invoke($"{EnemyType}", gameObject);
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