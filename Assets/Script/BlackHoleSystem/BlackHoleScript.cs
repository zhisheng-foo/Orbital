using UnityEngine;

public class BlackHoleScript : MonoBehaviour
{
    public Transform player;
    public float influenceRange;
    public float minDistanceToPlay;
    public AudioClip suctionSound;
    public float volume = 1f;
    private Rigidbody2D playerBody;
    private AudioSource audioSource;
    private bool isPlaying;
    private void Start()
    {
        playerBody = player.GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true; 
        isPlaying = false;
    }
    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceToPlayer <= influenceRange && distanceToPlayer >= minDistanceToPlay)
        {
            if (!isPlaying)
            {
                audioSource.clip = suctionSound;
                audioSource.volume = volume;
                audioSource.Play();
                isPlaying = true;
            }
        }
        else
        {
            audioSource.Stop();
            isPlaying = false;
        }
    }
}
