using UnityEngine;

public class Item : MonoBehaviour
{
    public int scoreValue = 10;

    private void OnTriggerEnter(Collider healthcare)
    {
        if (healthcare.CompareTag("Player"))
        {
            FindAnyObjectByType<GameManager>().AddScore(scoreValue);
            Destroy(gameObject);
        }
    }
}
