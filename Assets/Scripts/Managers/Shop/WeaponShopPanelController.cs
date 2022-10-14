using System.Collections.Generic;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Signals;
using TMPro;
using UnityEngine;

namespace Managers.Shop
{
    public class WeaponShopPanelController : MonoBehaviour
    {
        private CD_Bullet _cdBullet;
        private BulletData _pistolData;
        private BulletData _shotgunData;
        private BulletData _rifleData;
        private BulletData _minigunData;
        private List<BulletData> _bulletDatas = new List<BulletData>();

        [Header("Pistol")]
        [SerializeField] private GameObject PistolBuy;
        [SerializeField] private GameObject PistolSelect;
        [SerializeField] private GameObject PistolUpgrade;
        [SerializeField] private TextMeshProUGUI PistolLevel;
        // [SerializeField] private TextMeshProUGUI PistolUpgradeText;
        [Header("Shotgun")]
        [SerializeField] private GameObject ShotgunBuy;
        [SerializeField] private GameObject ShotgunSelect;
        [SerializeField] private GameObject ShotgunUpgrade;
        [SerializeField] private TextMeshProUGUI ShotgunLevel;
        // [SerializeField] private TextMeshProUGUI ShotgunUpgradeText;
        [Header("Rifle")]
        [SerializeField] private GameObject RifleBuy;
        [SerializeField] private GameObject RifleSelect;
        [SerializeField] private GameObject RifleUpgrade;
        [SerializeField] private TextMeshProUGUI RifleLevel;
        // [SerializeField] private TextMeshProUGUI RifleUpgradeText;
        [Header("MiniGun")]
        [SerializeField] private GameObject MinigunBuy;
        [SerializeField] private GameObject MinigunSelect;
        [SerializeField] private GameObject MinigunUpgrade;
        [SerializeField] private TextMeshProUGUI MinigunLevel;
        // [SerializeField] private TextMeshProUGUI MinigunUpgradeText;

        private void Awake()
        {
            Initialize();
            SetDatas();
        }

        private void Initialize()
        {
            _cdBullet = Resources.Load<CD_Bullet>("Data/CD_Bullet");

            _pistolData = _cdBullet.BulletDatas[WeaponType.Pistol];
            _shotgunData = _cdBullet.BulletDatas[WeaponType.Shotgun];
            _rifleData = _cdBullet.BulletDatas[WeaponType.Rifle];
            _minigunData = _cdBullet.BulletDatas[WeaponType.MiniGun];
            _bulletDatas.Add(_pistolData);
            _bulletDatas.Add(_shotgunData);
            _bulletDatas.Add(_rifleData);
            _bulletDatas.Add(_minigunData);
        }

        private void SetDatas()
        {
            PistolLevel.text = $"Level {_pistolData.Level}";
            CheckBuyState(_bulletDatas[0], PistolBuy, PistolSelect, PistolUpgrade);
            ShotgunLevel.text = $"Level {_shotgunData.Level}";
            CheckBuyState(_bulletDatas[1], ShotgunBuy, ShotgunSelect, ShotgunUpgrade);
            RifleLevel.text = $"Level {_rifleData.Level}";
            CheckBuyState(_bulletDatas[2], RifleBuy, RifleSelect, RifleUpgrade);
            MinigunLevel.text = $"Level {_minigunData.Level}";
            CheckBuyState(_bulletDatas[3], MinigunBuy, MinigunSelect, MinigunUpgrade);
        }

        private void CheckBuyState(BulletData bulletData, GameObject buy, GameObject select, GameObject upgrade)
        {
            if (bulletData.Bought)
            {
                buy.SetActive(false);
                select.SetActive(true);
                upgrade.SetActive(true);
            }
            else
            {
                buy.SetActive(true);
                select.SetActive(false);
                upgrade.SetActive(false);
            }
        }

        public void OnBuy(int order)
        {
            if (ScoreSignals.Instance.onGetMoneyAmount() >= 500)
            {
                _bulletDatas[order].Bought = true;
                ScoreSignals.Instance.onSetMoneyAmount?.Invoke(-500);
                SetDatas();
            }
        }

        public void OnUpgrade(int order)
        {
            if (ScoreSignals.Instance.onGetMoneyAmount() >= 500)
            {
                _bulletDatas[order].Level++;
                _bulletDatas[order].Damage += .25f;
                ScoreSignals.Instance.onSetMoneyAmount?.Invoke(-500);
                SetDatas();
            }
        }
        
        public void OnSelect(int order)
        {
            switch (order)
            {
                case 0 :
                    PlayerSignals.Instance.onPlayerWeaponTypeChanged?.Invoke(WeaponType.Pistol);
                    break;
                case 1 :
                    PlayerSignals.Instance.onPlayerWeaponTypeChanged?.Invoke(WeaponType.Shotgun);
                    break;
                case 2 :
                    PlayerSignals.Instance.onPlayerWeaponTypeChanged?.Invoke(WeaponType.Rifle);
                    break;
                case 3 :
                    PlayerSignals.Instance.onPlayerWeaponTypeChanged?.Invoke(WeaponType.MiniGun);
                    break;
            }
        }
    }
}