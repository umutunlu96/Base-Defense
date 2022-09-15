using System;
using UnityEngine;

namespace StateMachine.Exclusive
{
    public class HostagePhysicController : MonoBehaviour
    {
        [SerializeField] private HostageManager _manager;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("MineArea"))
            {
                _manager.MakeMeAMiner();
            }
        }
    }
}