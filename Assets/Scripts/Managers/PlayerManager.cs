using System.Collections;
using System.Collections.Generic;
using Controllers;
using Enums;
using Keys;
using Signals;
using Sirenix.OdinInspector.Editor.Drawers;
using UnityEngine;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        #region Self Variables

        #region Seriliazed Field

        [SerializeField] private Rigidbody rigidBody;
        [SerializeField] private PlayerMovementController movementController;
        [SerializeField] private PlayerPhysicsController physicsController;
        [SerializeField] private PlayerMeshController meshController;
        [SerializeField] private PlayerAimController aimController;
        [SerializeField] private PlayerAnimationController animationController;
        [SerializeField] private Transform enemyDetection;
        
        #endregion Seriliazed Field

        #region Private

        private PlayerData _playerData;
        private bool _isPlayerMoving;
        private bool _isPlayerUsingTurret;
        private bool _isAtOutside;
        private Transform _parent;
        
        #endregion Private

        
        #endregion Self Variables

        private void Awake()
        {
            _parent = transform.parent;
            _playerData = GetPlayerData();
            SetPlayerDataToControllers();
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
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion Event Subsicription

        private bool OnCanBuy() => _isPlayerMoving;
        
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
            animationController.TranslatePlayerAnimationState(PlayerAnimationState.Run);
        }

        private void OnInputReleased()
        {
            if(_isPlayerUsingTurret) return;
            DeactivateMovement();
            _isPlayerMoving = false;
            animationController.TranslatePlayerAnimationState(PlayerAnimationState.Idle);
        }

        private void OnInputDragged(InputParams inputParams) => movementController.UpdateInputValue(inputParams);

        private void OnPlayerUseTurret()
        {
            _isPlayerUsingTurret = true;
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
        
        public void OnEnterGate()
        {
            _isAtOutside = !_isAtOutside;
            if (_isAtOutside)
            {
                int layerIgnoreRaycastInsidePlayer = LayerMask.NameToLayer("Player");
                int layerIgnoreRaycastInsideAttackRadius = LayerMask.NameToLayer("PlayerAttackRadius");
                transform.gameObject.layer = layerIgnoreRaycastInsidePlayer;
                enemyDetection.gameObject.layer = layerIgnoreRaycastInsideAttackRadius;
                animationController.EnableAimLayer();
                aimController.EnableAimRig(true);
                return;
            }
            int layerIgnoreRaycastOutside = LayerMask.NameToLayer("Empty");
            transform.gameObject.layer = layerIgnoreRaycastOutside;
            enemyDetection.gameObject.layer = layerIgnoreRaycastOutside;
            animationController.DisableAimLayer();
            aimController.EnableAimRig(false);
        }
        
        private Transform OnGetPlayerTransform() => transform;
        
        private void OnWeaponTypeChanged(WeaponType weaponType) => aimController.ChangeWeaponRigPos(weaponType);

        private float OnGetPlayerSpeed() => rigidBody.velocity.magnitude;
        
        private void OnLevelFailed() => movementController.IsReadyToPlay(false);
        
        private void OnReset()
        {
            movementController.MovementReset();
            movementController.OnReset();
        }
    }
}