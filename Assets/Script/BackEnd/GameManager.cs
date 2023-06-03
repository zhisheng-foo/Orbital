using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public FloatingTextManager FloatingTextManager;

    private void Awake() {
        if (GameManager.instance != null) {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(FloatingTextManager.gameObject);
            return;
        }

        PlayerPrefs.DeleteAll();
        
        instance = this;
        //SceneManager.sceneLoaded += LoadState;   
        DontDestroyOnLoad(gameObject);
    }

    

    //Floating Text
    public void ShowText(string msg, int fontSize, Color color,
     Vector3 position, Vector3 motion, float duration) 
     {
        FloatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    //Resources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;

    public List<int> weaponPrices;
    public List<int> xpTable;

    //References
    
    //References 
    public Player player;

    public Weapon weapon;

    //public weapon
    //public Weapon weapon;
    //public FloatingTextManager FloatingTextManager;

    //Logic 
    public int dollar = 0;
    public int experience;

    //Save State
    /*
        int dollar
        int experience

    */
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

    public void LoadState(Scene s, LoadSceneMode mode)
    {
        if (!PlayerPrefs.HasKey("SaveState")) {
            return;
        }

    
        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        //dollar
        dollar = int.Parse(data[1]);
        
        //experience
        experience = int.Parse(data[2]);
        if(GetCurrentLevel() != 1) {
            player.SetLevel(GetCurrentLevel());

        }
    }

    public int GetCurrentLevel() {
        int r = 0;
        int add = 0;

        while(experience >= add) {
            add+= xpTable[r];
            r++;

            if(r == xpTable.Count) {
                return r;
            }
        }
        return r;
    }
    public int GetXpToLevel(int level) {
        
        int r = 0;
        int xp = 0;
        while(r < level) {
            xp += xpTable[r];
            r++;
        }

        return xp;
    }

    public void GrantXp(int xp) {
        int currLevel = GetCurrentLevel();
        experience += xp;
        if(currLevel < GetCurrentLevel()){
            OnLevelUp();
        }
    }

    public void OnLevelUp() {
        player.OnLevelUp();
    }

}
