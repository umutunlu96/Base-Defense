using System.Threading.Tasks;
using Signals;
using UnityEngine;

namespace StateMachine.Boss
{
    public class Grenade : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidbody;
        private Transform _playerTransform;
        private GameObject _explosion;
        
        public void Launch()
        {
            _playerTransform = PlayerSignals.Instance.onGetPlayerTransfrom();
            
            Vector3 Vo = Throw(_playerTransform, .5f);
            transform.rotation = Quaternion.LookRotation(Vo);
            rigidbody.useGravity = true;
            rigidbody.velocity = Vo;
        }
        
        private Vector3 Throw(Transform playerTransform, float time)
        {
            gameObject.transform.SetParent(null);

            Vector3 distance = playerTransform.position - transform.position;
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
        }

        private void OnDisable()
        {
            rigidbody.useGravity = false;
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
            rigidbody.velocity = Vector3.zero;
            PoolSignals.Instance.onReleasePoolObject?.Invoke("Grenade", gameObject);
        }

        private void GetExplosionParticle()
        {
            _explosion = null;
            _explosion = PoolSignals.Instance.onGetPoolObject?.Invoke("Explosion", transform);
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