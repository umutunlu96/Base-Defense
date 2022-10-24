using System;

namespace Data.ValueObject.Base
{   
    [Serializable]
    public class MilitaryBaseData
    {
        public int MaxSoldierAmount;
        
        public int MaxCandidateAmount;

        public int CurrentSoldierAmount;
        
        public float SoldierUpgradeTimer = 5f;

        public int SoldierSlotCost = 100;
        
        public int AttackTimer;
    }
}