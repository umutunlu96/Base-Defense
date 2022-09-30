using Managers;
using UnityEngine;

namespace Controllers
{
    public class EnemyDetectionController : MonoBehaviour
    {
        [SerializeField] private PlayerManager manager;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                manager.UpdateEnemyList(other.transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                manager.UpdateEnemyList(other.transform);
            }
        }
    }
}