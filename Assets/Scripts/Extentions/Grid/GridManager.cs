using Sirenix.OdinInspector;
using UnityEngine;

namespace Extentions.Grid
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private GridType _gridType;
        [ShowInInspector] private Vector3 _position;
        [ShowInInspector] private Vector3 _scale;
        [ShowInInspector] private Vector3 _placementPoint;
        private GridData _gridData;
        [ShowInInspector] private int _maxPlacementInLevel;
        [ShowInInspector] private int _placementCount = 0;
        [ShowInInspector] private float _gridX;
        [ShowInInspector] private float _gridY;
        [ShowInInspector] private float _gridZ;
        private void Awake()
        {
            GetData();
            _maxPlacementInLevel = _gridData.row * _gridData.column;
        }

        private void Start()
        {
            InitializeGrid();
        }

        private void GetData() => _gridData = Resources.Load<CD_Grid>("Data/CD_Grid").GridDatas[_gridType];
        
        private void InitializeGrid()
        {
            _position = transform.position;
            _scale = transform.lossyScale;
            _gridX = _scale.x * 10 / (_gridData.row * 2);
            _gridZ = _scale.z * 10 / (_gridData.column * 2);
            _gridY = _gridData.levelOffsetY;
        }
        
        public Vector3 GetPlacementVector()
        {
            _placementCount ++;
            if(_placementCount <= 0) { _placementCount = 0; return Vector3.zero;}
            _placementPoint = new Vector3((-_scale.x * 10 / 2), _gridData.groundOffsetY, (_scale.z * 10 / 2));
            _placementPoint = _position + _placementPoint;
            int row = Mathf.CeilToInt((((float)_placementCount % _maxPlacementInLevel) / _gridData.row)) == 0 ? 
                Mathf.CeilToInt((float)_maxPlacementInLevel / _gridData.row) : 
                Mathf.CeilToInt((((float)_placementCount % _maxPlacementInLevel) / _gridData.row)); 
            int column = _placementCount % _gridData.row != 0 ? _placementCount % _gridData.row : _gridData.row;
            _placementPoint.x += ((column * 2) -1) * _gridX;
            _placementPoint.y += Mathf.CeilToInt((float) _placementCount / _maxPlacementInLevel) > 1
                ? _gridY * Mathf.CeilToInt((float) _placementCount / _maxPlacementInLevel) - _gridY: 0;
            _placementPoint.z -= ((row * 2) - 1) * _gridZ;
            // print(_placementPoint);
            return _placementPoint;
        }
        
        public void ReleaseObjectOnGrid()
        {
            _placementCount--;
        }

        public void ReleaseAllObjectsOnGrid()
        {
            _placementCount = 0;
        }
    }
}