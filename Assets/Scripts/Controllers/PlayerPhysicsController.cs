using Enums;
using Managers;
using Signals;
using UnityEngine;

namespace Controllers
{
    public class PlayerPhysicsController : MonoBehaviour
    {
        [SerializeField] PlayerManager _manager;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Hostage"))
            {
                other.gameObject.tag = "Rescued";
                StackSignals.Instance.onAddStack?.Invoke(other.transform.parent);
            }

            if (other.CompareTag("MineArea"))
            {
                PlayerSignals.Instance.onPlayerEnterMineArea?.Invoke();
                // StackSignals.Instance.onRemoveAllStack?.Invoke(HostageType.Miner);
            }

            if (other.CompareTag("StockpileSpot"))
            {
                PlayerSignals.Instance.onPlayerEnterDiamondArea?.Invoke(_manager.transform);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            //
        }
    }
}