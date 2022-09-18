using System;
using UnityEngine;

namespace StateMachine.MoneyWorkerAI
{
    [RequireComponent(typeof(SphereCollider))]
    public class MoneyFinder : MonoBehaviour
    {
        [SerializeField] private MoneyWorkerAI manager;
        private SphereCollider _collider;

        public float Radius { get { return _collider.radius;} private set { _collider.radius = value; } }
        
        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
            Radius = manager.SearchRange;
        }

        public void IncreaseRaius()
        {
            Radius += manager.SearchRange;
        }

        public void ResetRadius()
        {
            Radius = manager.SearchRange;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Money") && other.gameObject.activeSelf)
            {
                manager.CantFindMoney = false;
                manager.MoneyTransform = other.transform;
            }
        }
    }
}