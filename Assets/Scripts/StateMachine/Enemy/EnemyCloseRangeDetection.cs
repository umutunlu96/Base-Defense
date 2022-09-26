using System;
using UnityEngine;

namespace StateMachine.Enemy
{
    [RequireComponent(typeof(SphereCollider))]
    public class EnemyCloseRangeDetection : MonoBehaviour
    {
        [SerializeField] private EnemyAI manager;

        private void Awake()
        {
            GetComponent<SphereCollider>().radius = manager.AttackRange;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                manager.CanAttack = true;
            }
            if (other.CompareTag("GateOutside"))
            {
                manager.ReachedAtBase = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                manager.CanAttack = false;
            }
            
            if (other.CompareTag("GateOutside"))
            {
                manager.ReachedAtBase = false;
            }
        }
    }
}