using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
UI element to show the user whenever they obtain treats as well as adding the
amound of treats obtained to the gamemanager.
*/

public class CoinScore : MonoBehaviour
{
    public GameManager instance;
    public Text MyscoreText;
    private int treatNum;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        treatNum = instance.dollar;
        MyscoreText.text = ": " + treatNum;
    }
    private void Update()
    {
        treatNum = instance.dollar;
        MyscoreText.text = ": " + treatNum;
    }
}

