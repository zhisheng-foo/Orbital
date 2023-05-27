using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

