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
        public int ChaseRange = 10;
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
        private bool _canChase;

        #endregion
        
        #endregion
        
        public bool CanChase { get { return _canChase; } set { _canChase = value; } }
        
        public Transform BaseTarget { get { return _baseTarget; } set { _baseTarget = value; } }
        
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

            var search = new Search(this);
            var move = new Move(this, _animator, _navMeshAgent);
            var chase = new Chase(this, _animator, _navMeshAgent, chaseUpdateSpeed);
            var attack = new Attack(this, _animator, _navMeshAgent, _navMeshObstacle);
            var death = new Death(this, _animator);
            
            At(search,move,HasTarget());
            At(move,chase,CanChasePlayer());
            At(chase,attack, IsInAttackRange());
            At(attack,chase, IsNotInAttackRange());
            At(chase,search, CantChasePlayer());
            
            _stateMachine.AddAnyTransition(death,IsDeath());
            
            _stateMachine.SetState(search);
            
            void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);
            Func<bool> HasTarget() => () => CurrentTarget != null;
            Func<bool> CanChasePlayer() => () => CanChase;
            Func<bool> IsInAttackRange() => () => Vector3.Distance(transform.position, CurrentTarget.transform.position) < 1
                                                  && Vector3.Distance(transform.position, CurrentTarget.transform.position) > .5f;
            Func<bool> IsNotInAttackRange() => () => Vector3.Distance(transform.position, CurrentTarget.transform.position) > 1;
            Func<bool> CantChasePlayer() => () => !CanChase;
            Func<bool> IsDeath() => () => Health <= 0;
        }

        private void Update() => _stateMachine.Tick();
    }
}