using System;
using Abstract;
using UnityEngine;

namespace ObjectPool
{
    [Serializable]
    public class EnemyPoolData
    {
        public GameObject PoolObject;
        public int Amount;
        public Attribute Attribute;
    }
}