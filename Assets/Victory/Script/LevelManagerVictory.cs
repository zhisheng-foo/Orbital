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
        GameManager.instance.dollar = 0;
    }

    IEnumerator PlayAudioClips()
    {
        
        yield return new WaitForSeconds(9.3f); 
        audiosource.clip = convoaudio;
        audiosource.Play();

        yield return new WaitForSeconds(17.7f); 
        audiosource.clip = ReZeroMonsterSpawn;
        audiosource.Play();

        yield return new WaitForSeconds(15f); 
        SceneManager.LoadScene("Start Game");
    }
    
    void Update()
    {

    }
}
