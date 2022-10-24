using UnityEngine;

namespace Controllers
{
    public class GroundMinePhysicController : MonoBehaviour
    {
        [SerializeField] private GroundMine manager;
        
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                manager.OnPlayerEnter();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                manager.OnPlayerLeave();
            }
        }
    }
}