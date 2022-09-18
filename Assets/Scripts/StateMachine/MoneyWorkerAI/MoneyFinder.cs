using System;
using UnityEngine;

namespace StateMachine.MoneyWorkerAI
{
    [RequireComponent(typeof(SphereCollider))]
    public class MoneyFinder : MonoBehaviour
    {
        [SerializeField] private MoneyWorkerAI manager;
        private SphereCollider _collider;
        
        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
            _collider.radius = manager.SearchRange;
        }

        public void IncreaseRaius()
        {
            _collider.radius += manager.SearchRange;
        }

        public void ResetRadius()
        {
            _collider.radius = manager.SearchRange;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Money") && other.gameObject.activeSelf)
                manager.MoneyTransform = other.transform;
        }
    }
}