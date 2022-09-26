using Controllers;
using Keys;
using Signals;
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
        [SerializeField] private PlayerAnimationController animationController;
        [SerializeField] private Transform playerDetection;
        
        #endregion Seriliazed Field

        #region Private

        private PlayerData _playerData;
        private bool _isPlayerMoving;
        private bool _isAtOutside;
        #endregion Private

        
        #endregion Self Variables

        private void Awake()
        {
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

        private void OnPlay()
        {
            movementController.IsReadyToPlay(true);
        }
        
        private void OnPointerDown()
        {
            ActivateMovement();
            _isPlayerMoving = true;
            //animation controller set run animation
        }

        private void OnInputReleased()
        {
            DeactivateMovement();
            _isPlayerMoving = false;
            //animation controller set idle animation
        }

        private void OnInputDragged(InputParams inputParams) => movementController.UpdateInputValue(inputParams);
        
        private void ActivateMovement() => movementController.ActivateMovement();

        public void DeactivateMovement() => movementController.DeactivateMovement();
        
        // private void OnTranslatePlayerAnimationState(AnimationStateMachine state)
        // {
        //     animationController.TranslatePlayerAnimationState(state);
        // }

        public void OnEnterGate()
        {
            _isAtOutside = !_isAtOutside;
            if (_isAtOutside)
            {
                int layerIgnoreRaycastInside = LayerMask.NameToLayer("PlayerDetection");
                playerDetection.gameObject.layer = layerIgnoreRaycastInside;
                return;
            }
            int layerIgnoreRaycastOutside = LayerMask.NameToLayer("Empty");
            playerDetection.gameObject.layer = layerIgnoreRaycastOutside;
        }
        
        private Transform OnGetPlayerTransform() => transform;

        private float OnGetPlayerSpeed() => rigidBody.velocity.magnitude;
        
        private void OnLevelFailed() => movementController.IsReadyToPlay(false);
        
        private void OnReset()
        {
            movementController.MovementReset();
            movementController.OnReset();
        }
    }
}