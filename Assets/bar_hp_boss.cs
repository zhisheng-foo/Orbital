using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class bar_hp_boss : MonoBehaviour
{
    public Boss1 boss;
    public Slider boss_slider;
    

    void Awake()
    {
        boss_slider = GetComponent<Slider>();
    } 

    void Update()
    {
        float fillValue = (float)boss.hitpoint / (float)boss.maxHitpoint;
        boss_slider.value = fillValue;
        Debug.Log("slider has been changed");


        
    }

}
