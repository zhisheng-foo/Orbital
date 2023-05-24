using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinScore : MonoBehaviour
{   

    public GameManager instance;
    public Text MyscoreText;
    private int treatNum;
    
    void Start()
    {

        treatNum = instance.dollar;
     
        MyscoreText.text = ": " + treatNum;

    }

    void Update()
    {
        treatNum = instance.dollar;
        MyscoreText.text = ": " + treatNum;
    }

}
