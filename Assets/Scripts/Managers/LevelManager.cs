using Data.UnityObject;
using Commands;
using Signals;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables
        
        #endregion

        #region Serialized Variables

        [SerializeField] private GameObject levelHolder;
        
        #endregion

        #region Private Variables
        
        private int _levelID;
        private LevelLoaderCommand _levelLoader;
        private ClearActiveLevelCommand _levelClearer;

        #endregion

        #endregion

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _levelID = GetActiveLevel();
            _levelLoader = levelHolder.AddComponent<LevelLoaderCommand>();
            _levelClearer = levelHolder.AddComponent<ClearActiveLevelCommand>();
        }
        
        private int GetActiveLevel()
        {
            if (!ES3.FileExists()) return 1;
            return ES3.KeyExists("Level") ? ES3.Load<int>("Level") : 1;
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
            LevelSignals.Instance.onGetLevelID += OnGetLevelID;
        }

        private void UnsubscribeEvents()
        {
            LevelSignals.Instance.onLevelInitialize -= OnInitializeLevel;
            LevelSignals.Instance.onClearActiveLevel -= OnClearActiveLevel;
            LevelSignals.Instance.onNextLevel -= OnNextLevel;
            LevelSignals.Instance.onRestartLevel -= OnRestartLevel;
            LevelSignals.Instance.onGetLevelID -= OnGetLevelID;
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
            // levelLoader.InitializeLevel(GetLevelCount(), levelHolder.transform);
        }

        private void OnClearActiveLevel()
        {
            _levelClearer.ClearActiveLevel(levelHolder.transform);
        }
    }
}