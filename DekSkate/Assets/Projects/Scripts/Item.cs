using UnityEngine;

public class Item : MonoBehaviour
{
    public int scoreValue = 100;
    public AudioClip sound;

    private void OnTriggerEnter(Collider item)
    {
        if (item.CompareTag("Player"))
        {
            AudioSource[] allAudioSource = item.GetComponents<AudioSource>();

            AudioSource sfxSource = null;
            foreach (AudioSource source in allAudioSource)
            {
                if (!source.loop)
                {
                    sfxSource = source;
                    break;
                }
            }

            if (sfxSource != null && sound != null)
            {
                sfxSource.PlayOneShot(sound);
            }

            GameManager gm = FindAnyObjectByType<GameManager>();
            if (gm != null)
            {
                gm.AddScore(scoreValue);
            }

            Destroy(gameObject);
        }
    }
}
