using System;
using Abstract;
using Data.UnityObject;
using Enums;
using Signals;
using UnityEngine;

namespace Managers.Shop
{
    public class WeaponShop : ShopableMarket
    {
        public ShopPanelType ShopPanelType { get; set; }
        private CD_Bullet _data;

        protected override void GetData()
        {
            _data = Resources.Load<CD_Bullet>("Data/CD_Bullet");
        }

        protected override void SetData()
        {
            
        }
        

        public override void SubscribeEvents()
        {
            
        }
        
        public override void UnSubscribeEvents()
        {
            
        }

        public override void OnBuy(ShopItemType shopItemType)
        {
            string itemType = shopItemType.ToString();
            WeaponType weaponType = (WeaponType) Enum.Parse(typeof(WeaponType), itemType);
            PlayerSignals.Instance.onPlayerWeaponTypeChanged?.Invoke(weaponType);
        }

        public override void OnUpgrade(ShopItemType shopItemType)
        {
            string itemType = shopItemType.ToString();
            WeaponType weaponType = (WeaponType) Enum.Parse(typeof(WeaponType), itemType);
            _data.BulletDatas[weaponType].Damage++;
        }
        
        public override void Save(int levelId)
        {
            
        }
        
        public override void Load(int levelId)
        {
            
        }
    }
}