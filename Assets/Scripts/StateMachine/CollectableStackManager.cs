using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            _collectedAmount++;
            if (_collectedAmount % _stackData.MaxHeight != 0)
            {
                _nextPos += new Vector3(0, _stackData.OffsetY, 0);
            }
            else if (_collectedAmount % _stackData.MaxHeight == 0)
            {
                _nextPos = Vector3.zero;
                _nextPos += new Vector3(0, _stackData.OffsetY, -_stackData.OffsetZ);
            }
            
            collectable.SetParent(transform);
            collectable.DOLocalMove(_nextPos, 1).SetEase(Ease.InOutBack);
            collectable.transform.localRotation = Quaternion.Euler(0,0,0);
            collectableList.Add(collectable);
            
            collectable.gameObject.tag = "Collected";
        }
        
        public async void RemoveStackAll()
        {
            for (int i = 0; i < collectableList.Count; i++)
            {
                Vector3 pos1 = new Vector3(collectableList[i].transform.localPosition.x + Random.Range(-2, 2), collectableList[i].transform.localPosition.y + 3, collectableList[i].transform.localPosition.z + Random.Range(-2, 2));
                Vector3 pos2 = new Vector3(collectableList[i].transform.localPosition.x + Random.Range(-2, 2), collectableList[i].transform.localPosition.y - 5, collectableList[i].transform.localPosition.z + Random.Range(-2, 2));
                collectableList[i].transform.DOLocalPath(new Vector3[2] { pos1, pos2 }, 0.5f);
                await Task.Delay(100);
            }
            await Task.Delay(200);
            RemoveAllList();
        }

        private void RemoveAllList()
        {
            foreach (var money in collectableList)
            {
                var o = money.gameObject;
                o.tag = "Money";
                PoolSignals.Instance.onReleasePoolObject?.Invoke("Money", o);
            }
            collectableList.Clear();
            _collectedAmount = 0;
            InitializeStackPos();
        }
    }
}