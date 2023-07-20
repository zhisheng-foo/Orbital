using UnityEngine;

/*
Zooms out the camera during the boss battle to provide the player with increased visibility, 
and provide a more thematic experience
*/

public class CameraScaling : MonoBehaviour
{
    public float bossBattleScale = 1.5f; 
    private float originalSize; 
    private GameObject boss;
    private bool isInBossBattle = false;
    private void Start()
    {
        originalSize = Camera.main.orthographicSize;
        boss = GameObject.Find("Boss");
    }
    private void Update()
    {
        if (!isInBossBattle && transform.position.y > 10 && boss != null)
        {
            SetBossBattle(true);
        }
    }
    public void SetBossBattle(bool isBossBattle)
    {
        isInBossBattle = isBossBattle;

        if (isInBossBattle)
        {
            Camera.main.orthographicSize = originalSize * bossBattleScale;
        }
        else
        {
            Camera.main.orthographicSize = originalSize;
        }
    }
    public void BossDefeated()
    {
        if (isInBossBattle)
        {
            SetBossBattle(false);
        }
    }
}
