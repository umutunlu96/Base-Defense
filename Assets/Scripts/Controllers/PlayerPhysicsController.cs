using Managers;
using UnityEngine;


namespace Controllers
{
    public class PlayerPhysicsController : MonoBehaviour
    {
        [SerializeField] PlayerManager manager;

        private void OnTriggerEnter(Collider other)
        {
            //
        }
        private void OnTriggerExit(Collider other)
        {
            //
        }
    }
}