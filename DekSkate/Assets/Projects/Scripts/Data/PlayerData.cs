using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    public int currentLife = 3;
    public int maxLife = 3;

    public int totalScore = 0;

    public void ResetData()
    {
        currentLife = maxLife;
        totalScore = 0;
    }
}
