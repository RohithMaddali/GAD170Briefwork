using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildGrass : MonoBehaviour
{
    private GameObject gameManager;
    public bool isInGrass;
    public int chance;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        caught();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void caught()
    {
        int no = Random.Range(0, 51);
        if(no > 24 && isInGrass)
        {
            Debug.Log("enemy Encountered");
            gameManager.GetComponent<GameManager>().TravelToWorld(GameManager.Worlds.BattleScene);
        }
        StartCoroutine(CheckTimer());
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

    IEnumerator CheckTimer()
    {
        yield return new WaitForSeconds(2);
        caught();
    }
}
