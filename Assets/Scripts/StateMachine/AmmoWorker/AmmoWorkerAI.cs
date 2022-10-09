using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Managers;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.AmmoWorker
{
    public class AmmoWorkerAI : MonoBehaviour
    {
        #region Variables

        #region Public

        public StackType StackType;
        public bool IsBougth;

        #endregion

        #region Serialized
        
        [SerializeField] private CollectableStackManager stackManager;
        [SerializeField] private float speed = 2f;
        
        #endregion

        #region Private

        private StateMachine _stateMachine;
        private StackData _stackData;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        
        [ShowInInspector] private List<TurretManager> _turretManagers = new List<TurretManager>();
        [ShowInInspector] private List<Transform> _turretAmmoHolderTransformList = new List<Transform>();
        [ShowInInspector] private List<int> _turretCurrentAvaibleAmmoAmountList = new List<int>();
        [SerializeField] private Transform _ammoWarehouseTransform;
        
        private Transform _targetTurretTransform;

        [ShowInInspector] private int _capacity;
        [ShowInInspector] private bool _isAtAmmoWarehouse;
        [ShowInInspector] private bool _isAtTurretAmmoHolder;
        [ShowInInspector] private int _collectedAmmo = 0;
        [ShowInInspector] private bool _isCurrentTurretFull = false;
        [ShowInInspector] private bool _isAllTurretsAreaFullNow = false;
        [ShowInInspector] private bool _isPlacedAmmo = false;

        #endregion

        #endregion

        public float Speed { get => speed; set => speed = value; }
        
        public Transform CurrentTarget { get => _targetTurretTransform; set => _targetTurretTransform = value; }
        
        public bool IsAtAmmoWarehouse { get => _isAtAmmoWarehouse; set => _isAtAmmoWarehouse = value; }
        
        public bool IsCurrentTurretFull { get => _isCurrentTurretFull; set => _isCurrentTurretFull = value; }
        
        public bool IsAtTurretAmmoHolder { get => _isAtTurretAmmoHolder; set => _isAtTurretAmmoHolder = value; }
        
        private StackData GetStackData() => Resources.Load<CD_StackData>("Data/CD_StackData").StackDatas[StackType];
        
        private void SetReferances()
        {
            _stackData = GetStackData();
            _capacity = _stackData.Capacity;
            stackManager.SetStackData(_stackData);
            _turretManagers = AiSignals.Instance.onGetTurretManagers();
        }
        
        private void Start()
        {
            SetReferances();
            GetTurretsAmmoHolderTransforms();
            InitAI();
        }

        private void GetTurretsAmmoHolderTransforms()
        {
            for (int i = 0; i < _turretManagers.Count; i++)
            {
                _turretAmmoHolderTransformList.Add(_turretManagers[i].AmmoHolderTransform);
            }
        }

        private void GetTurretsCurrentAmmoAmount()
        {
            _turretCurrentAvaibleAmmoAmountList.Clear();
            
            for (int i = 0; i < _turretManagers.Count; i++)
            {
                _turretCurrentAvaibleAmmoAmountList.Add(_turretManagers[i].GetCurrentEmptyAmmoCount());
            }
        }
        
        private void InitAI()
        {
            _animator = GetComponentInChildren<Animator>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _stateMachine = new StateMachine();

            var stationary = new Stationary(this, _animator, _navMeshAgent);
            var goAmmoWarehouse = new GoAmmoWarehouse(this, _animator, _navMeshAgent, _ammoWarehouseTransform);
            var pickAmmo = new PickAmmo(this, _animator);
            var goTurret = new GoTurret(this, _animator, _navMeshAgent);
            var placeAmmoToTurret = new PlaceAmmoToTurret(this);
            var searchForEmptyTurret = new SearchForEmptyTurret(this);
            
            At(stationary, goAmmoWarehouse, IsBought());
            At(goAmmoWarehouse, pickAmmo, IsAtAmmoWarehouse());
            At(pickAmmo, goTurret, CapasityFullAndHasTurretTarget());
            At(goTurret, placeAmmoToTurret, IsAtTurretAmmoHolder());
            At(placeAmmoToTurret, goAmmoWarehouse, IsDeployedAllAmmoOrAllTurretsAreFull());
            At(placeAmmoToTurret, goTurret, HaveSomeAmmoAndCurrentTurretFull());
            At(pickAmmo, searchForEmptyTurret, SearchForTurretEmptySlot());
            
            _stateMachine.SetState(stationary);
            _stateMachine.AddAnyTransition(goAmmoWarehouse, IsAllTurretsAreFull());
            
            void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);
            
            Func<bool> IsBought() => () => IsBougth;
            Func<bool> IsAtAmmoWarehouse() => () => this.IsAtAmmoWarehouse;
            Func<bool> CapasityFullAndHasTurretTarget() => () => _targetTurretTransform != null && _collectedAmmo == _capacity;
            Func<bool> IsAtTurretAmmoHolder() => () => this.IsAtTurretAmmoHolder && !_isPlacedAmmo;
            Func<bool> HaveSomeAmmoAndCurrentTurretFull() => () => _collectedAmmo != 0 && _isCurrentTurretFull && CurrentTarget != null && _isPlacedAmmo;
            Func<bool> IsDeployedAllAmmoOrAllTurretsAreFull() => () => _collectedAmmo == 0 || _isAllTurretsAreaFullNow;
            Func<bool> SearchForTurretEmptySlot() => () => _isAllTurretsAreaFullNow && this.IsAtAmmoWarehouse;
            Func<bool> IsAllTurretsAreFull() => () => _isAllTurretsAreaFullNow && !this.IsAtAmmoWarehouse;
            
        }
        
        private void Update() => _stateMachine.Tick();

        public void GetAvaibleTurretTarget()
        {
            GetTurretsCurrentAmmoAmount();

            int ammoCache = 0;
            
            foreach (var ammo in _turretCurrentAvaibleAmmoAmountList)
            {
                ammoCache += ammo;
            }

            if (ammoCache == 0)
            {
                _targetTurretTransform = null;
                _isAllTurretsAreaFullNow = true;
                return;
            }
            
            for (int i = 0; i < _turretCurrentAvaibleAmmoAmountList.Count; i++)
            {
                if (_turretCurrentAvaibleAmmoAmountList[i] > 0)
                {
                    _isAllTurretsAreaFullNow = false;
                    CurrentTarget = _turretAmmoHolderTransformList[i];
                    _isCurrentTurretFull = false;
                    break;
                }
            }
        }

        public async void TakeAmmo()
        {
            if(_collectedAmmo == _capacity) return;

            for (int i = 0; i < _capacity; i++)
            {
                GameObject ammo = PoolSignals.Instance.onGetPoolObject?.Invoke("Ammo", _ammoWarehouseTransform);
                if (_collectedAmmo == _capacity) break;
                if(ammo == null) return;
                stackManager.AddStack(ammo.transform);
                _collectedAmmo++;

                await Task.Delay(100);
            }
        }
        
        public async void DropAmmoToTurret()
        {
            int index = _turretAmmoHolderTransformList.IndexOf(_targetTurretTransform);
            
            for (int i = 0; i < _capacity; i++)
            {
                if (_turretManagers[index].GetCurrentEmptyAmmoCount() == 0)
                {
                    _isCurrentTurretFull = true;
                    // GetAvaibleTurretTarget();
                    break;
                }
                if(_collectedAmmo == 0) break;
                _turretManagers[index].PlaceAmmoToGround(stackManager.GetStackedObject());
                _collectedAmmo--;
                _isPlacedAmmo = true;
                await Task.Delay(100);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("AmmoWarehouse")) IsAtAmmoWarehouse = true;
            if (other.CompareTag("TurretAmmoHolder")) IsAtTurretAmmoHolder = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("AmmoWarehouse")) IsAtAmmoWarehouse = false;
            if (other.CompareTag("TurretAmmoHolder"))
            {
                IsAtTurretAmmoHolder = false;
                _isPlacedAmmo = false;
            }
        }
    }
}