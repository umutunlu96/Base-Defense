using System;
using Cinemachine;
using Signals;
using UnityEngine;

namespace Managers
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private Animator cameraAnimator;
        [SerializeField] private CinemachineVirtualCamera inGameCam;
        [SerializeField] private CinemachineVirtualCamera turretUseCam;
        
        #region EventSubscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onPlay += GetPlayer;
            LevelSignals.Instance.onRestartLevel += GetPlayer;
            PlayerSignals.Instance.onPlayerUseTurret += OnPlayerUseTurret;
            PlayerSignals.Instance.onPlayerLeaveTurret += OnPlayerLeaveTurret;
        }
        
        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onPlay -= GetPlayer;
            LevelSignals.Instance.onRestartLevel -= GetPlayer;
            PlayerSignals.Instance.onPlayerUseTurret -= OnPlayerUseTurret;
            PlayerSignals.Instance.onPlayerLeaveTurret -= OnPlayerLeaveTurret;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion

        #region Event Functions

        private void OnPlayerUseTurret()
        {
            cameraAnimator.Play("UseTurret");
        }
        
        private void OnPlayerLeaveTurret()
        {
            cameraAnimator.Play("InGame");
        }

        private void GetPlayer()
        {
            inGameCam.Follow = PlayerSignals.Instance.onGetPlayerTransfrom();
            turretUseCam.Follow = PlayerSignals.Instance.onGetPlayerTransfrom();
        }

        #endregion
    }
}