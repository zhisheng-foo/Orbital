using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bar_hp_boss : MonoBehaviour
{
    public Enemy boss;
    public Slider boss_slider;
    private string playerName = "Player";
    public float distanceThreshold = 20f; 
    private bool isInRange = false;
    private GameObject playerObject;
    private bool isSliderVisible = false;
    void Awake()
    {
        boss_slider = GetComponent<Slider>();
        boss_slider.gameObject.SetActive(true); 
        boss_slider.enabled = false; 
        playerObject = GameObject.Find(playerName); 
        if (playerObject == null)
        {
            Debug.LogWarning("Player object not found!");
        }
    }
    void Update()
    {
        if (playerObject == null)
        {
            return;
        }

        float distanceToBoss = Vector3.Distance(playerObject.transform.position, boss.transform.position);

        if (distanceToBoss <= distanceThreshold)
        {
            isInRange = true;
            if (!isSliderVisible) 
            {
                boss_slider.enabled = true; 
                isSliderVisible = true;
            }
            float fillValue = (float)boss.hitpoint / (float)boss.maxHitpoint;
            boss_slider.value = fillValue;
        }
        else
        {
            isInRange = false;
            if (isSliderVisible) 
            {
                boss_slider.enabled = false; 
                isSliderVisible = false;
            }
        }
    }
}
