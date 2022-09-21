using System;
using System.Collections.Generic;
using Commands;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Signals;
using StateMachine;
using StateMachine.Exclusive;
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
            StackSignals.Instance.onAddStack += OnAddStack;
            StackSignals.Instance.onRemoveStack += OnRemoveStack;
            StackSignals.Instance.onRemoveAllStack += OnRemoveAllStack;
        }
        
        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onPlay -= OnPlay;
            StackSignals.Instance.onAddStack -= OnAddStack;
            StackSignals.Instance.onRemoveStack -= OnRemoveStack;
            StackSignals.Instance.onRemoveAllStack += OnRemoveAllStack;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion

        #region Event Functions

        private void OnAddStack(Transform collected)
        {
            collected.gameObject.tag = "Rescued";
            _addStackCommand.Execute(collected);
        }

        private void OnRemoveStack(Transform collected)
        {
            collected.SetParent(AiSignals.Instance.onGetMineBaseArea());
            _removeStackCommand.Execute(collected);
        }

        private void OnRemoveAllStack(HostageType hostageType)
        {
            switch (hostageType)
            {
                case HostageType.Miner:

                    // for (int i = 0; i < hostageList.Count; i++)
                    // {
                    //     hostageList[i].SetParent(AiSignals.Instance.onGetMineBaseArea());
                    //     hostageList[i].TryGetComponent(out HostageManager manager);
                    //     manager.MakeMeAMiner();
                    //     _removeStackCommand.Execute(hostageList[i]);
                    // }
                    
                    foreach (var hostage in hostageList)
                    {
                        hostage.SetParent(AiSignals.Instance.onGetMineBaseArea());
                        hostage.TryGetComponent(out HostageManager manager);
                        manager.MakeMeAMiner();
                    }
                    
                    hostageList.Clear();

                    break;
                case HostageType.Soldier:
                    foreach (var hostage in hostageList)
                    {
                        hostage.TryGetComponent(out HostageManager manager);
                        manager.MakeMeASoldier();
                    }
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(hostageType), hostageType, null);
            }
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