﻿using System;
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

        public Action onPlayerUseTurret;
        public Action onPlayerLeaveTurret;
        
        public Action onPlayerEnterMineArea;
        public Action onPlayerEnterSoldierArea;
        public Action<Transform> onPlayerEnterDiamondArea;

        public Action<WeaponType> onPlayerWeaponTypeChanged;
    }
}