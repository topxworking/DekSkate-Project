using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager gm = FindAnyObjectByType<GameManager>();

            if (gm != null && gm.isGameActive)
            {
                gm.EndLevel();
            }
        }
    }
}
