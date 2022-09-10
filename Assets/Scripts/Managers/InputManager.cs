using Data.UnityObject;
using Data.ValueObject;
using Keys;
using Signals;
using UnityEngine;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        [Header("Data")] public InputData Data;

        #endregion

        #region Serialized Variables

        [SerializeField] private bool isReadyForTouch, isFirstTimeTouchTaken;
        [SerializeField] private FloatingJoystick floatingJoystick;
        #endregion
        
        #region Private Variables

        private bool _isTouching;
        private Vector3 _moveVector;

        #endregion

        #endregion
        
        private void Awake()
        {
            Data = GetInputData();
        }
        
        private InputData GetInputData() => Resources.Load<CD_Input>("Data/CD_Input").InputData;

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            InputSignals.Instance.onEnableInput += OnEnableInput;
            InputSignals.Instance.onDisableInput += OnDisableInput;
            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onReset += OnReset;
        }

        private void UnsubscribeEvents()
        {
            InputSignals.Instance.onEnableInput -= OnEnableInput;
            InputSignals.Instance.onDisableInput -= OnDisableInput;
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onReset -= OnReset;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion
        
        void Update()
        {
            if (!isReadyForTouch) return;
            if (Input.GetMouseButtonUp(0)) 
            {
                _isTouching = false;
                InputSignals.Instance.onInputReleased?.Invoke();
            }
                
            if (Input.GetMouseButtonDown(0))
            {
                _isTouching = true;
                InputSignals.Instance.onInputTaken?.Invoke();
                if (!isFirstTimeTouchTaken)
                {
                    isFirstTimeTouchTaken = true;
                    InputSignals.Instance.onFirstTimeTouchTaken?.Invoke();
                }
            }
            
            if (Input.GetMouseButton(0))
            {
                if (_isTouching)
                {
                    InputSignals.Instance.onInputDragged?.Invoke(new InputParams()
                    {
                        movementVector = new Vector3(floatingJoystick.Horizontal, 0, floatingJoystick.Vertical).normalized
                    });
                }
            }
        }
        
        private void OnEnableInput() => isReadyForTouch = true;

        private void OnDisableInput() => isReadyForTouch = false;

        private void OnPlay() => isReadyForTouch = true;
            
        private void OnReset()
        {
            _isTouching = false;
            isReadyForTouch = false;
            isFirstTimeTouchTaken = false;
        }
    }
}