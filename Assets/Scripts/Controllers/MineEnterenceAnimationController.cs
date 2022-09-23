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
            _sequence.Append(mineCart.DOLocalMoveZ(5.5f, 2.5f).SetEase(Ease.InOutBack).SetDelay(3));
            _sequence.Append(mineCart.DOLocalMoveZ(1.5f, 2.5f).SetEase(Ease.InOutBack).SetDelay(3));
            _sequence.SetLoops(-1);
        }
        
        private void OnDisable()
        {
            _sequence.Kill();
        }
    }
}