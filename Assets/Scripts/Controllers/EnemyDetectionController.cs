using Managers;
using UnityEngine;

namespace Controllers
{
    public class EnemyDetectionController : MonoBehaviour
    {
        [SerializeField] private PlayerManager manager;
        [SerializeField] private PlayerAimController aimController;
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;
            // aimController.UpdateEnemyList(other.transform.parent, true);
            // print("Enemy Detected");
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;
            // aimController.UpdateEnemyList(other.transform.parent, false);
            // print("Enemy Removed from sight");
        }
    }
}