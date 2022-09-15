using System;
using System.Collections.Generic;
using Extentions;
using UnityEngine;

namespace Managers.Pool
{
    public class CollectableManager : MonoBehaviour
    {
        public List<GameObject> moneypool = new List<GameObject>();
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject money = CollectablePoolManager.MoneyPool.GetObject();
                moneypool.Add(money);
                moneypool.TrimExcess();
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                if (moneypool.Count > 0)
                {
                    CollectablePoolManager.MoneyPool.ReturnObject(moneypool[0]);
                    moneypool.Remove(moneypool[0]);
                    moneypool.TrimExcess();
                }
            }
        }
    }
}