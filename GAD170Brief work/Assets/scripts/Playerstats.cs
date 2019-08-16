using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Playerstats : MonoBehaviour
{
    public Stats myStats;
    private GameObject gameManager;

    void Start()
    {
        myStats = GetComponent<Stats>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        gameManager.GetComponent<GameManager>().LoadPlayerStuff(true);

    }
       
    }



    //These are the initial player stats when he startes the game and is at level 1

