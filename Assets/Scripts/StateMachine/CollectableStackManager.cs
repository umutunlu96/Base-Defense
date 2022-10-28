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
            foreach (var t in collectableList)
            {
                Vector3 colPos = t.position;
                Vector3 pos1 = new Vector3(colPos.x + Random.Range(-1f, 1f), colPos.y + 1.5f, colPos.z + Random.Range(-1f, 1f));
                Vector3 pos2 = new Vector3(pos1.x, -.2f, pos1.z);
                t.transform.DOPath(new Vector3[2] { pos1, pos2 }, 1f);
                await Task.Delay(100);
            }
            await Task.Delay(200);
            RemoveAllList();
        }

        public async void JustRemove()
        {
            if(_collectedAmount == 0) return;
            Transform baseTransform = AiSignals.Instance.onGetBaseTransform();
            foreach (var t in collectableList)
            {
                t.SetParent(baseTransform);
                
                Vector3 colPos = t.position;
                Vector3 pos1 = new Vector3(colPos.x + Random.Range(-1.4f, 1.4f), 0, colPos.z + Random.Range(-1.4f, 1.4f));
                t.transform.DOJump(pos1, .5f, 2, .2f);
            }

            await Task.Delay(5000);
            
            foreach (var t in collectableList)
            {
                var o = t.gameObject;
                if(ReturnTag != null)
                    o.tag = ReturnTag;
            }
            
            await Task.Delay(100);
            collectableList.Clear();
            _collectedAmount = 0;
            InitializeStackPos();
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