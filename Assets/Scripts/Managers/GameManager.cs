using Enums;
using Signals;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Self Variables
    
        #region Public Variables
    
        public GameStates States;
    
        #endregion Public Variables

        #region Serialized

        #endregion

        #region Private
        
        #endregion
        
        #endregion Self Variables
    
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        #region Event System
        
        private void OnEnable()
        {
            SubscribeEvents();
        }
    
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onChangeGameState += OnChangeGameState;
            CoreGameSignals.Instance.onReset += OnReset;
        }
    
        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onChangeGameState -= OnChangeGameState;
            CoreGameSignals.Instance.onReset -= OnReset;
        }
    
        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion
        
        private void OnPlay()
        {
        }
    
        private void OnChangeGameState(GameStates newState)
        {
            States = newState;
        }
        
        private GameStates OnGetGameState() => States;
        

        private void OnReset()
        {
        }
    }
}