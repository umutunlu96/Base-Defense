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
                StackSignals.Instance.onRemoveAllStack?.Invoke(HostageType.Miner);
            }

            if (other.CompareTag("GatherSpot"))
            {
                PlayerSignals.Instance.onPlayerCollectAllDiamonds?.Invoke(_manager.transform);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            //
        }
    }
}