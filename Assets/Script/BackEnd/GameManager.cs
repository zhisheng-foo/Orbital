using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
Handles the passing of information to the floating text class so that it can provide information
to the player and creates responsiveness to interaction with the environment.

Also stores the current amount of dollar "treats" that the player has and is updated accordingly.
*/

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public FloatingTextManager FloatingTextManager;
    public Player player;
    public Weapon weapon;
    public int dollar = 0;
    private int experience;
    
    
    private void Awake() {

        if (GameManager.instance != null) {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(FloatingTextManager.gameObject);
            return;
        }

        PlayerPrefs.DeleteAll();  
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //Passes to floating text with relevant information.
    public void ShowText(string msg, int fontSize, Color color,
     Vector3 position, Vector3 motion, float duration){
    
        FloatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    } 


    //Saves the current amount of "treats"
    public void SaveState() 
    {     
        string s = "";
        s += "0" + "|";
        s += dollar.ToString() + "|";
        s += experience.ToString() + "|";
        s += "0";
        //s += weapon.weaponLevel.ToString();
        Debug.Log("save");
        PlayerPrefs.SetString("SaveState",s);       
    }
    
    //Loads the amount of "treats"
    public void LoadState(Scene s, LoadSceneMode mode)
    {
        if (!PlayerPrefs.HasKey("SaveState")) {
            return;
        }

        string[] data = PlayerPrefs.GetString("SaveState").Split('|');
  
        dollar = int.Parse(data[1]);
        experience = int.Parse(data[2]);
    }    
}
