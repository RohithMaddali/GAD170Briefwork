using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<GameObject> EnemyList;
    public List<GameObject> EnemySpawnList;
    private bool doBattle = true;

    public enum GameState
    {
        notIncombat,
        Incombat
    }
    public GameState gameState;

    public enum CombatState
    {
        Playerturn,
        Enemyturn,
        Vicotry,
        Loss
    }
    public CombatState combatState;
    //Objects for combat
    public GameObject enemyobj;
    public GameObject playerobj;
    public int count = 1;

    void Start()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            EnemyList.Add(enemy);
        }

    }
    void Update()
    {
        if (doBattle)
        {
            StartCoroutine(Battlego());
            doBattle = false;
        }
    }
    public void RemoveEnemy(GameObject enemyToRemove)
    {
        EnemyList.Remove(enemyToRemove);
    }
    public void SpawnEnemy()
    {
        //enemy is swpaned form the provided list.
        Instantiate(EnemySpawnList[Random.Range(0, EnemySpawnList.Count)], transform);
    }
    public void CheckCombatState()
    {
        switch (combatState)
        {
            //playerturn
            case CombatState.Playerturn:
                //decision - attack
                //attack the enemy
                BattleRound(playerobj, enemyobj);
                //check if enemy is defeated
                if (enemyobj.GetComponent<Stats>().isDefeated)     
                     SpawnEnemy();                  
                //next case is most likely enemy's turn
                combatState = CombatState.Enemyturn;
                break;

            //enemy turn
            case CombatState.Enemyturn:
                //decision attack
                //attack the player
                BattleRound(enemyobj, playerobj);
                //check if player is defeated
                if (playerobj.GetComponent<Stats>().isDefeated)
                {
                    //set state to loss casue its game over
                    combatState = CombatState.Loss;
                    Debug.Log("Game Over");
                    break;
                }
                //Next case will be players turn
                combatState = CombatState.Playerturn;
                break;

            //victory
            case CombatState.Vicotry:
                Debug.Log("You win!!");
                break;
            //Tell the player they won
            //end game
            case CombatState.Loss:
                //loss 
                //tell the player they lost
                //restart the game.
                if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene("SampleScene");
                break;

        }
    }
        public void BattleRound(GameObject attacker, GameObject defender)
        {
        //will take an attacker and defender and then make them do combat
        if (attacker.name == "Capsule")
        {
            count += 1;
            attacker.GetComponent<Stats>().attack = Random.Range(45, 56);
            defender.GetComponent<Stats>().Attacked(attacker.GetComponent<Stats>().attack, Stats.StatusEffect.none);
            Debug.Log("Attacker: " + attacker.name + " | Defender: " + defender.name);
            Debug.Log(attacker.name +
                " Attacks " + defender.name +
                " For a total of " +
                (attacker.GetComponent<Stats>().attack - defender.GetComponent<Stats>().defense) +
                " Damage ");
            Debug.Log("The hit count is " + count);
        }
        else
        {
            defender.GetComponent<Stats>().Attacked(attacker.GetComponent<Stats>().attack, Stats.StatusEffect.none);
            Debug.Log("Attacker: " + attacker.name + " | Defender: " + defender.name);
            Debug.Log(attacker.name +
                " Attacks " + defender.name +
                " For a total of " +
                (attacker.GetComponent<Stats>().attack - defender.GetComponent<Stats>().defense) +
                " Damage ");
        }
        }
    IEnumerator Battlego()
    {
        CheckCombatState();
        yield return new WaitForSeconds(1f);
        doBattle = true;
    }

    }

