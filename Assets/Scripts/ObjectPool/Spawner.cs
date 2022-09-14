using System.Collections.Generic;
using Extentions;
using UnityEngine;

namespace ObjectPool
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject bullet;
        public List<GameObject> Bullets = new List<GameObject>();

        public ObjectPool<GameObject> BulletPool;

        private void Awake()
        {
            BulletPool = new ObjectPool<GameObject>(BulletFactoryMethod, TurnOn, TurnOff, 50, true);
        }

        public GameObject BulletFactoryMethod()
        {
            return Instantiate(bullet);
        }

        public void TurnOn(GameObject obj)
        {
            obj.transform.SetParent(transform);
            Bullets.Add(obj);
            obj.SetActive(true);
        }

        public void TurnOff(GameObject obj)
        {
            Bullets.Add(obj);
            obj.SetActive(false);
        }
    }
}