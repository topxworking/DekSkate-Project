using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public MasterLevelData masterLevelData;
    public ObjectPoolingManager pooler;

    void Start()
    {
        PlaceObjects();
    }

    void PlaceObjects()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (sceneIndex < masterLevelData.levels.Count)
        {
            LevelLayout currentLevel = masterLevelData.levels[sceneIndex];

            foreach (ObjectTransform info in currentLevel.Placements)
            {
                GameObject obj = pooler.GetFromPool(info.objectID);

                if (obj != null)
                {
                    obj.transform.position = info.position;
                    obj.transform.rotation = Quaternion.Euler(info.rotation);
                }
            }
        }
    }
}
