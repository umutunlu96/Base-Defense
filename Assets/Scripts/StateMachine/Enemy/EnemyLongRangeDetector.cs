using System;
using UnityEngine;

namespace StateMachine.Enemy
{
    [RequireComponent(typeof(SphereCollider))]
    public class EnemyLongRangeDetector : MonoBehaviour
    {
        [SerializeField] private EnemyAI manager;

        private void Awake()
        {
            GetComponent<SphereCollider>().radius = manager.ChaseRange;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                manager.CurrentTarget = other.transform;
                manager.CanChase = true;
            }
            
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                manager.CanChase = false;
            }
        }
    }
}