using System;
using Enums;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace StateMachine.Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        #region Variables

        #region Public

        public EnemyType EnemyType;
        public Transform CurrentTarget;
        public int Health;
        public float ChaseRange = 5f;
        public float AttackRange = 2.5f;
        #endregion

        #region Serialized
        //Datalastir
        [SerializeField] private float chaseUpdateSpeed = 0.2f;
        [SerializeField] private float walkSpeed = 1.5f;
        [SerializeField] private float runSpeed = 2f;
        //
        #endregion

        #region Private

        private StateMachine _stateMachine;
        private NavMeshAgent _navMeshAgent;
        private NavMeshObstacle _navMeshObstacle;
        private Animator _animator;
        private Transform _baseTarget;
        private Transform _playerTarget;
        
        // private bool _canChase;
        private bool _canAttack;
        private bool _reachedAtTheBase;
        private bool _attacked;
        private bool _attackAnimEnded;
        #endregion
        
        #endregion

        public bool CanAttack { get { return _canAttack; } set { _canAttack = value; } }
        
        public bool Attacked { get { return _attacked; } set { _attacked = value; } }
        
        public bool AttackAnimEnded { get { return _attackAnimEnded; } set { _attackAnimEnded = value; } }
        
        public bool ReachedAtBase { get { return _reachedAtTheBase; } set { _reachedAtTheBase = value; } }
        
        public Transform BaseTarget { get { return _baseTarget; } set { _baseTarget = value; } }
        
        public Transform PlayerTarget { get { return _playerTarget; } set { _playerTarget = value; } }
        
        //Datalastir bunleri
        
        public float WalkSpeed { get { return walkSpeed; } set { walkSpeed = value; } }
        
        public float RunSpeed { get { return runSpeed; } set { runSpeed = value; } }
        //
        
        private void Start()
        {
            InitAI();
        }
        
        private void InitAI()
        {
            _animator = GetComponentInChildren<Animator>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshObstacle = GetComponent<NavMeshObstacle>();
            _stateMachine = new StateMachine();
            BaseTarget = AiSignals.Instance.onGetBaseAttackPoint();

            var moveToBase = new MoveToBase(this, _animator, _navMeshAgent, BaseTarget);
            var chasePlayer = new Chase(this, _animator, _navMeshAgent, chaseUpdateSpeed);
            var attack = new Attack(this, _animator, _navMeshAgent, _navMeshObstacle);
            var reachedBase = new ReachedBase(this, _animator, _navMeshAgent, _navMeshObstacle);
            var death = new Death(this, _animator);

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
            Func<bool> IsDeath() => () => Health <= 0;
        }

        private void Update() => _stateMachine.Tick();

        public void AttackedToPlayer()
        {
            if (Attacked == true)
            {
                //PlayerTakeDamage
                Debug.Log("PlayerTookDamage");
                Attacked = false;
            }
        }
        
    }
}