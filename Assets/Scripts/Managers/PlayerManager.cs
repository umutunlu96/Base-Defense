using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Controllers;
using Enums;
using Keys;
using Signals;
using StateMachine;
using UnityEngine;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        public int Health = 100;
        public bool InBombArea = false;
        
        [SerializeField] private Rigidbody rigidBody;
        [SerializeField] private PlayerMovementController movementController;
        [SerializeField] private PlayerPhysicsController physicsController;
        [SerializeField] private PlayerHealthController healthController;
        [SerializeField] private PlayerAimController aimController;
        [SerializeField] private PlayerAnimationController animationController;
        [SerializeField] private Transform attackRadius;
        [SerializeField] private PlayerStackController stackController;

        private PlayerData _playerData;
        private bool _isPlayerMoving;
        private bool _isPlayerUsingTurret;
        private bool _isAtOutside;
        private Transform _parent;
        private WeaponType _weaponType;
        private bool _isAlive = true;

        private void Awake()
        {
            _parent = transform.parent;
            _playerData = GetPlayerData();
            SetPlayerDataToControllers();
            OnEnterBase();
        }

        private PlayerData GetPlayerData() => Resources.Load<CD_Player>("Data/CD_Player").Data;

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onReset += OnReset;
            
            InputSignals.Instance.onInputTaken += OnPointerDown;
            InputSignals.Instance.onInputReleased += OnInputReleased;
            InputSignals.Instance.onInputDragged += OnInputDragged;

            PlayerSignals.Instance.onIsPlayerMoving += OnCanBuy;
            PlayerSignals.Instance.onGetPlayerTransfrom += OnGetPlayerTransform;
            PlayerSignals.Instance.onGetPlayerSpeed += OnGetPlayerSpeed;
            PlayerSignals.Instance.onPlayerWeaponTypeChanged += OnWeaponTypeChanged;
            PlayerSignals.Instance.onPlayerEnterTurretArea += OnPlayerUseTurret;
            PlayerSignals.Instance.onPlayerLeaveTurretArea += OnPlayerLeaveTurret;
            PlayerSignals.Instance.onTakeDamage += OnTakeDamage;
            PlayerSignals.Instance.onReturnHealth += OnReturnHealth;
            PlayerSignals.Instance.onPlayerDeadAnimComplete += OnAlive;

            AiSignals.Instance.onGrenadeExplode += OnGranadeExplode;
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onReset -= OnReset;

            InputSignals.Instance.onInputTaken -= OnPointerDown;
            InputSignals.Instance.onInputReleased -= OnInputReleased;
            InputSignals.Instance.onInputDragged -= OnInputDragged;

            PlayerSignals.Instance.onIsPlayerMoving -= OnCanBuy;
            PlayerSignals.Instance.onGetPlayerTransfrom -= OnGetPlayerTransform;
            PlayerSignals.Instance.onGetPlayerSpeed -= OnGetPlayerSpeed;
            PlayerSignals.Instance.onPlayerWeaponTypeChanged -= OnWeaponTypeChanged;
            PlayerSignals.Instance.onPlayerEnterTurretArea -= OnPlayerUseTurret;
            PlayerSignals.Instance.onPlayerLeaveTurretArea -= OnPlayerLeaveTurret;
            PlayerSignals.Instance.onTakeDamage -= OnTakeDamage;
            PlayerSignals.Instance.onReturnHealth -= OnReturnHealth;
            PlayerSignals.Instance.onPlayerDeadAnimComplete -= OnAlive;
            
            AiSignals.Instance.onGrenadeExplode += OnGranadeExplode;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion Event Subsicription

        private bool OnCanBuy() => _isPlayerMoving;

        private int OnReturnHealth() => Health;

        private void OnGranadeExplode()
        {
            if(!InBombArea) return;
            OnTakeDamage(20);
        }
        
        private void OnTakeDamage(int damage)
        {
            Health -= damage;
            healthController.SetHealthBar(Health);
            if (Health <= 0 && _isAlive)
            {
                PlayerSignals.Instance.onPlayerDead?.Invoke();
                Health = 0;
                OnDeath();
            }
        }
        
        private void SetPlayerDataToControllers()
        {
            movementController.SetMovementData(_playerData.playerMovementData);
        }

        private void OnPlay() => movementController.IsReadyToPlay(true);
        
        private void OnPointerDown()
        {
            if(_isPlayerUsingTurret) return;
            ActivateMovement();
            _isPlayerMoving = true;
        }

        private void OnInputReleased()
        {
            if(_isPlayerUsingTurret) return;
            DeactivateMovement();
            _isPlayerMoving = false;
            animationController.SetSpeed(0);
        }

        private void OnInputDragged(InputParams inputParams)
        {
            if(_isPlayerUsingTurret) return;
            animationController.SetSpeed(inputParams.movementVector.magnitude);
            movementController.UpdateInputValue(inputParams);
        }

        private void OnPlayerUseTurret()
        {
            _isPlayerUsingTurret = true;
            animationController.SetSpeed(0);
            animationController.TranslatePlayerAnimationState(PlayerAnimationState.Idle);
            DeactivateMovement();
        }

        private void OnPlayerLeaveTurret()
        {
            _isPlayerUsingTurret = false;
            transform.SetParent(_parent);
            ActivateMovement();
        }
        
        private void ActivateMovement() => movementController.ActivateMovement();

        public void DeactivateMovement() => movementController.DeactivateMovement();
        
        public void OnEnterBase()
        {
            AiSignals.Instance.onPlayerIsAtOutside?.Invoke(false);
            int layerIgnoreRaycastOutside = LayerMask.NameToLayer("Empty");
            physicsController.gameObject.layer = layerIgnoreRaycastOutside;
            attackRadius.gameObject.layer = layerIgnoreRaycastOutside;
            animationController.DisableAimLayer();
            aimController.DisableAimRig();
            if (Health < 100)
            {
                healthController.gameObject.SetActive(true);
                healthController.Heal();
            }
            else
                healthController.gameObject.SetActive(false);
        }
        
        public void OnExitBase()
        {
            AiSignals.Instance.onPlayerIsAtOutside?.Invoke(true);
            int layerIgnoreRaycastInsidePlayer = LayerMask.NameToLayer("Player");
            int layerIgnoreRaycastInsideAttackRadius = LayerMask.NameToLayer("PlayerAttackRadius");
            physicsController.gameObject.layer = layerIgnoreRaycastInsidePlayer;
            attackRadius.gameObject.layer = layerIgnoreRaycastInsideAttackRadius;
            animationController.EnableAimLayer();
            aimController.EnableAimRig(_weaponType);
            DropAllAmmoToGround();
            healthController.gameObject.SetActive(true);
        }
        
        public void StackMoney(Transform money) => stackController.StackMoney(money);

        public void StackAmmo() => stackController.StackAmmo();

        public void StopStackAmmo() => stackController.CanStackAmmo = false;
        
        public void DropMoneyToBase() => stackController.DropAllMoney();
        
        public void DropAmmoToTurret(TurretManager turretManager) => stackController.DropAmmo(turretManager);

        public void StopDropAmmoToTurret() => stackController.CanDropAmmo = false;

        public void DropAllAmmoToGround() => stackController.DropAllAmmoToGround();

        private Transform OnGetPlayerTransform() => transform;
        
        private void OnWeaponTypeChanged(WeaponType weaponType) => _weaponType = weaponType;

        private float OnGetPlayerSpeed() => rigidBody.velocity.magnitude;
        
        private void OnLevelFailed() => movementController.IsReadyToPlay(false);

        private void OnDeath()
        {
            _isAlive = false;
            animationController.DisableAimLayer();
            aimController.DisableAimRig();
            aimController.StopAttack();
            animationController.TranslatePlayerAnimationState(PlayerAnimationState.Death);
        }

        private void OnAlive()
        {
            PlayerSignals.Instance.onPlayerAlive?.Invoke();
            _isAlive = true;
            animationController.TranslatePlayerAnimationState(PlayerAnimationState.Idle);
            transform.position = Vector3.zero;
            OnEnterBase();
        }

        public async void OnNextLevel()
        {
            await Task.Delay(2000);
            LevelSignals.Instance.onNextLevel?.Invoke();
        }
        
        private void OnReset()
        {
            OnEnterBase();
            movementController.MovementReset();
            movementController.OnReset();
            healthController.ResetHealthBar();
        }
    }
}