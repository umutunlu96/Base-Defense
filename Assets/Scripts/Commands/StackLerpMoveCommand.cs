using System.Collections.Generic;
using Data.ValueObject;
using Signals;
using UnityEngine;

namespace Commands
{
    public class StackLerpMoveCommand
    {
        private List<Transform> _hostage;
        private LerpData _lerpData;
        private Transform _playerTransform;

        public StackLerpMoveCommand(ref List<Transform> hostage, ref LerpData lerpData, Transform playerTransform)
        {
            _hostage = hostage;
            _lerpData = lerpData;
            _playerTransform = playerTransform;
        }

        public void Execute()
        {
            if(_hostage.Count == 0) return;
            
            float actualDistance = Vector3.Distance(_hostage[0].transform.position, _playerTransform.position);
            
            if (_hostage.Count > 0 && _playerTransform != null && actualDistance > _lerpData.DistanceOffSet)
            {
                var playerPosition = _playerTransform.position;
                var followToPlayer = (_hostage[0].position - playerPosition).normalized;
                followToPlayer.Scale(new Vector3(_lerpData.DistanceOffSet,_lerpData.DistanceOffSet,_lerpData.DistanceOffSet));
                _hostage[0].position = playerPosition + followToPlayer;
                Vector3 targetDirection = playerPosition - _hostage[0].position;
                float singleStep = _lerpData.LerpSpeed * Time.deltaTime;
                Vector3 newDirection = Vector3.RotateTowards(_hostage[0].forward, targetDirection, singleStep, 0f);
                _hostage[0].rotation = Quaternion.LookRotation(newDirection);
                
                if(_hostage.Count == 1) return;
                
                for (int i = 1; i < _hostage.Count; i++)
                {
                    float actualDistanceToNextHostage = Vector3.Distance(_hostage[i].transform.position, _hostage[i-1].position);
                    if(actualDistanceToNextHostage< _lerpData.DistanceOffSet) return;
                    var followToNextHostage = (_hostage[i].position - _hostage[i-1].position).normalized;
                    followToNextHostage.Scale(new Vector3(_lerpData.DistanceOffSet,_lerpData.DistanceOffSet,_lerpData.DistanceOffSet));
                    _hostage[i].position = _hostage[i-1].position + followToNextHostage;
                    Vector3 nextHostageDirection = _hostage[i-1].position - _hostage[i].position;
                    float singleStepHostage = _lerpData.LerpSpeed * Time.deltaTime;
                    Vector3 newHostageDirection = Vector3.RotateTowards(_hostage[i].forward, nextHostageDirection, singleStepHostage, 0f);
                    _hostage[i].rotation = Quaternion.LookRotation(newHostageDirection);
                }
            }
        }
    }
}