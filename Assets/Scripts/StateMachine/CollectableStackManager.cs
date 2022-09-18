using System.Collections.Generic;
using Commands;
using Data.UnityObject;
using Data.ValueObject;
using DG.Tweening;
using Enums;
using Signals;
using UnityEngine;

namespace StateMachine
{
    public class CollectableStackManager : MonoBehaviour
    {
        #region Variables

        #region Serialized

        [SerializeField] private StackType stackType;
        [SerializeField] private List<Transform> collectableList = new List<Transform>();
        
        #endregion

        #region Private

        private AddStackCommand _addStackCommand;
        private RemoveStackCommand _removeStackCommand;
        
        private StackData _stackData;
        private Vector3 _nextPos;
        
        #endregion
        
        #endregion
        
        private StackData GetStackData() => Resources.Load<CD_StackData>("Data/CD_StackData").StackDatas[(int)stackType];
        
        private void Awake()
        {
            Initialize();
        }
        
        private void Initialize()
        {
            _stackData = GetStackData();
            _nextPos = new Vector3(0, 0, 0);
            // _addStackCommand = new AddStackCommand(ref collectableList, transform, ref _nextPos);
            // _removeStackCommand = new RemoveStackCommand();
        }
        
        public void AddStack(Transform collectable)
        {
            collectable.SetParent(transform);
            collectable.DOLocalMove(_nextPos, 1).OnComplete(()=> collectable.localPosition = _nextPos).SetEase(Ease.InOutBack);
            collectable.transform.localRotation = Quaternion.Euler(0,0,0);
            collectableList.Add(collectable);
            
            _nextPos += new Vector3(0, _stackData.OffsetY, 0);
            collectable.gameObject.tag = "Collected";
        }

        public void RemoveStackAll()
        {
            foreach (var collectable in collectableList)
            {
                
            }
        }
    }
}