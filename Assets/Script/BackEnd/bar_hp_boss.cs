using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bar_hp_boss : MonoBehaviour
{
    public Enemy boss;
    public Slider boss_slider;
    public string playerName = "Player"; // The name of the player GameObject

    public float distanceThreshold = 20f; // Define the distance threshold for showing the UI

    private bool isInRange = false;
    private GameObject playerObject;
    private bool isSliderVisible = false;

    void Awake()
    {
        boss_slider = GetComponent<Slider>();
        boss_slider.gameObject.SetActive(true); // Ensure the slider GameObject is active
        boss_slider.enabled = false; // Disable the slider component graphics initially
        playerObject = GameObject.Find(playerName); // Find the player object by name
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
            if (!isSliderVisible) // Show the slider if it's not already visible
            {
                boss_slider.enabled = true; // Enable the slider component graphics
                isSliderVisible = true;
            }
            float fillValue = (float)boss.hitpoint / (float)boss.maxHitpoint;
            boss_slider.value = fillValue;
        }
        else
        {
            isInRange = false;
            if (isSliderVisible) // Hide the slider if it's visible
            {
                boss_slider.enabled = false; // Disable the slider component graphics
                isSliderVisible = false;
            }
        }
    }
}
