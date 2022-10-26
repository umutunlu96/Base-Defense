using System.Collections.Generic;
using System.Threading.Tasks;
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

        #region Public

        public string ReturnTag;
        public PoolType PoolType;
        #endregion
        
        #region Serialized
        
        [SerializeField] private List<Transform> collectableList = new List<Transform>();
        
        #endregion

        #region Private
        
        private StackData _stackData;
        private Vector3 _nextPos;
        private int _collectedAmount; 
            
        #endregion
        
        #endregion
        
        public void SetStackData(StackData stackData)
        {
            _stackData = stackData;
            InitializeStackPos();
        }
        
        private void InitializeStackPos()
        {
            _nextPos = new Vector3(0, -_stackData.OffsetY, 0);
        }
        
        public void AddStack(Transform collectable)
        {
            if (_collectedAmount == 0) InitializeStackPos();
            
            if(_collectedAmount == _stackData.Capacity) return;
            
            _collectedAmount++;
            if (_collectedAmount % _stackData.MaxHeight != 0)
            {
                _nextPos += new Vector3(0, _stackData.OffsetY, 0);
            }
            else if (_collectedAmount % _stackData.MaxHeight == 0)
            {
                _nextPos = Vector3.zero;
                _nextPos += new Vector3(0, 0, -_stackData.OffsetZ);
            }
            
            collectable.SetParent(transform);
            collectable.DOLocalMove(_nextPos, 1).SetEase(Ease.InOutBack).OnComplete(()
                => collectable.transform.localRotation = Quaternion.Euler(0,0,0));
            
            collectableList.Add(collectable);
            
            collectable.gameObject.tag = "Collected";
        }

        public Transform GetStackedObject()
        {
            if (collectableList.Count == 0) return null;
            
            _collectedAmount--;
            
            if(collectableList.Count != 1)
                _nextPos = collectableList[collectableList.Count - 1].localPosition;
            if(collectableList.Count == 1)
                _nextPos = collectableList[0].localPosition;
            
            Transform collectableCache = collectableList[collectableList.Count - 1];
            collectableCache.SetParent(null);
            collectableList.RemoveAt(collectableList.Count - 1);
            collectableList.TrimExcess();
            

            return collectableCache;
        }
        
        public async void RemoveStackAll()
        {
            if(_collectedAmount == 0) return;
            for (int i = 0; i < collectableList.Count; i++)
            {
                Vector3 pos1 = new Vector3(collectableList[i].transform.localPosition.x + Random.Range(-1, 1), collectableList[i].transform.localPosition.y + 1, collectableList[i].transform.localPosition.z + Random.Range(-1, 1));
                Vector3 pos2 = new Vector3(collectableList[i].transform.localPosition.x + Random.Range(-1, 1), collectableList[i].transform.localPosition.y - 1, collectableList[i].transform.localPosition.z + Random.Range(-1, 1));
                collectableList[i].transform.DOLocalPath(new Vector3[2] { pos1, pos2 }, 0.5f);
                await Task.Delay(100);
            }
            await Task.Delay(200);
            RemoveAllList();
        }

        private void RemoveAllList()
        {
            foreach (var collectable in collectableList)
            {
                var o = collectable.gameObject;
                if(ReturnTag != null)
                    o.tag = ReturnTag;
                PoolSignals.Instance.onReleasePoolObject?.Invoke(PoolType.ToString(), o);
            }
            collectableList.Clear();
            _collectedAmount = 0;
            InitializeStackPos();
        }
    }
}