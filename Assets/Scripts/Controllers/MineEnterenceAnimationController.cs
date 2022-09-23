using DG.Tweening;
using UnityEngine;

namespace Controllers
{
    public class MineEnterenceAnimationController : MonoBehaviour
    {
        [SerializeField] private Transform mineCart;

        private Sequence _sequence;

        private void Awake()
        {
            _sequence = DOTween.Sequence();
        }

        private void OnEnable()
        {
            _sequence.Append(mineCart.DOLocalMoveZ(6, 2.5f).SetEase(Ease.InOutBack).SetDelay(3));
            _sequence.Append(mineCart.DOLocalMoveZ(1, 2.5f).SetEase(Ease.InOutBack).SetDelay(3));
            _sequence.Restart();
        }
        
        private void OnDisable()
        {
            _sequence.Kill();
        }
    }
}