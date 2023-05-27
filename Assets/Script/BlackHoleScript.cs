using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackHoleScript : MonoBehaviour
{
    public Transform player;
    public float influenceRange;
    public float intensity;
    public float distanceToPlayer;
    public AudioClip suctionSound;
    public float volume = 1f;

    private Rigidbody2D playerBody;
    private AudioSource audioSource;

    private Vector2 pullForce;

    private static BlackHoleScript instance;
    private bool isInMainScene;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        playerBody = player.GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isInMainScene = (scene.name == "Main");
        gameObject.SetActive(isInMainScene);
    }

    private void Update()
    {
        if (!isInMainScene)
            return;

        distanceToPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceToPlayer <= influenceRange)
        {
            pullForce = (transform.position - player.position).normalized / distanceToPlayer * intensity;
            playerBody.AddForce(pullForce, ForceMode2D.Force);

            if (!audioSource.isPlaying)
            {
                audioSource.clip = suctionSound;
                audioSource.volume = volume;
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }
}

