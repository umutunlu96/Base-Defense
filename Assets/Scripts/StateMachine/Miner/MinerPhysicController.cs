using UnityEngine;

namespace StateMachine.Miner
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class MinerPhysicController : MonoBehaviour
    {
        public MinerAI manager;
        
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("GemSpot"))
            {
                manager.ReachedGemArea = true;
            }
            
            if (other.CompareTag("StockpileSpot"))
            {
                manager.ReachedStockpileArea = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("GemSpot"))
            {
                manager.ReachedGemArea = false;
            }
            
            if (other.CompareTag("StockpileSpot"))
            {
                manager.ReachedStockpileArea = false;
            }
        }
    }
}