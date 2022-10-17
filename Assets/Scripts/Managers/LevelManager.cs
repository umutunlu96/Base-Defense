using Data.UnityObject;
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
            // return SaveLoadSignals.Instance.onLevelLoad().Level;
            print("GetactiveLevel");
            if (!ES3.FileExists()) return 1;
            print("file not exist");
            return ES3.KeyExists("Level") ? ES3.Load<int>("Level") : 1;
        }
        
        
        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onPlay += OnInitializeLevel;
            LevelSignals.Instance.onLevelInitialize += OnInitializeLevel;
            LevelSignals.Instance.onClearActiveLevel += OnClearActiveLevel;
            LevelSignals.Instance.onNextLevel += OnNextLevel;
            LevelSignals.Instance.onRestartLevel += OnRestartLevel;
            LevelSignals.Instance.onGetLevelID += OnGetLevelID;
            LevelSignals.Instance.onGetLevelCount += GetLevelCount;
            
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onPlay -= OnInitializeLevel;
            LevelSignals.Instance.onLevelInitialize -= OnInitializeLevel;
            LevelSignals.Instance.onClearActiveLevel -= OnClearActiveLevel;
            LevelSignals.Instance.onNextLevel -= OnNextLevel;
            LevelSignals.Instance.onRestartLevel -= OnRestartLevel;
            LevelSignals.Instance.onGetLevelID -= OnGetLevelID;
            LevelSignals.Instance.onGetLevelCount -= GetLevelCount;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void Start()
        {
            SetLevelText();
        }

        private void OnNextLevel()
        {
            _levelID++;
            print(_levelID);
            SaveLoadSignals.Instance.onLevelSave?.Invoke();
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
            // return SaveLoadSignals.Instance.onLevelLoad().Level;
            return _levelID;
        }

        private int GetLevelCount()
        {
            print("GetlevelID"+_levelID);
            if (_levelID % Resources.Load<CD_Level>("Data/CD_Level").Levels.Count == 0)
                return 2;
            return _levelID % Resources.Load<CD_Level>("Data/CD_Level").Levels.Count;
        }

        private void SetLevelText()
        {
            UISignals.Instance.onSetLevelText?.Invoke(_levelID);
        }
        
        private void OnInitializeLevel()
        {
            _levelLoader.InitializeLevel(GetLevelCount(), levelHolder.transform);
            InputSignals.Instance.onEnableInput?.Invoke();
        }

        private void OnClearActiveLevel()
        {
            _levelClearer.ClearActiveLevel(levelHolder.transform);
        }
    }
}