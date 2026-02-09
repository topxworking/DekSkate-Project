using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class  ObjectTransform
{
    public string objectID;
    public Vector3 position;
    public Vector3 rotation;
}

[System.Serializable]
public class LevelLayout
{
    public string levelName;
    public List<ObjectTransform> Placements = new List<ObjectTransform>();
}

[CreateAssetMenu(fileName = "MasterLevelData", menuName = "Data/MasterLevelData")]
public class MasterLevelData : ScriptableObject
{
    public List<LevelLayout> levels = new List<LevelLayout>();
}
