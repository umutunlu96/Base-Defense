using DG.Tweening;
using Signals;
using UnityEngine;

namespace Managers
{
    public class GateManager : MonoBehaviour
    {
        [SerializeField] private GameObject gateStick;
        [SerializeField] private float duration;
        
        public void OpenTheGate()
        {
            gateStick.transform.DOLocalRotate(new Vector3(0, 0, -90), duration);
        }

        public void CloseTheGate()
        {
            gateStick.transform.DOLocalRotate(new Vector3(0, 0, 0), duration);
        }
    }
}