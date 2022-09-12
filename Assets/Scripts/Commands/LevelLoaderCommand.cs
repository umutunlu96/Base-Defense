using UnityEngine;

namespace Commands
{
    public class LevelLoaderCommand : MonoBehaviour
    {
        public void InitializeLevel(int _levelID, Transform levelHolder)
        {
            Instantiate(Resources.Load<GameObject>($"LevelPrefabs/Runner/Level{_levelID}"), levelHolder);
        }
    }
}