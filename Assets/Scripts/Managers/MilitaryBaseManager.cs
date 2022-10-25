using System.Collections.Generic;
using System.Threading.Tasks;
using Data.UnityObject;
using Data.ValueObject.Base;
using Signals;
using StateMachine;
using UnityEngine;

namespace Managers
{
    public class MilitaryBaseManager : MonoBehaviour
    {
        [SerializeField] private Transform militaryBaseTentEnterenceTransform;
        [SerializeField] private Transform militaryBaseTentTransform;
        [SerializeField] private Transform soldierSpawnTransform;
        [SerializeField] private SpriteRenderer sRenderer;
        [SerializeField] private List<Transform> soldierWaitPoints = new List<Transform>();
        
        private  MilitaryBaseData _data;
        private List<Transform> _candidates = new List<Transform>();

        private float _radialMultiplier;
        private float _timer;
        private float _playerAttackTimer;
        private int _maxSoldierAmount;
        private int _currentSoldierAmount;
        private int _maxCandidateAmount;
        private int _currentCandidateAmount;
        private float _soldierUpgradeTimer;
        private int _soldierSlotCost;
        private int _attackTimer;

        private bool _canInterractWithPlayer = true;
        
        private MilitaryBaseData GetData() => Resources.Load<CD_Level>("Data/CD_Level").Levels[GetLevelCount() - 1]
            .BaseData.MilitaryBaseData;

        private void Start()
        {
            _data = GetData();
            InitDatas();
            InitSoldiers();
            _radialMultiplier = 360 / _data.SoldierUpgradeTimer;
        }

        private void InitDatas()
        {
            _maxSoldierAmount = _data.MaxSoldierAmount;
            _maxCandidateAmount = _data.MaxCandidateAmount;
            _soldierUpgradeTimer = _data.SoldierUpgradeTimer;
            _soldierSlotCost = _data.SoldierSlotCost;
            _attackTimer = _data.AttackTimer;
        }

        private async void InitSoldiers()
        {
            await Task.Delay(200);
            
            for (int i = 0; i < _data.CurrentSoldierAmount; i++)
            {
                GenerateSoldier();
                await Task.Delay(200);
            }
        }
        
        #region EventSubscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            AiSignals.Instance.onGetMilitaryBaseTentEnterenceTransform += ReturnTentEnterenceTransform;
            AiSignals.Instance.onGetMilitaryBaseTentTransform += ReturnMilitaryBaseTentTransform;
            AiSignals.Instance.onGetSoldierWaitTransform += ReturnSoldierWaitPoint;
            AiSignals.Instance.onGetCurrentEmptySlotForCandidate += CurrentAvaibleHostageWaitSlot;
            AiSignals.Instance.onCandidateEnteredMilitaryArea += OnGetCandidates;
        }
        
        private void UnSubscribeEvents()
        {
            AiSignals.Instance.onGetMilitaryBaseTentEnterenceTransform -= ReturnTentEnterenceTransform;
            AiSignals.Instance.onGetMilitaryBaseTentTransform -= ReturnMilitaryBaseTentTransform;
            AiSignals.Instance.onGetSoldierWaitTransform -= ReturnSoldierWaitPoint;
            AiSignals.Instance.onGetCurrentEmptySlotForCandidate -= CurrentAvaibleHostageWaitSlot;
            AiSignals.Instance.onCandidateEnteredMilitaryArea -= OnGetCandidates;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion

        #region Event-Functions

        private Transform ReturnMilitaryBaseTentTransform() => militaryBaseTentTransform;
        
        private Transform ReturnTentEnterenceTransform() => militaryBaseTentEnterenceTransform;
        
        private int GetLevelCount() => LevelSignals.Instance.onGetLevelCount();
        
        private void OnGetCandidates(Transform candidate) => _candidates.Add(candidate);

        private Transform ReturnSoldierWaitPoint() => soldierWaitPoints[_currentSoldierAmount - 1];

        #endregion

        private void Update()
        {
            if (_candidates.Count == 0) return;
            _timer += Time.deltaTime;
            UpdateRadialFilletAmount(_timer);
            if (_timer >= _soldierUpgradeTimer && _currentSoldierAmount <= _maxSoldierAmount)
            {
                GenerateSoldier();
                ResetRadialFilletAmount();
                _timer = 0;
            }
        }

        private void GenerateSoldier()
        {
            GameObject soldier = PoolSignals.Instance.onGetPoolObject?.Invoke("Soldier", soldierSpawnTransform);
            soldier.transform.SetParent(transform);
            _currentSoldierAmount++;
            if(_candidates.Count == 0) return;
            _currentCandidateAmount--;
            _candidates.Remove(_candidates[0]);
            _candidates.TrimExcess();
        }

        private void UpdateRadialFilletAmount(float currentTime) => sRenderer.material.SetFloat("_Arc1", 360 - currentTime * _radialMultiplier);

        private void ResetRadialFilletAmount() => sRenderer.material.SetFloat("_Arc1", 360);
        
        private int CurrentAvaibleHostageWaitSlot() => _maxCandidateAmount - _currentCandidateAmount;

        public void OnPlayerEnter()
        {
            _playerAttackTimer += Time.deltaTime;
            if (_playerAttackTimer >= _attackTimer && _canInterractWithPlayer)
            {
                OnPlayerPressedAttack();
                _canInterractWithPlayer = false;
            }
        }

        public void OnPlayerExit()
        {
            _canInterractWithPlayer = true;
            _playerAttackTimer = 0;
        }
        
        public void OnPlayerPressedAttack()
        {
            AiSignals.Instance.onAttackAllSoldiers?.Invoke();
            _currentSoldierAmount = 0;
        }
    }
}