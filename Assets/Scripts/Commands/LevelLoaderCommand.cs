using UnityEngine;

namespace Commands
{
    public class LevelLoaderCommand : MonoBehaviour
    {
        public void InitializeLevel(int _levelID, Transform levelHolder)
        {
            Instantiate(Resources.Load<GameObject>($"Levels/Level{_levelID}"), levelHolder);
        }
    }
}