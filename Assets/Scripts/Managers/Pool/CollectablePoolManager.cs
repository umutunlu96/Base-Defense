using System.Collections.Generic;
using Extentions;
using UnityEngine;

namespace Managers.Pool
{
    public class CollectablePoolManager : MonoBehaviour
    {
        public List<GameObject> MoneyList;
        public List<GameObject> GemList;
        
        [SerializeField] private GameObject moneyPrefab;
        [SerializeField] private GameObject gemPrefab;
        
        public static ObjectPool<GameObject> MoneyPool;
        public static ObjectPool<GameObject> GemPool;


        public int MoneyAmount = 5;
        public int GemAmount = 5;

        private void Awake()
        {
            InitPool();
        }

        private void InitPool()
        {
            MoneyPool = new ObjectPool<GameObject>(MoneyFactory,MoneyTurnOnCallback,MoneyTurnOffCallback,MoneyAmount,true);
            GemPool = new ObjectPool<GameObject>(GemFactory, GemTurnOnCallback, GemTurnOffCallback, GemAmount, true);
        }

        private GameObject MoneyFactory()
        {
            GameObject moneyObject = Instantiate(moneyPrefab, transform);
            return moneyObject;
        }

        private void MoneyTurnOnCallback(GameObject money)
        {
            MoneyList.Remove(money);
            MoneyList.TrimExcess();
            money.SetActive(true);
        }
        
        private void MoneyTurnOffCallback(GameObject money)
        {
            MoneyList.Add(money);
            MoneyList.TrimExcess();
            money.SetActive(false);
        }
        
        private GameObject GemFactory()
        {
            GameObject gemObject = Instantiate(gemPrefab, transform);
            return gemObject;
        }
        
        private void GemTurnOnCallback(GameObject gem)
        {
            GemList.Remove(gem);
            GemList.TrimExcess();
            gem.SetActive(true);
        }
        
        private void GemTurnOffCallback(GameObject gem)
        {
            GemList.Add(gem);
            GemList.TrimExcess();
            gem.SetActive(false);
        }
    }
}