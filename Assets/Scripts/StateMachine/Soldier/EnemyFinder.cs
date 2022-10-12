using System;
using UnityEngine;

namespace StateMachine.Soldier
{
    public class EnemyFinder : MonoBehaviour
    {
        [SerializeField] private SoldierAI manager;
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
            if (other.CompareTag("Enemy") && other.gameObject.activeSelf)
            {
                manager.enemyTarget = other.transform;
                ResetRadius();
                print("enemyfound");
            }
        }
    }
}