using Data.UnityObject;
using Data.ValueObject.Base;
using Signals;
using StateMachine;
using TMPro;
using UnityEngine;

namespace Controllers
{
    public class GroundMine : MonoBehaviour
    {
        [SerializeField] private SphereCollider sCollider;
        [SerializeField] private GameObject BuySquare;
        [SerializeField] private ParticleSystem explosionParticle;
        [SerializeField] private TextMeshPro payedAmoundText;
        private float _timer;
        private FrontYardItemsData _data;
        private float _delay = 0.005f;
        private int _payedAmound;
        private float _bombTick;
        private bool _isActivated;
        private bool _isInCoolDown;
        private int _bombWaitTime;
        
        private void Start()
        {
            GetData();
            _bombWaitTime = _data.BombWaitTime;
        }

        private void GetData() => _data = Resources.Load<CD_Level>("Data/CD_Level").Levels[LevelSignals.Instance.onGetLevelID()]
            .frontYardData.FrondYardItemsDatas[0];

        private void Update()
        {
            if (_isActivated)
            {
                _bombTick += Time.deltaTime;
                sCollider.enabled = true;
                if(_bombTick < _data.BombExplodeTimer) return;
                explosionParticle.Play();
                AiSignals.Instance.onGroundMineExplode?.Invoke();
                _isActivated = false;
                _bombTick = 0;
                _isInCoolDown = true;
            }

            if (_isInCoolDown)
            {
                sCollider.enabled = false;
                _bombTick += Time.deltaTime;
                if (_bombTick >= _bombWaitTime)
                {
                    BuySquare.SetActive(true);
                    UpdateText(_data.BombCost);
                }
            }
        }

        public void OnPlayerEnter()
        {
            if (PlayerSignals.Instance.onIsPlayerMoving()) return;
            if (ScoreSignals.Instance.onGetDiamondAmount() < _data.BombCost) return;
                
            _timer -= Time.deltaTime;

            if (!(_timer <= 0)) return;
            if (_payedAmound < _data.BombCost)
            {
                UpdateText(_data.BombCost - _payedAmound);
                ScoreSignals.Instance.onSetDiamondAmount(-1);
                _payedAmound++;
            }
            else
            {
                if(_isActivated) return;
                BuySquare.SetActive(false);
                _isActivated = true;
                AiSignals.Instance.onGroundMinePlanted?.Invoke(transform);
            }
            _timer = _delay;
        }

        public void OnPlayerLeave()
        {
            UpdateText(_data.BombCost);
        }

        private void UpdateText(int amount)
        {
            payedAmoundText.text = $"{amount}";
        }
    }
}