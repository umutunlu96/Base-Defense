using System;
using System.Collections;
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
                // Debug.Log("PlayerEnteredAtCloseRange");
            }
            if (other.CompareTag("GateOutside"))
            {
                // Debug.Log("EnemyEnteredAtGate");
                StartCoroutine(ChangeReachedBaseState(true));
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // Debug.Log("PlayerExitedAtCloseRange");
                manager.CanAttack = false;
            }
            
            if (other.CompareTag("GateOutside"))
            {
                // Debug.Log("EnemyExitedAtGate");
                StartCoroutine(ChangeReachedBaseState(false));
            }
        }

        private IEnumerator ChangeReachedBaseState(bool value)
        {
            yield return new WaitForSeconds(.5f);
            manager.ReachedAtBase = value;
        }
    }
}