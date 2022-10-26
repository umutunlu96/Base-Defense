using DG.Tweening;
using Keys;
using Signals;
using StateMachine.Miner;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.Hostage
{
    public class HostageManager : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private HostagePhysicController physicController;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform pickAxeTransform;
        [SerializeField] private Transform diamondTransform;
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int Scared = Animator.StringToHash("Scared");
        

        private bool isRescued;
        private bool isCandidate;
        
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
            if(!isRescued || isCandidate) return;
            animator.SetFloat(Speed,inputParams.movementVector.magnitude);
        }

        private void OnInputReleased()
        {
            if (!isRescued || isCandidate) return;
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
            
            int workerLayer = LayerMask.NameToLayer("Worker");
            var physicControllerGO = physicController.gameObject;
            physicControllerGO.tag = "Worker";
            physicControllerGO.layer = workerLayer;
            animator.runtimeAnimatorController = (RuntimeAnimatorController) Resources.Load("Animators/MineWorker",
                typeof(RuntimeAnimatorController ));

            gameObject.name = "Miner";
            
            Destroy(physicController);
            Destroy(this);
        }

        public void MakeMeACandidateSoldier()
        {
            int soldierBaseEmptySlotCount = AiSignals.Instance.onGetCurrentEmptySlotForCandidate();
            if(soldierBaseEmptySlotCount <= 0) return;
            isCandidate = true;
            StackSignals.Instance.onRemoveStack?.Invoke(transform);
            
            animator.SetTrigger(Run);
            
            //Tents
            Vector3 tentEnterencePosition = AiSignals.Instance.onGetMilitaryBaseTentEnterenceTransform().position;
            Vector3 tentPosition = AiSignals.Instance.onGetMilitaryBaseTentTransform().position;
            //Rotations
            Vector3 lookEnterenceRotation = tentEnterencePosition - transform.position;
            Vector3 lookTentRotation = tentPosition - transform.position;
            
            transform.rotation = Quaternion.LookRotation(lookEnterenceRotation);
            
            transform.DOMove(tentEnterencePosition, 3).OnComplete(() =>
            {
                transform.rotation = Quaternion.LookRotation(lookTentRotation);
                transform.DOMove(tentPosition, 2f);
            });
        }

        public void ReturnMeToPool()
        {
            AiSignals.Instance.onCandidateEnteredMilitaryArea?.Invoke(transform);
            animator.SetFloat(Speed, 0);
            ResetHostage();
            PoolSignals.Instance.onReleasePoolObject?.Invoke("Hostage", gameObject);
        }
        
        private void ResetHostage()
        {
            isRescued = false;
            isCandidate = false;
            animator.SetTrigger(Scared);
        }
    }
}