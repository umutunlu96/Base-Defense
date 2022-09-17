using Signals;
using StateMachine.Miner;
using UnityEditor.Animations;
using UnityEngine;

namespace StateMachine.Exclusive
{
    public class HostageManager : MonoBehaviour
    {
        [SerializeField] private HostagePhysicController physicController;
        [SerializeField] private Animator animator;
        
        public void MakeMeAMiner()
        {
            // animator.runtimeAnimatorController
            StackSignals.Instance.onRemoveStack?.Invoke(transform);
            gameObject.AddComponent<MinerAI>();
            Destroy(physicController);
            Destroy(this);
        }
    }
}