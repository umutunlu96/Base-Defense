using System;
using Managers;
using UnityEngine;

namespace Controllers
{
    public class MilitaryBaseAttackController : MonoBehaviour
    {
        [SerializeField] private MilitaryBaseManager manager;
        [SerializeField] private Renderer filledSquareRenderer;
        private float _timer;
        private float _filletAmount;
        
        public void SetRadialFilletAmount(bool isInitialize)
        {
            if (isInitialize)
            {
                filledSquareRenderer.material.SetFloat("_Arc2", 360);
            }
            else
            {
                _timer += Time.deltaTime;
                _filletAmount = 360 - (_timer * 360 / 2);
                filledSquareRenderer.material.SetFloat("_Arc2", _filletAmount);
            }
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                SetRadialFilletAmount(false);
                manager.OnPlayerEnter();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                manager.OnPlayerExit();
                _timer = 0;
                SetRadialFilletAmount(true);
            }
        }
    }
}