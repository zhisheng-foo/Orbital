using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManagerVictory : MonoBehaviour
{
    public AudioClip BGM;
    public AudioClip ReZeroMonsterSpawn;
    public AudioClip convoaudio;

    private AudioSource audiosource;

    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        StartCoroutine(PlayAudioClips());
    }

    IEnumerator PlayAudioClips()
    {
        /*
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        audiosource.clip = BGM;
        audiosource.Play();
        */
        yield return new WaitForSeconds(9.3f); // Wait for 11.3 - 2 = 9.3 seconds
        audiosource.clip = convoaudio;
        audiosource.Play();

        yield return new WaitForSeconds(17.7f); // Wait for 29 - 11.3 = 17.7 seconds
        audiosource.clip = ReZeroMonsterSpawn;
        audiosource.Play();

        yield return new WaitForSeconds(10f); // Wait for 40 - 29 = 10 seconds
        SceneManager.LoadScene("Start Game");
    }
    
    // Update is called once per frame
    void Update()
    {

    }
}
