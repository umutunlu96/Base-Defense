using System.Collections;
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
            StartCoroutine(RemoveStackCoroutine());
        }
        
        public IEnumerator RemoveStackCoroutine()
        {
            for (int i = 0; i < collectableList.Count; i++)
            {
                Vector3 pos1 = new Vector3(collectableList[i].transform.localPosition.x + Random.Range(-4, 4), collectableList[i].transform.localPosition.y + 10, collectableList[i].transform.localPosition.z + Random.Range(-4, 4));
                Vector3 pos2 = new Vector3(collectableList[i].transform.localPosition.x + Random.Range(-4, 4), collectableList[i].transform.localPosition.y - 30, collectableList[i].transform.localPosition.z + Random.Range(-4, 4));
                collectableList[i].transform.DOLocalPath(new Vector3[2] { pos1, pos2 }, 0.5f);
                yield return new WaitForSeconds(.1f);
            }
            yield return new WaitForSeconds(.2f);
            RemoveAllList();
        }

        private void RemoveAllList()
        {
            for (int i = 0; i < collectableList.Count; i++)
            {
                collectableList[i].gameObject.SetActive(false);
                collectableList.RemoveAt(i);
                collectableList.TrimExcess();
            }
            //Return To Pool
        }
        
    }
}