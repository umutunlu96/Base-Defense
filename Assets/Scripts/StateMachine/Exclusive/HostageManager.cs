using Signals;
using StateMachine.Miner;
using UnityEngine;

namespace StateMachine.Exclusive
{
    public class HostageManager : MonoBehaviour
    {
        [SerializeField] private HostagePhysicController physicController;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform pickAxeTransform;
        [SerializeField] private Transform diamondTransform;
        
        public void MakeMeAMiner()
        {
            int mineBaseEmptySlotCount = BaseSignals.Instance.onGetMineBaseEmptySlotCount();
            if(mineBaseEmptySlotCount <= 0) return;
            StackSignals.Instance.onRemoveStack?.Invoke(transform);
            
            transform.SetParent(AiSignals.Instance.onGetMineBaseArea());
            
            MinerAI minerAI = gameObject.AddComponent(typeof(MinerAI)) as MinerAI;
            
            minerAI.DiamondTransform = diamondTransform;
            minerAI.PickAxeTransform = pickAxeTransform;
            
            MinerPhysicController minerPhysicController = physicController.transform.gameObject.AddComponent(typeof(MinerPhysicController)) as MinerPhysicController;
            minerPhysicController.manager = minerAI;

            animator.runtimeAnimatorController = (RuntimeAnimatorController) Resources.Load("Animators/MineWorker",
                typeof(RuntimeAnimatorController ));

            gameObject.name = "Miner";
            
            Destroy(physicController);
            Destroy(this);
        }

        public void MakeMeASoldier()
        {

        }
    }
}