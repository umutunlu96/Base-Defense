using System;
using Managers;
using UnityEngine;

namespace Controllers
{
    public class GatePhysicController : MonoBehaviour
    {
        [SerializeField] private GateManager manager;
        
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player") || other.CompareTag("Worker") || other.CompareTag("Soldier"))
                manager.OpenTheGate();
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Player") || other.CompareTag("Worker") || other.CompareTag("Soldier"))
                manager.CloseTheGate();
        }
    }
}