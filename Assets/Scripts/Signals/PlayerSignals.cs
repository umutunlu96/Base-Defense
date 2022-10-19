using System;
using Enums;
using Extentions;
using UnityEngine;

namespace Signals
{
    public class PlayerSignals : MonoSingleton<PlayerSignals>
    {
        public Func<Transform> onGetPlayerTransfrom;
        public Func<float> onGetPlayerSpeed;
        public Func<bool> onIsPlayerMoving;
        
        public Action onPlayerEnterMineArea;
        public Action onPlayerEnterSoldierArea;

        public Action onPlayerEnterTurretArea;
        public Action onPlayerLeaveTurretArea;
        
        public Action<Transform> onPlayerEnterDiamondArea;

        public Action<WeaponType> onPlayerWeaponTypeChanged;

        public Action<int> onTakeDamage;
    }
}