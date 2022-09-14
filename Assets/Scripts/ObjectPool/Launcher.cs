using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    public class Launcher : MonoBehaviour
    {
        public static Launcher instance;
        public List<GameObject> pooledObjects;
        public GameObject objectToPool;
        public int amountToPool;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            pooledObjects = new List<GameObject>();
            GameObject temp;
            for (int i = 0; i < amountToPool; i++)
            {
                temp = Instantiate(objectToPool);
                temp.SetActive(false);
                pooledObjects.Add(temp);
            }
        }

        public GameObject GetPooledObject()
        {
            for (int i = 0; i < amountToPool; i++)
            {
                if (!pooledObjects[i].activeInHierarchy)
                {
                    return pooledObjects[i];
                }
            }

            return null;
        }
        
        
        
        
        
    }
}