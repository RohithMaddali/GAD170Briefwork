using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public Playerstats Pstats;
    public Enemystats Estats;
    void Start()
    {
        
    }




    // Update is called once per frame
    void Update()
    {
        
        Pstats = GetComponent<Playerstats>();
        Estats = GetComponent<Enemystats>();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Checker for successful interaction based on chances for different enemies.
            int EH = Random.Range(Estats.health, Estats.health + 101);
            int Hdiff = EH - Pstats.Health;
            if (Hdiff < Pstats.Health)
            {
                if (Random.Range(1, 11) > 2)
                {
                    Attack(EH, Pstats.Dmg);
                }
                else
                {
                    Debug.Log("Attack missed");
                }
            }
            if (Hdiff > Pstats.Health)
            {
                if (Random.Range(1, 11) > 6)
                {
                    Attack(EH, Pstats.Dmg);
                }
                else
                {
                    Debug.Log("Attack missed");
                }
            }
            if (Hdiff == Pstats.Health)
            {
                if (Random.Range(1, 11) > 4)
                {
                    Attack(EH, Pstats.Dmg);
                }
                else
                {
                    Debug.Log("Attack missed");
                }
            }

        }

    }
    public void Attack (int health, int pdmg)
    {
        int count = 0;
        float expreq = 0;
        int dmg = Random.Range(pdmg - 5, pdmg + 6);
        while (health > 0)
        {
            health -= pdmg;
            count++;
        }
        Debug.Log("Player defeats the enemy in " + count + " hits.");
        //this is in order to count the no. of hits player takes to defeat the enemy.
        switch (count)//Calculate the players exp for each enemyt defeated.
        {
            case 3:
                Pstats.exp += 30;
                Debug.Log("Player's Exp : " + Pstats.exp);
                break;
            case 4:
                Pstats.exp += 25;
                Debug.Log("Player's Exp : " + Pstats.exp);
                break;
            case 5:
                Pstats.exp += 20;
                Debug.Log("Player's Exp : " + Pstats.exp);
                break;
            case 6:
                Pstats.exp += 15;
                Debug.Log("Player's Exp : " + Pstats.exp);
                break;
            case 7:
                Pstats.exp += 10;
                Debug.Log("Player's Exp : " + Pstats.exp);
                break;
            case 8:
                Pstats.exp += 5;
                Debug.Log("Player's Exp : " + Pstats.exp);
                break;
            case 9:
                Pstats.exp += 2;
                Debug.Log("Player's Exp : " + Pstats.exp);
                break;
        }
        //exp calculator as per the provided exp curve in google doc.
        expreq = Mathf.Pow(Pstats.level + 3, 3) + 100;
        if(Pstats.exp > expreq)
        {
            Pstats.level += 1;
            Debug.Log("player leveled up to " + Pstats.level);
        }
        
        
        
        
        
    }
    public void Levelup()
    {
        //LEvels up the player and sends him to the attribute function
    }
    public void Attribute()
    {
        //Decides the picked attribute by player and updates it.
    }
    public void movement()
    {
        //decides the playermovement controls in here.
        //This function will be called from update incase player wants to make any movement.
    }
    public void Eattack()
    {
        //Attacks coming from the enemy's side.
    }
    public void poweups()
    {
        //These are player power ups which are yet to be decided on how to use them.
        //These would be enums and will have specific values.
    }

}

