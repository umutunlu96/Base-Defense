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
                manager.PlayerTarget = other.transform.parent;
                manager.SetTarget(manager.PlayerTarget);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                manager.SetTarget(manager.BaseTarget);
            }
        }
    }
}