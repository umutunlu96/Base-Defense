using System.Collections.Generic;
using Commands;
using Data.UnityObject;
using Data.ValueObject;
using Signals;
using UnityEngine;

namespace Managers
{
    public class StackManager : MonoBehaviour
    {
        #region Variables

        #region Serialized
        
        [SerializeField] private List<Transform> hostageList = new List<Transform>();
        
        #endregion

        #region Private
        
        private Transform _transform;
        private LerpData _lerpData;
        private Transform _playerTransform;
        private StackLerpMoveCommand _stackLerpMoveCommand;
        private AddStackCommand _addStackCommand;
        private RemoveStackCommand _removeStackCommand;
        
        #endregion
        
        #endregion
        private LerpData GetLerpData() => Resources.Load<CD_Lerp>("Data/CD_Lerp").Data;
        
        #region EventSubscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onPlay += OnPlay;
            StackSignals.Instance.onAddStack += _addStackCommand.Execute;
            StackSignals.Instance.onRemoveStack += _removeStackCommand.Execute;
        }
        
        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onPlay -= OnPlay;
            StackSignals.Instance.onAddStack -= _addStackCommand.Execute;
            StackSignals.Instance.onRemoveStack -= _removeStackCommand.Execute;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion
        
        private void Awake()
        {
            Initialize();
        }
        private void Initialize()
        {
            _transform = this.transform;
            _lerpData = GetLerpData();
            _addStackCommand = new AddStackCommand(ref hostageList, ref _transform);
            _removeStackCommand = new RemoveStackCommand(ref hostageList);
        }
        
        private void FixedUpdate()
        {
            if(_playerTransform == null) return;
            _stackLerpMoveCommand.Execute();
        }

        private void OnPlay()
        {
            _playerTransform = PlayerSignals.Instance.onGetPlayerTransfrom();
            _stackLerpMoveCommand = new StackLerpMoveCommand(ref hostageList, ref _lerpData, _playerTransform);
        }
    }
}