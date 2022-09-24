using TMPro;
using UnityEngine;

namespace Controllers
{
    public class LevelPanelController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        // [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI gemText;
        
        #endregion

        #endregion


        public void SetLevelText(int value)
        {
            // levelText.text = value.ToString();
        }

        public void SetMoneyText(int value)
        {
            moneyText.text = value.ToString();
        }
        
        public void SetGemText(int value)
        {
            gemText.text = value.ToString();
        }
    }
}