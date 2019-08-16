using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    public List<GameObject> EnemySpawnList;
    public List<GameObject> EnemiesToFight;
    private GameObject battleUIManager;
    

    public enum Worlds
    {
        WorldView,
        BattleScene
    }

    //void awake is called before void start or any object
    private static GameManager gameManRef;

    void Awake()
    {
        //this will make it so we can travel between scenes (good for keeping track of game play)
        battleUIManager = GameObject.FindGameObjectWithTag("BattleUIManager");
        if(battleUIManager != null)
        battleUIManager.GetComponent<BattleUIManager>().CallHardReset += DeleteSavedStuff;
        if (gameManRef == null)
        {
            gameManRef = this;
            //This will make it so we can travel between scenes (good for keeping track of gameplay!)
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
        
    }

    void Start()
    {
        LoadPlayerStuff(true);
    }

    public void TravelToWorld(Worlds destination)
    {
        switch (destination)
        {
            case Worlds.WorldView:
                //load overworld scene;
                SavePlayerStuff(false);
                SceneManager.LoadScene("WorldView");
                //LoadPlayerStuff(true);
                break;
            case Worlds.BattleScene:
                //load battlescene
                SavePlayerStuff(true);
                SceneManager.LoadScene("BattleScene");
                //LoadPlayerStuff(false);
                break;
        }
    }

    void SavePlayerStuff(bool isFromOverworld)
    {
        string tag;
        if (isFromOverworld)
        {
            tag = "Player";
        }
        else
        {
            tag = "BattlePlayer";
        }
        GameObject playerObj = GameObject.FindGameObjectWithTag(tag);
        //only save position in overworld
        if (isFromOverworld)
        {
            //save both location and rotation as seperate floats, using similar naming conventions
            //storing position values
            PlayerPrefs.SetFloat("PlayerPosx", transform.position.x);
            PlayerPrefs.SetFloat("PlayerPosy", transform.position.y);
            PlayerPrefs.SetFloat("PlayerPosz", transform.position.z);
            //storing rotation values
            PlayerPrefs.SetFloat("PlayerRotx", transform.rotation.x);
            PlayerPrefs.SetFloat("PlayerRoty", transform.rotation.y);
            PlayerPrefs.SetFloat("PlayerRotz", transform.rotation.z);
        }
        
        Stats playerStats = GameObject.FindGameObjectWithTag(tag).GetComponent<Stats>();
        PlayerPrefs.SetFloat("playerHealth", playerStats.health);
        PlayerPrefs.SetInt("playerLevel", playerStats.level);
        PlayerPrefs.SetInt("playerDamage", playerStats.attack);
        PlayerPrefs.SetInt("playerExpReq", playerStats.reqExp);
        PlayerPrefs.SetInt("playerGainedExp", playerStats.expGained);
        PlayerPrefs.SetFloat("playerDefense", playerStats.defense);
        PlayerPrefs.SetFloat("playerHealthMax", playerStats.maxHP);
    }

    public void LoadPlayerStuff(bool goingToOverworld)
    {
        string tag;
        if(goingToOverworld)
        {
            tag = "Player";
        }
        else
        {
            tag = "BattlePlayer";
        }
        //load the existing stats and apply them to the player!
        GameObject player = GameObject.FindGameObjectWithTag(tag);
        Stats playerStats = player.GetComponent<Stats>();
        //Debug.Log(player.name);
        playerStats.health = PlayerPrefs.GetFloat("playerHealth", 100f);
        playerStats.level = PlayerPrefs.GetInt("playerLevel", playerStats.level);
        playerStats.attack = PlayerPrefs.GetInt("playerDamage", playerStats.attack);
        playerStats.reqExp = PlayerPrefs.GetInt("playerExpReq", playerStats.reqExp);
        playerStats.expGained = PlayerPrefs.GetInt("playerGainedExp", playerStats.expGained);
        playerStats.defense = PlayerPrefs.GetFloat("playerDefense", playerStats.defense);
        playerStats.maxHP = PlayerPrefs.GetFloat("playerHealthMax", playerStats.maxHP);

        //load position only in overworld
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (goingToOverworld)
        {
            playerObj.transform.position = new Vector3(PlayerPrefs.GetFloat("playerPosx", 8.14f), PlayerPrefs.GetFloat("playerPosy", 2f),
                                                   PlayerPrefs.GetFloat("playerPosz", -1.43f));
            playerObj.transform.rotation = Quaternion.Euler(PlayerPrefs.GetFloat("playerRotx", 0f), PlayerPrefs.GetFloat("playerRoty", 2f),
                                                   PlayerPrefs.GetFloat("playerRotz", 0f));
        }

    }

    public void DeleteSavedStuff()
    {
        //used to reset the entire game
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("WorldView");
        Debug.Log("Save Data Deleted");
    }

   

    // Update is called once per frame
    void Update()
    {
       
    }
}
