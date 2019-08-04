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
    private CombatState combatstate;

    void Start()
    {
        //public bool doBattle = true;
        foreach (GameObject Enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {


            for (int i = 0; i < nooFEnemies; i++)
            {
                EnemySpawnList.Add(Enemy);
                GameObject SpawnedEnemy = Instantiate(EnemyList[Random.Range(0, EnemyList.Count)], transform);
                //EnemySpawnList.Add(SpawnedEnemy);
            }
        }
        SetNewEnemyToFight();
        //UpdateHealth(true, 0.5f); 
    }

    void Awake()
    {
        battleUIManager = GameObject.FindGameObjectWithTag("BattleUIManager");
        battleUIManager.GetComponent<BattleUIManager>().CallAttackButton += CheckCombatState;
        battleUIManager.GetComponent<BattleUIManager>().CallDefense += SkillSelectDefense;
        battleUIManager.GetComponent<BattleUIManager>().CallAttack += SkillSelectAttack;
        battleUIManager.GetComponent<BattleUIManager>().CallHealth += SkillSelectHealth; ;
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
            EnemyToFight = Instantiate(EnemySpawnList[Random.Range(0, EnemySpawnList.Count)], transform);
        StartCoroutine(Battlego());
    }

 
    void CheckCombatState()
    {
        //use switch to check cases for player turn and enemies turn.


        switch (combatState)
        {
            case CombatState.Playerturn:
                //if( Input.GetKeyDown(KeyCode.Space))
                 //{
                //player attacks
                float Hdiff = EnemyToFight.GetComponent<Stats>().health - playerobj.GetComponent<Stats>().health;
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
                            Debug.Log("^LEVEL UP^ Select  a Skill to proceed");
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
                            playerobj.GetComponent<Stats>().maxHP += 10;
                            Debug.Log("^LEVEL UP^");
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
                    }
                    else
                    {
                        Debug.Log("Attack missed");
                        combatState = CombatState.Enemyturn;
                    }
                //}
                
                //player eneds turn.
                 }
                break;
            case CombatState.Enemyturn:
                //enemy attacks.
                //StartCoroutine(Battlego());
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
            
            if (EnemyToFight.GetComponent<Stats>().isDefeated)
            {
                Debug.Log("Player hp : " + " 0 ");
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
        if(isInCombat)
        {
            if (combatstate == CombatState.Enemyturn)
            {
                CheckCombatState();
            }
        }
        yield return new WaitForSeconds(3f);
        //doBattle = true;
    }  


}


