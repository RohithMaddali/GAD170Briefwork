using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;


public class BattleManager : MonoBehaviour
{
    //lists for enemies 
    public List<GameObject> EnemyList;
    //lists for enemies to spawn from
    public List<GameObject> EnemySpawnList;
    //current enemy the player would fight.
    public GameObject EnemyToFight;
    bool doBattle = true;
    public bool isInCombat = true;
    public int nooFEnemies;
    private int count = 0;
    int defeatCount = 0;
    public Vector3 Spawnloc;
    private Button Attackbutton;
    public Vector3 position;

    private GameObject gameManager;
    

    public event System.Action<int, float> UpdateHealth;

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
    private GameObject battleUIManager;
    

    void Start()
    {
        //public bool doBattle = true;
        gameManager.GetComponent<GameManager>().LoadPlayerStuff(false);
        EnemySpawnList.Add(EnemyList[(Random.Range(0, EnemyList.Count))]);
        SetNewEnemyToFight();
        //UpdateHealth(true, 0.5f); 
    }

    void Awake()
    {
        //used for buttons in order to perfrom the following function on clicking the buttons.
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        battleUIManager = GameObject.FindGameObjectWithTag("BattleUIManager");
        battleUIManager.GetComponent<BattleUIManager>().CallAttackButton += CheckCombatState;
        battleUIManager.GetComponent<BattleUIManager>().CallDefense += SkillSelectDefense;
        battleUIManager.GetComponent<BattleUIManager>().CallAttack += SkillSelectAttack;
        battleUIManager.GetComponent<BattleUIManager>().CallHealth += SkillSelectHealth; 
    }

    void Update()
    {

        if (doBattle == true && combatState == CombatState.Enemyturn)
            
        {
            
            StartCoroutine(Battlego());
            doBattle = false;
        }
        
        
        
    }

   

    void SetNewEnemyToFight()
    {

        //spawning and setting the enemy to fight once player enters battle scene.
        EnemyToFight = EnemySpawnList[Random.Range(0, EnemySpawnList.Count)];
        Vector3 position = new Vector3(-9.13f, 3.94f);
        Instantiate(EnemyToFight, position, Quaternion.identity);
        
        //StartCoroutine(Battlego());
    }


    void CheckCombatState()
    {
        //use switch to check cases for player turn and enemies turn.


        switch (combatState)
        {
            case CombatState.Playerturn:
                //if( Input.GetKeyDown(KeyCode.Space))
                //check if player is dead
                if (playerobj.GetComponent<Stats>().health <= 0)
                {
                    combatState = CombatState.Loss;
                }
                else
                {
                    //{
                    //player attacks
                    playerobj.GetComponent<Stats>().reqExp = (int)Mathf.Pow(playerobj.GetComponent<Stats>().level + 3, 3) + 100;
                    if (playerobj.GetComponent<Stats>().expGained > playerobj.GetComponent<Stats>().reqExp)
                    {
                        //PLAYER LEVELS UP
                        playerobj.GetComponent<Stats>().level += 1;
                        playerobj.GetComponent<Stats>().health += 10;
                        playerobj.GetComponent<Stats>().maxHP += 10;
                        Debug.Log("^LEVEL UP^ select a skill");
                        //SkillSelect();
                        //enemy levels up if player leveld up
                        //checks if enemy and player on same level and based on that increases the enemy's health.
                        //also increases enemy's level.
                        if (EnemyToFight.GetComponent<Stats>().Enemylvl < playerobj.GetComponent<Stats>().level)
                        {
                            playerobj.GetComponent<Stats>().Enemylvl += 1;
                            EnemyToFight.GetComponent<Stats>().health = (EnemyToFight.GetComponent<Stats>().health + 10);
                        }
                    }
                    float Hdiff = EnemyToFight.GetComponent<Stats>().health - playerobj.GetComponent<Stats>().health;
                    if (Hdiff < 50)
                    {
                        if (Random.Range(1, 11) > 2)
                        {
                            BattleRound(playerobj, EnemyToFight);
                            //combatState = CombatState.Enemyturn;
                            //player gains exp if enemy is defeated.
                            if (EnemyToFight.GetComponent<Stats>().isDefeated)
                            {
                                RemoveEnemy(EnemyToFight);
                                
                            }
                            //player levels up if enough exp is gained
                            
                        }
                        else
                        {
                            Debug.Log("Attack missed");
                        }
                    }
                    else if (Hdiff > 50 && Hdiff < 70)
                    {
                        if (Random.Range(1, 11) > 6)
                        {
                            BattleRound(playerobj, EnemyToFight);
                            //combatState = CombatState.Enemyturn;
                            //player gains exp if enemy is defeated.
                            if (EnemyToFight.GetComponent<Stats>().isDefeated)
                            {
                                RemoveEnemy(EnemyToFight);
                            }
                            //player levels up if enough exp is gained
                            playerobj.GetComponent<Stats>().reqExp = (int)Mathf.Pow(playerobj.GetComponent<Stats>().level + 3, 3) + 100;
                            
                        }
                        else
                        {
                            Debug.Log("Attack missed");
                        }


                        //}

                        //player eneds turn.
                    }
                    else if (Hdiff > 70 && Hdiff < 100)
                    {
                        if (Random.Range(1, 11) > 6)
                        {
                            BattleRound(playerobj, EnemyToFight);
                            //combatState = CombatState.Enemyturn;
                            //player gains exp if enemy is defeated.
                            if (EnemyToFight.GetComponent<Stats>().isDefeated)
                            {
                                RemoveEnemy(EnemyToFight);
                            }
                            //player levels up if enough exp is gained
                            playerobj.GetComponent<Stats>().reqExp = (int)Mathf.Pow(playerobj.GetComponent<Stats>().level + 3, 3) + 100;

                        }
                        else
                        {
                            Debug.Log("Attack missed");
                        }


                        //}

                        //player eneds turn.
                    }

                    if (combatState != CombatState.Victory && combatState != CombatState.Loss)
                    {
                        combatState = CombatState.Enemyturn;
                        Debug.Log(combatState);
                        doBattle = true;
                    }
                }
                break;
            case CombatState.Enemyturn:
                //enemy attacks.
                //StartCoroutine(Battlego());
                if (playerobj.GetComponent<Stats>().isDefeated)
                {
                    combatState = CombatState.Loss;
                }
                else
                {
                    
                    BattleRound(EnemyToFight, playerobj);
                    //player loses if enemy defeats the player.
                    //enemy's turn ends.
                    combatState = CombatState.Playerturn;
                    Debug.Log("enemys turn ended");
                }
                
                break;
            case CombatState.Victory:
                //player wins if all the required numbe rof enemies are defeated.
                Debug.Log("No of enemies defeated: " + defeatCount);
                doBattle = false;
                isInCombat = false;
                gameManager.GetComponent<GameManager>().TravelToWorld(GameManager.Worlds.WorldView);
                //Display victory message.
                break;
            case CombatState.Loss:
                //player loses if his health reaches 0.
                Debug.Log("Game Over!!!");
                isInCombat = false;
                gameManager.GetComponent<GameManager>().TravelToWorld(GameManager.Worlds.WorldView);
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
        playerobj.GetComponent<Stats>().expGained += (count * 2) + 60;
        EnemySpawnList.Remove(EnemyToFight);
        Destroy(EnemyToFight);
        count = 0;
        if (EnemySpawnList.Count == 0)
        {
            Debug.Log("You win!!");
            combatState = CombatState.Victory;
            gameManager.GetComponent<GameManager>().TravelToWorld(GameManager.Worlds.WorldView);

        }
        else
        {

            
            SetNewEnemyToFight();
            
        }
    }

    //choose skills upon level up.
    void SkillSelectHealth()
    {

        playerobj.GetComponent<Stats>().health += 10;
        playerobj.GetComponent<Stats>().maxHP += 10;
        Debug.Log("Player's Health increased to " + playerobj.GetComponent<Stats>().health);
        float percentage = playerobj.GetComponent<Stats>().health / playerobj.GetComponent<Stats>().maxHP;
        UpdateHealth(1, percentage);
    }
    void SkillSelectAttack()
    {

        playerobj.GetComponent<Stats>().attack += 3;
        Debug.Log("Player's Attack increased to " + playerobj.GetComponent<Stats>().attack);
    }

    void SkillSelectDefense()
    {

         playerobj.GetComponent<Stats>().defense += 0.2f;
            Debug.Log("Player's Defense increased to " + playerobj.GetComponent<Stats>().defense);
    }
    
    

    void BattleRound(GameObject attacker, GameObject defender)
    {
        if(attacker == playerobj)
        {
            Debug.Log("PLAYER ATTACKS ENEMY");
            defender.GetComponent<Stats>().Attacked(attacker.GetComponent<Stats>().attack, Stats.StatusEffect.none);
            count++;
            
            if (EnemyToFight.GetComponent<Stats>().isDefeated)
            {
                Debug.Log("Enemy hp : " + " 0 ");
                
            }
            else
            {
                Debug.Log("Enemy hp : " + EnemyToFight.GetComponent<Stats>().health);
               
            }
            float percentage = EnemyToFight.GetComponent<Stats>().health / defender.GetComponent<Stats>().maxHP;
            UpdateHealth(2, percentage);
            //Debug.Log(percentage);
            

        }
        if(attacker == EnemyToFight)
        {
            Debug.Log("ENEMY ATTACKS PLAYER");
            defender.GetComponent<Stats>().Attacked(attacker.GetComponent<Stats>().attack, Stats.StatusEffect.none);
            
            if (playerobj.GetComponent<Stats>().isDefeated)
            {
                Debug.Log("Player hp : " + " 0 ");
                Debug.Log("You Lost");
                gameManager.GetComponent<GameManager>().TravelToWorld(GameManager.Worlds.WorldView);

            }
            else
            {
                Debug.Log("Player hp : " + playerobj.GetComponent<Stats>().health);
            }
            float percentage = playerobj.GetComponent<Stats>().health / defender.GetComponent<Stats>().maxHP;
            UpdateHealth(1, percentage);
            //Debug.Log(percentage);

        }
    }
    IEnumerator Battlego()
    {
        //if(isInCombat)
              
        yield return new WaitForSeconds(2f);
        CheckCombatState();
    }
}


