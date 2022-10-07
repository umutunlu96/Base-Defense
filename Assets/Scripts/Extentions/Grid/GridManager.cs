using UnityEngine;

namespace Extentions.Grid
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private GridType _gridType;
        private Vector3 _scale;
        private Vector3 _placementPoint;
        private GridData _gridData;
        private int _maxPlacementInLevel;
        private int _placementCount = 0;

        private float _gridX;
        private float _gridZ;
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
            _scale = transform.localScale;
            _gridX = _scale.x * 10 / (_gridData.row * 2);
            _gridZ = _scale.z * 10 / (_gridData.column * 2);
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                print(GetPlacementVector(1));
            }

            if (Input.GetMouseButtonDown(1))
            {
                print(GetPlacementVector(-1));
            }
        }
        
        private Vector3 GetPlacementVector(int increment)
        {
            _placementCount += increment;
            if(_placementCount <= 0) { _placementCount = 0; return Vector3.zero;}
            _placementPoint = new Vector3((-_scale.x * 10 / 2), _gridData.groundOffsetY, (_scale.z * 10 / 2));
            
            int row = Mathf.CeilToInt((((float)_placementCount % _maxPlacementInLevel) / _gridData.row)) == 0 ? 
                Mathf.CeilToInt(1f / _gridData.row) : 
                Mathf.CeilToInt((((float)_placementCount % _maxPlacementInLevel) / _gridData.row)); 
            int column = _placementCount % _gridData.row != 0 ? _placementCount % _gridData.row : _gridData.row;
            _placementPoint.x += ((column * 2) -1) * _gridX;
            _placementPoint.y += Mathf.CeilToInt((float) _placementCount / _maxPlacementInLevel) > 1
                ? _gridData.levelOffsetY * Mathf.CeilToInt((float) _placementCount / _maxPlacementInLevel) - 1 : 0;
            _placementPoint.z -= ((row * 2) - 1) * _gridZ;
            return _placementPoint;
        }
        
        public void PlaceObjectOnGrid(Transform Obj)
        {
            Obj.transform.position = GetPlacementVector(1);
        }

        public void GetObjectOnGrid()
        {
            _placementCount--;
        }
    }
}