using System;
using Keys;
using Signals;
using StateMachine.Miner;
using UnityEngine;

namespace StateMachine.Hostage
{
    public class HostageManager : MonoBehaviour
    {
        [SerializeField] private HostagePhysicController physicController;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform pickAxeTransform;
        [SerializeField] private Transform diamondTransform;
        private static readonly int Speed = Animator.StringToHash("Speed");
        

        private bool isRescued;
        public bool IsRescued { get { return isRescued;} set { isRescued = value; } }
        
        #region EventSubscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            InputSignals.Instance.onInputDragged += OnInputDragged;
            InputSignals.Instance.onInputReleased += OnInputReleased;
        }
        
        private void UnSubscribeEvents()
        {
            InputSignals.Instance.onInputDragged -= OnInputDragged;
            InputSignals.Instance.onInputReleased -= OnInputReleased;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion

        #region Event Functions

        private void OnInputDragged(InputParams inputParams)
        {
            if(!isRescued) return;
            animator.SetFloat(Speed,inputParams.movementVector.magnitude);
        }

        private void OnInputReleased()
        {
            if (!isRescued) return;
            animator.SetFloat(Speed,0);
        }
        
        #endregion
        
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