using System;
using Abstract;
using Enums;
using UnityEngine;

namespace Data.ValueObject.Base
{   
    [Serializable]
    public class TurretData : ISaveableEntity
    {
        public BuyState BuyState;

        public int Cost;

        public int PayedAmount;
        
        public bool HasTurretSoldier;

        public int AmmoCapacity;

        public int AmmoDamage;

        public ParticleSystem TurretParticle;
        
        public string Key = "TurretData";
        
        public string GetKey() => Key;
    }
}