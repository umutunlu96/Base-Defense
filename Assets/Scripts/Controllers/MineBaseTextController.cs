using TMPro;
using UnityEngine;

namespace Controllers
{
    public class MineBaseTextController : MonoBehaviour
    {
        [SerializeField] private TextMeshPro mineText;

        public void SetText(int currentAmount, int maxAmount)
        {
            if (currentAmount < maxAmount)
            {
                mineText.text = $"<color=green>{currentAmount}/{maxAmount}</color>\nGEM MINE";
                return;
            }
            mineText.text = $"<color=red>{currentAmount}/{maxAmount}</color>\nGEM MINE";
        }
    }
}