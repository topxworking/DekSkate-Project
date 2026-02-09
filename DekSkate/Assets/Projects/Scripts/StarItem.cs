using UnityEngine;

public class StarItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager gm = FindAnyObjectByType<GameManager>();
            if (gm != null)
            {
                gm.CollectStar();
                Destroy(gameObject);
            }
        }
    }
}
