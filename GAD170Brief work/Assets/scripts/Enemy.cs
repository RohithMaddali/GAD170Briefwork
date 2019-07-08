using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Stats myStats;
    public int enemyID = 1;
    public GameObject GameManager;
    public enum EnemyTypes
    {
        tank,
        agility,
        none
    }
    public EnemyTypes myType;
    void Start()
    {
        //finding our game manager
        GameManager = GameObject.FindGameObjectWithTag("GameManager");
        myStats = GetComponent<Stats>();
        switch (myType)
        {
            case EnemyTypes.tank:
                //will have more health
                break;
            case EnemyTypes.agility:
                //will have more attack speed
                break;
            case EnemyTypes.none:
                //will be noremal enemy with normal attack speed and damage
                break;
        }
    }
        public void Defeated()
        {
            GameManager.GetComponent<GameManager>().RemoveEnemy(gameObject);
        }
    }
