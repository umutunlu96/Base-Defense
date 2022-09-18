using System;
using UnityEngine;

namespace StateMachine.MoneyWorkerAI
{
    [RequireComponent(typeof(SphereCollider))]
    public class MoneyCollector : MonoBehaviour
    {
        [SerializeField] private MoneyWorkerAI manager;

        private void Awake()
        {
            GetComponent<SphereCollider>().radius = manager.CollectRange;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Money"))
            {
                other.tag = "Collected";
                manager.TakeMoney(other.transform);
            }
        }
    }
}