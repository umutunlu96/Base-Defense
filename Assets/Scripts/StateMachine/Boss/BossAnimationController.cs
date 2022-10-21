using UnityEngine;

namespace StateMachine.Boss
{
    public class BossAnimationController : MonoBehaviour
    {
        [SerializeField] private Transform grenadeTransform;
        
        public Grenade grenade;

        public void Throw() => grenade.Launch();
        public Transform ReturnGrenadeInitialPos() => grenadeTransform;
    }
}