using Enums;
using Keys;
using DG.Tweening;
using UnityEngine;
using Managers;

namespace Controllers
{
    public class PlayerMovementController : MonoBehaviour
    {
        #region Self Variables

        #region Public
        
        #endregion

        #region Serialized

        [SerializeField] private Rigidbody rigidBody;
        [SerializeField] private PlayerManager manager;
        
        #endregion

        #region Private

        private PlayerMovementData _movementData;
        private bool _isReadyToMove = true, _isReadyToPlay = true;
        private Vector3 _movementDirection;

        #endregion
        
        #endregion
        
        public void SetMovementData(PlayerMovementData movementData) => _movementData = movementData;
        public void ActivateMovement() => _isReadyToMove = true;
        public void DeactivateMovement() => _isReadyToMove = false;
        public void UpdateInputValue(InputParams inputParam) => _movementDirection = inputParam.movementVector;
        public void IsReadyToPlay(bool state) => _isReadyToPlay = state;
        private void FixedUpdate()
        {
            if (_isReadyToPlay)
            {
                if (_isReadyToMove)
                {
                    Move();
                }
                else
                {
                    Stop();
                }
            }
            else
                Stop();
        }
        
        private void Move()
        {
            Vector3 velocity = rigidBody.velocity;
            velocity = new Vector3(_movementDirection.x * _movementData.MoveSpeed, velocity.y,
                _movementDirection.z * _movementData.MoveSpeed);
            rigidBody.velocity = velocity;

            if (_movementDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(_movementDirection);
                transform.rotation = toRotation;
            }
        }

        private void Stop()
        {
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
        }

        public  void MovementReset()
        {
            Stop();
            _isReadyToPlay = false;
            _isReadyToMove = false;
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }
        public void OnReset()
        {
            DOTween.KillAll();
        }
    }
}