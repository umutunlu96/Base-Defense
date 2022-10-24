using System.Threading.Tasks;
using Signals;
using UnityEngine;

namespace StateMachine.Boss
{
    public class Grenade : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidBody;
        private Vector3 _playerPosition;
        private GameObject _explosion;
        
        public void Launch()
        {
            Vector3 Vo = Throw(_playerPosition, .5f);
            transform.rotation = Quaternion.LookRotation(Vo);
            rigidBody.useGravity = true;
            rigidBody.velocity = Vo;
        }
        
        private Vector3 Throw(Vector3 playerPosition, float time)
        {
            gameObject.transform.SetParent(null);
            CheckPlayerPositionIfNull();
            Vector3 distance = playerPosition - transform.position;
            Vector3 distanceXZ = distance;
            distanceXZ.y = 0;

            float Sy = distance.y;
            float Sxz = distanceXZ.magnitude;

            float Vxz = Sxz / time;
            float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

            Vector3 result = distanceXZ.normalized;
            result *= Vxz;
            result.y = Vy;

            return result;
        }

        private void OnEnable()
        {
            AiSignals.Instance.onGrenadeSpawned?.Invoke(this);
            AiSignals.Instance.onGrenadeThrowed += OnGrenadeThrowed;
        }

        private void OnDisable()
        {
            _playerPosition = Vector3.zero;
            rigidBody.useGravity = false;
            AiSignals.Instance.onGrenadeThrowed -= OnGrenadeThrowed;
        }

        private void OnGrenadeThrowed()
        {
            _playerPosition = PlayerSignals.Instance.onGetPlayerTransfrom().position;
        }

        private void CheckPlayerPositionIfNull()
        {
            if (_playerPosition == Vector3.zero)
            {
                _playerPosition = PlayerSignals.Instance.onGetPlayerTransfrom().position;
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ground"))
            {
                GetExplosionParticle();
                ResetGrenade();
            }
        }

        private void ResetGrenade()
        {
            rigidBody.velocity = Vector3.zero;
            PoolSignals.Instance.onReleasePoolObject?.Invoke("Grenade", gameObject);
        }

        private void GetExplosionParticle()
        {
            _explosion = PoolSignals.Instance.onGetPoolObject?.Invoke("Explosion", transform);
            AiSignals.Instance.onGrenadeExplode?.Invoke();
            _explosion.transform.position = transform.position;
            ReturnExplosion();
        }

        private async void ReturnExplosion()
        {
            await Task.Delay(2000);
            PoolSignals.Instance.onReleasePoolObject?.Invoke("Explosion", _explosion);
        }
    }
}