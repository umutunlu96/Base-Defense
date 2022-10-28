using System;
using System.Collections;
using System.Globalization;
using Managers;
using TMPro;
using UnityEngine;

namespace Controllers
{
    public class PlayerHealthController : MonoBehaviour
    {
        [SerializeField] private PlayerManager manager;
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private TextMeshPro healthText;
        private Coroutine _healCoroutine;

        private void Update()
        {
            transform.rotation = Quaternion.Euler(30, -manager.transform.rotation.y, -90);
        }

        public void Heal()
        {
            if (_healCoroutine == null)
                _healCoroutine = StartCoroutine(HealUp());
        }

        private IEnumerator HealUp()
        {
            WaitForSeconds Wait = new WaitForSeconds(.05f);
            
            yield return Wait;
            
            while (manager.Health < 101)
            {
                manager.Health++;
                healthText.text = manager.Health.ToString();
                SetHealthBar(manager.Health);
                yield return Wait;
                if(manager.Health == 100)
                    gameObject.SetActive(false);
            }
            _healCoroutine = null;
        }
        
        public void SetHealthBar(int curretHealth)
        {
            float ratio = (float)curretHealth / 100;
            healthText.text = curretHealth.ToString();
            healthBar.HealthNormalized = ratio;
            if (curretHealth == 100)
                gameObject.SetActive(false);
        }

        public void ResetHealthBar() => healthBar.HealthNormalized = 1;

        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}