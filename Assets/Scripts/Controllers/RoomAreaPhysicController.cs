using DG.Tweening;
using Enums;
using Managers;
using Signals;
using TMPro;
using UnityEngine;

namespace Controllers
{
    public class RoomAreaPhysicController : MonoBehaviour
    {
        [SerializeField] private float delay = 0.005f;
        [SerializeField] private RoomManager manager;
        [SerializeField] private TextMeshPro payedAmountText;
        [SerializeField] private Renderer filledSquareRenderer;
        private float _timer;
        
        public void UpdatePayedAmountText(int payedAmount, int cost) => payedAmountText.text = (cost - payedAmount).ToString();
        
        public void ResetRadialFilletAmount()
        {
            filledSquareRenderer.material.SetFloat("_Arc2", 360);
        }
        public void SetRadialFilletAmount(int payedAmount, int cost)
        {
            float filletAmount = 360 - (payedAmount* 360 / cost);
            filledSquareRenderer.material.DOFloat(filletAmount,"_Arc2",delay);
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                manager.OnPlayerEnter();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                manager.OnPlayerExit();
                ResetRadialFilletAmount();
            }
        }
    }
}