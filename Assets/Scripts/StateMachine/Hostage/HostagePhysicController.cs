using System;
using Signals;
using UnityEngine;

namespace StateMachine.Hostage
{
    public class HostagePhysicController : MonoBehaviour
    {
        [SerializeField] private HostageManager _manager;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _manager.IsRescued = true;
                AiSignals.Instance.onHostageRescued?.Invoke(transform);
            }
            if (other.CompareTag("MineArea"))
            {
                _manager.MakeMeAMiner();
            }

            if (other.CompareTag("MilitaryBase"))
            {
                _manager.MakeMeACandidateSoldier();
            }
            
            if (other.CompareTag("MilitaryTent"))
            {
                _manager.ReturnMeToPool();
            }
        }
    }
}