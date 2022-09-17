using UnityEngine;

namespace StateMachine.Enemy
{
    public class PlayerDetector : MonoBehaviour
    {
        [SerializeField] private EnemyAI manager;
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