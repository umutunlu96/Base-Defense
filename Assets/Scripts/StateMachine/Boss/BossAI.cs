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

namespace StateMachine.Boss
{
    public class BossAI : MonoBehaviour, IDamageable
    {
        [SerializeField] private BossAnimationController animationController;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform hand;
        [SerializeField] private GameObject targetCircle;
        [SerializeField] private SkinnedMeshRenderer sMeshRenderer;
        [ShowInInspector] private bool _isPlayerInRange;
        private StateMachine _stateMachine;
        private EnemyData _data;
        private Transform _bombInitialTransform;
        private Transform _playerTransform;
        private float _health;
        private float _damage;
        
        private void Start()
        {
            _data = GetData();
            SetData();
            InitAI();
        }

        private EnemyData GetData() => Resources.Load<CD_Enemy>("Data/CD_Enemy").EnemyDatas[EnemyType.Boss];

        private void SetData()
        {
            _health = _data.Health;
            _damage = _data.Damage;
            _bombInitialTransform = animationController.ReturnGrenadeInitialPos();
        }
        
        private void InitAI()
        {
            _stateMachine = new StateMachine();
            _playerTransform = PlayerSignals.Instance.onGetPlayerTransfrom();
            
            var stationary = new Stationary(animator);
            var attack = new Attack(this, animator, _playerTransform, hand, _bombInitialTransform);
            var death = new Death(this, animator);

            At(stationary, attack, CanAttack());
            At(attack, stationary, CantAttack());
            
            _stateMachine.SetState(stationary);
            _stateMachine.AddAnyTransition(death, IsDead());
            
            void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);
            
            Func<bool> IsDead() => () => _health <= 0;
            Func<bool> CanAttack() => () => _isPlayerInRange;
            Func<bool> CantAttack() => () => !_isPlayerInRange;
        }

        private void Update()
        {
            _stateMachine.Tick();
        }

        #region EventSubscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            AiSignals.Instance.onPlayerEnterBossArea += OnPlayerEnterRange;
            AiSignals.Instance.onPlayerLeaveBossArea += OnPlayerExitRange;
            AiSignals.Instance.onGrenadeSpawned += OnGrenadeSpawned;
            AiSignals.Instance.onGrenadeThrowed += OnGrenadeThrowed;
            AiSignals.Instance.onGrenadeExplode += OnGrenadeExplode;
        }
        
        private void UnSubscribeEvents()
        {
            AiSignals.Instance.onPlayerEnterBossArea -= OnPlayerEnterRange;
            AiSignals.Instance.onPlayerLeaveBossArea -= OnPlayerExitRange;
            AiSignals.Instance.onGrenadeSpawned -= OnGrenadeSpawned;
            AiSignals.Instance.onGrenadeThrowed -= OnGrenadeThrowed;
            AiSignals.Instance.onGrenadeExplode -= OnGrenadeExplode;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion

        #region Event Functions

        private void OnPlayerEnterRange() => _isPlayerInRange = true;
        
        private void OnPlayerExitRange() => _isPlayerInRange = false;
        
        private void OnGrenadeSpawned(Grenade grenade) => animationController.grenade = grenade;
        
        private void OnGrenadeThrowed()
        {
            targetCircle.SetActive(true);
            Vector3 playerPos = PlayerSignals.Instance.onGetPlayerTransfrom().position;
            targetCircle.transform.position = playerPos;
        }
        
        private void OnGrenadeExplode() => targetCircle.SetActive(false);
        
        #endregion

        public void TakeDamage(float damage)
        {
            _health -= damage;
        }

        public async void OnDeath()
        {
            targetCircle.SetActive(false);
            transform.DOMoveY(-.5f, .2f);
            ChangeSaturation(.25f, 1, 1.5f);
            AiSignals.Instance.onEnemyAIDead?.Invoke(this);
            AiSignals.Instance.onBossDead?.Invoke();
            await Task.Delay(3600);
            gameObject.SetActive(false);
            PoolSignals.Instance.onResetPool?.Invoke();
        }
        
        private void ChangeSaturation(float saturation, float brightness, float duration)
        {
            sMeshRenderer.material.DOFloat(saturation,"_Saturation", duration);
            sMeshRenderer.material.DOFloat(brightness,"_Brightness", duration);
        }
        public Transform GetTransform() => this.transform;

        public bool AmIDeath() => _health <= 0;
    }
}