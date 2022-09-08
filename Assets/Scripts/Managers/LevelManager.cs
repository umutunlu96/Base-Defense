using System;
using Controllers;
using Data.UnityObject;
using Data.ValueObject;
using Commands;
using Keys;
using Signals;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        [Header("Data")] public LevelData Data;

        #endregion

        #region Serialized Variables

        [SerializeField] private GameObject levelHolder;
        
        #endregion

        #region Private Variables
        
        private int _levelID;
        private LevelLoaderCommand levelLoader = new LevelLoaderCommand();
        private ClearActiveLevelCommand levelClearer = new ClearActiveLevelCommand();

        #endregion

        #endregion

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _levelID = GetActiveLevel();
        }
        
        private int GetActiveLevel()
        {
            if (!ES3.FileExists()) return 1;
            return ES3.KeyExists("Level") ? ES3.Load<int>("Level") : 1;
        }

        private int GetActiveIdleLevel()
        {
            if (!ES3.FileExists()) return 0;
            return ES3.KeyExists("IdleLevel") ? ES3.Load<int>("IdleLevel") : 0;
        }
        
        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            LevelSignals.Instance.onLevelInitialize += OnInitializeLevel;
            LevelSignals.Instance.onClearActiveLevel += OnClearActiveLevel;
            LevelSignals.Instance.onNextLevel += OnNextLevel;
            LevelSignals.Instance.onRestartLevel += OnRestartLevel;
        }

        private void UnsubscribeEvents()
        {
            LevelSignals.Instance.onLevelInitialize -= OnInitializeLevel;
            LevelSignals.Instance.onClearActiveLevel -= OnClearActiveLevel;
            LevelSignals.Instance.onNextLevel -= OnNextLevel;
            LevelSignals.Instance.onRestartLevel -= OnRestartLevel;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void Start()
        {
            OnInitializeLevel();
            SetLevelText();
        }

        private void OnNextLevel()
        {
            _levelID++;
            LevelSignals.Instance.onClearActiveLevel?.Invoke();
            CoreGameSignals.Instance.onReset?.Invoke();
            LevelSignals.Instance.onLevelInitialize?.Invoke();
            SetLevelText();
        }

        private void OnRestartLevel()
        {
            LevelSignals.Instance.onClearActiveLevel?.Invoke();
            CoreGameSignals.Instance.onReset?.Invoke();
            LevelSignals.Instance.onLevelInitialize?.Invoke();
        }

        private int OnGetLevelID()
        {
            return _levelID;
        }

        private int GetLevelCount()
        {
            return _levelID % Resources.Load<CD_Level>("Data/CD_Level").Levels.Count;
        }

        private void SetLevelText()
        {
            UISignals.Instance.onSetLevelText?.Invoke(_levelID);
        }
        
        private void OnInitializeLevel()
        {
            levelLoader.InitializeLevel(GetLevelCount(), levelHolder.transform);
        }

        private void OnClearActiveLevel()
        {
            levelClearer.ClearActiveLevel(levelHolder.transform);
        }
    }
}