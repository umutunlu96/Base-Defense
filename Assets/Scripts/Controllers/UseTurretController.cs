using StateMachine.TurretSoldier;
using UnityEngine;

namespace Controllers
{
    public class UseTurretController : MonoBehaviour
    {
        [SerializeField] private TurretSoldierAI manager;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                manager.IsPlayerUsingTurret = true;
        }
    }
}