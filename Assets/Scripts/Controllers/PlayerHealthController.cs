using System;
using System.Collections;
using Managers;
using Signals;
using UnityEngine;

namespace Controllers
{
    public class PlayerHealthController : MonoBehaviour
    {
        [SerializeField] private PlayerManager manager;
        [SerializeField] private HealthBar healthBar;
        private Coroutine healCoroutine;

        private void Update()
        {
            transform.rotation = Quaternion.Euler(30, -manager.transform.rotation.y, -90);
        }

        public void Heal()
        {
            if (healCoroutine == null)
                healCoroutine = StartCoroutine(HealUp());
        }

        private IEnumerator HealUp()
        {
            WaitForSeconds Wait = new WaitForSeconds(.1f);
            
            yield return Wait;
            
            while (manager.Health < 100)
            {
                manager.Health++;
                SetHealthBar(manager.Health);
                yield return Wait;
            }
            healCoroutine = null;
        }
        
        public void SetHealthBar(float curretHealth)
        {
            float ratio = curretHealth / 100;
            healthBar.HealthNormalized = ratio;
        }

        public void ResetHealthBar() => healthBar.HealthNormalized = 1;
    }
}