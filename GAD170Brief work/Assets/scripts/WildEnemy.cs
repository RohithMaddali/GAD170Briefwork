using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildEnemy : MonoBehaviour
{
    private GameObject gameManager;
    public bool isInGrass;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        EnemySpotted();
    }

    void EnemySpotted()
    {
        if(isInGrass)
        {
            //encounter! Load battle scene
            gameManager.GetComponent<GameManager>().TravelToWorld(GameManager.Worlds.BattleScene);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            isInGrass = true;
        }

    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            isInGrass = false;
        }
    }
}
