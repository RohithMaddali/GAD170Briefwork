using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Playerstats : MonoBehaviour
{
    public Stats myStats;
    void Start()
    {
        myStats = GetComponent<Stats>();
    }

    //These are the initial player stats when he startes the game and is at level 1
}
