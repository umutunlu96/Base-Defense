using System.Collections.Generic;
using Controllers;
using Data.UnityObject;
using Data.ValueObject.Base;
using Signals;
using StateMachine;
using UnityEngine;

namespace Managers
{
    public class MilitaryBaseManager : MonoBehaviour
    {
        [SerializeField] private MilitaryBaseAttackController attackController; 
        [SerializeField] private Transform militaryBaseTentEnterenceTransform;
        [SerializeField] private Transform militaryBaseTentTransform;
        [SerializeField] private Transform soldierSpawnTransform;
        [SerializeField] private List<Transform> soldierWaitPoints = new List<Transform>();
        [SerializeField] private SpriteRenderer renderer;
        
        private  MilitaryBaseData _data;
        private List<Transform> candidates = new List<Transform>();

        private float radialMultiplier;
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
            radialMultiplier = 360 / _data.SoldierUpgradeTimer;
        }

        private void InitDatas()
        {
            _maxSoldierAmount = _data.MaxSoldierAmount;
            _maxCandidateAmount = _data.MaxCandidateAmount;
            _soldierUpgradeTimer = _data.SoldierUpgradeTimer;
            _soldierSlotCost = _data.SoldierSlotCost;
            _attackTimer = _data.AttackTimer;
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
        
        private int GetLevelCount() => LevelSignals.Instance.onGetLevelID();
        
        private void OnGetCandidates(Transform candidate) => candidates.Add(candidate);

        private Transform ReturnSoldierWaitPoint() => soldierWaitPoints[_currentSoldierAmount];

        #endregion

        private void Update()
        {
            if (candidates.Count == 0) return;
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
            print("Soldier made");
            PoolSignals.Instance.onGetPoolObject?.Invoke("Soldier", soldierSpawnTransform);
            _currentCandidateAmount--;
            _currentSoldierAmount++;
            candidates.Remove(candidates[0]);
            candidates.TrimExcess();
        }

        private void UpdateRadialFilletAmount(float currentTime) => renderer.material.SetFloat("_Arc1", 360 - currentTime * radialMultiplier);

        private void ResetRadialFilletAmount() => renderer.material.SetFloat("_Arc1", 360);
        
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