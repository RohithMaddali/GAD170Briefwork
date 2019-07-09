using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //lists for enemies 
    public List<GameObject> EnemyList;
    //lists for enemies to spawn from
    public List<GameObject> EnemySpawnList;
    //current enemy the player would fight.
    public GameObject EnemyToFight;
    public bool doBattle = true;
    public int nooFEnemies;
    private int count = 0;
    int defeatCount = 0;

    //check the player's state.
    public enum GameState
    {
        InCombat,
        NotInCombat,
    }
    public GameState gameState;

    //used to switch turns from player to enemy and vice versa
    public enum CombatState
    {
        Playerturn,
        Enemyturn,
        Victory,
        Loss
    }
    public CombatState combatState;
    //objects to store player's and enemy's stats.
    public GameObject playerobj;
    

    void Start()
    {
        for (int i = 0; i < nooFEnemies; i++)
        {
            GameObject SpawnedEnemy = Instantiate(EnemyList[Random.Range(0, EnemyList.Count)], transform);
            EnemySpawnList.Add(SpawnedEnemy);
        }
        SetNewEnemyToFight();
        

    }

    void Update()
    {
        if (doBattle == true)
        {
            StartCoroutine(Battlego());
            doBattle = false;
        }
        
    }

    void SetNewEnemyToFight()
        {
            EnemyToFight = EnemySpawnList[Random.Range(0, EnemySpawnList.Count)];
            //StartCoroutine(Battlego());
    }

 
    void CheckCombatState()
    {
        //use switch to check cases for player turn and enemies turn.
        
        switch (combatState)
        {
            case CombatState.Playerturn:
                //if( Input.GetKeyDown(KeyCode.Space))
                // {
                //player attacks
                int Hdiff = EnemyToFight.GetComponent<Stats>().health - playerobj.GetComponent<Stats>().health;
                if (Hdiff < 50)
                {
                    if (Random.Range(1, 11) > 2)
                    {
                        BattleRound(playerobj, EnemyToFight);
                        combatState = CombatState.Enemyturn;
                        //combatState = CombatState.Enemyturn;
                        //player gains exp if enemy is defeated.
                        if (EnemyToFight.GetComponent<Stats>().isDefeated)
                        {
                            RemoveEnemy(EnemyToFight);
                            playerobj.GetComponent<Stats>().TotalExp += playerobj.GetComponent<Stats>().expGained;
                        }
                        //player levels up if enough exp is gained
                        playerobj.GetComponent<Stats>().reqExp = (int)Mathf.Pow(playerobj.GetComponent<Stats>().level + 3, 3) + 100;
                        if (playerobj.GetComponent<Stats>().TotalExp > playerobj.GetComponent<Stats>().reqExp)
                        {
                            //PLAYER LEVELS UP
                            playerobj.GetComponent<Stats>().level += 1;
                            playerobj.GetComponent<Stats>().health += 10;
                            Debug.Log("^LEVEL UP^");
                            SkillSelect();
                            //enemy levels up if player leveld up
                            //checks if enemy and player on same level and based on that increases the enemy's health.
                            //also increases enemy's level.
                            if (EnemyToFight.GetComponent<Stats>().Enemylvl < playerobj.GetComponent<Stats>().level)
                            {
                                playerobj.GetComponent<Stats>().Enemylvl += 1;
                                EnemyToFight.GetComponent<Stats>().health = (EnemyToFight.GetComponent<Stats>().health + 10);
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("Attack missed");
                        combatState = CombatState.Enemyturn;
                    }
                }
                else if (Hdiff > 50)
                {
                    if (Random.Range(1, 11) > 6)
                    {
                        BattleRound(playerobj, EnemyToFight);
                        combatState = CombatState.Enemyturn;
                        //combatState = CombatState.Enemyturn;
                        //player gains exp if enemy is defeated.
                        if (EnemyToFight.GetComponent<Stats>().isDefeated)
                        {
                            RemoveEnemy(EnemyToFight);
                            playerobj.GetComponent<Stats>().TotalExp += playerobj.GetComponent<Stats>().expGained;
                        }
                        //player levels up if enough exp is gained
                        playerobj.GetComponent<Stats>().reqExp = (int)Mathf.Pow(playerobj.GetComponent<Stats>().level + 3, 3) + 100;
                        if (playerobj.GetComponent<Stats>().TotalExp > playerobj.GetComponent<Stats>().reqExp)
                        {
                            //PLAYER LEVELS UP
                            playerobj.GetComponent<Stats>().level += 1;
                            playerobj.GetComponent<Stats>().health += 10;
                            Debug.Log("^LEVEL UP^");
                            SkillSelect();
                            //enemy levels up if player leveld up
                            //checks if enemy and player on same level and based on that increases the enemy's health.
                            //also increases enemy's level.
                            if (EnemyToFight.GetComponent<Stats>().Enemylvl < playerobj.GetComponent<Stats>().level)
                            {
                                playerobj.GetComponent<Stats>().Enemylvl += 1;
                                EnemyToFight.GetComponent<Stats>().health = (EnemyToFight.GetComponent<Stats>().health + 10);
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("Attack missed");
                        combatState = CombatState.Enemyturn;
                    }
                }
                
                //player eneds turn.
                // }
                break;
            case CombatState.Enemyturn:       
                //enemy attacks.
                BattleRound(EnemyToFight, playerobj);
                //player loses if enemy defeats the player.
                if(playerobj.GetComponent<Stats>().isDefeated)
                {
                    combatState = CombatState.Loss;
                }
                //enemy's turn ends.
                combatState = CombatState.Playerturn;
                break;
            case CombatState.Victory:
                //player wins if all the required numbe rof enemies are defeated.
                Debug.Log("No of enemies defeated: " + defeatCount);
                doBattle = false;
                //Display victory message.
                break;
            case CombatState.Loss:
                //player loses if his health reaches 0.
                Debug.Log("Game Over!!!");
                //display game over message.
                break;
        }
    }
    
    //remove an enemy if dead
    public void RemoveEnemy(GameObject EnemyToRemove)
    {
        //count the no of enemies defeated
        defeatCount += 1;
        //calculate the player's increase in exp.
        playerobj.GetComponent<Stats>().expGained = (count * 2) + 60;
        EnemySpawnList.Remove(EnemyToFight);
        Destroy(EnemyToFight);
        count = 0;
        if (EnemySpawnList.Count == 0)
        {
            Debug.Log("You win!!");
            combatState = CombatState.Victory;
        }
        else
        {

            
            SetNewEnemyToFight();
            
        }
    }

    //choose skills upon level up.
    void SkillSelect()
    {
        Debug.Log("Select the following"); 
        Debug.Log("1. Health - H");
        Debug.Log("2. Attack - A");
        Debug.Log("3. Defense - D");
        //if (Input.GetKeyDown(KeyCode.A))
        //{
            playerobj.GetComponent<Stats>().attack += 3;
            Debug.Log("Player's Attack increased to " + playerobj.GetComponent<Stats>().attack);
        //}
       // else if (Input.GetKeyDown(KeyCode.H))
        //{
            playerobj.GetComponent<Stats>().health += 10;
            Debug.Log("Player's Health increased to " + playerobj.GetComponent<Stats>().health);
        //}
       // else if (Input.GetKeyDown(KeyCode.D))
        //{
            playerobj.GetComponent<Stats>().defense += 1;
            Debug.Log("Player's Defense increased to " + playerobj.GetComponent<Stats>().defense);
        //}
    }

    void BattleRound(GameObject attacker, GameObject defender)
    {
        if(attacker == playerobj)
        {
            defender.GetComponent<Stats>().Attacked(attacker.GetComponent<Stats>().attack, Stats.StatusEffect.none);
            count++;
            Debug.Log("PLAYER ATTACKS ENEMY");
            if (EnemyToFight.GetComponent<Stats>().isDefeated)
            {
                Debug.Log("Enemy hp : " + " 0 ");
            }
            else
            {
                Debug.Log("Enemy hp : " + EnemyToFight.GetComponent<Stats>().health);
            }
        }
        if(attacker == EnemyToFight)
        {
            defender.GetComponent<Stats>().Attacked(attacker.GetComponent<Stats>().attack, Stats.StatusEffect.none);
            Debug.Log("ENEMY ATTACKS PLAYER");
            if (EnemyToFight.GetComponent<Stats>().isDefeated)
            {
                Debug.Log("Player hp : " + " 0 ");
            }
            else
            {
                Debug.Log("Player hp : " + playerobj.GetComponent<Stats>().health);
            }
        }
    }
    IEnumerator Battlego()
    {
        
        yield return new WaitForSeconds(3f);
        CheckCombatState();
        doBattle = true;
        
        
    }


}


