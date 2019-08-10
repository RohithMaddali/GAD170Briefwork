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
        //LoadPlayerStuff(true);
    }

    public void TravelToWorld(Worlds destination)
    {
        switch (destination)
        {
            case Worlds.WorldView:
                //load overworld scene;
                SavePlayerStuff(false);
                SceneManager.LoadScene("WorldView");
                LoadPlayerStuff(true);
                break;
            case Worlds.BattleScene:
                //load battlescene
                SavePlayerStuff(true);
                SceneManager.LoadScene("BattleScene");
                LoadPlayerStuff(false);
                break;
        }
    }

    void SavePlayerStuff(bool isFromOverworld)
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
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
        Stats playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();
        PlayerPrefs.SetFloat("playerHealth", playerStats.health);
        PlayerPrefs.SetInt("playerLevel", playerStats.level);
        PlayerPrefs.SetInt("playerDamage", playerStats.attack);
        PlayerPrefs.SetInt("playerExpReq", playerStats.reqExp);
        PlayerPrefs.SetInt("playerTotalExp", playerStats.TotalExp);
        PlayerPrefs.SetInt("playerGainedExp", playerStats.expGained);
        PlayerPrefs.SetFloat("playerDefense", playerStats.defense);
        PlayerPrefs.SetFloat("playerHealthMax", playerStats.maxHP);
    }

    void LoadPlayerStuff(bool goingToOverworld)
    {
        //load the existing stats and apply them to the player!
        Stats playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();
        playerStats.health = PlayerPrefs.GetFloat("playerHealth", 100f);
        playerStats.level = PlayerPrefs.GetInt("playerLevel", 0);
        playerStats.attack = PlayerPrefs.GetInt("playerDamage", 10);
        playerStats.reqExp = PlayerPrefs.GetInt("playerExpReq", 0);
        playerStats.TotalExp = PlayerPrefs.GetInt("playerTotalExp", 0);
        playerStats.expGained = PlayerPrefs.GetInt("playerGainedExp", 0);
        playerStats.defense = PlayerPrefs.GetFloat("playerDefense", 0);
        playerStats.maxHP = PlayerPrefs.GetFloat("playerHealthMax", 100f);

        //load position only in overworld
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (goingToOverworld)
        {
            playerObj.transform.position = new Vector3(PlayerPrefs.GetFloat("playerPosx", 0f), PlayerPrefs.GetFloat("playerPosy", 2f),
                                                   PlayerPrefs.GetFloat("playerPosz", 0f));
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
