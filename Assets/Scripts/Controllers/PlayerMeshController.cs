using UnityEngine;

namespace Controllers
{
    public class PlayerMeshController : MonoBehaviour
    { 
        private SkinnedMeshRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<SkinnedMeshRenderer>();
        }

        public void ChangeMaterialColor(Color color)
        {
            _renderer.material.color = color;
        }
    }
}