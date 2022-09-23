using System;
using UnityEngine;

namespace StateMachine.Hostage
{
    public class HostagePhysicController : MonoBehaviour
    {
        [SerializeField] private HostageManager _manager;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                _manager.IsRescued = true;
            if (other.CompareTag("MineArea"))
            {
                _manager.MakeMeAMiner();
            }
        }
    }
}