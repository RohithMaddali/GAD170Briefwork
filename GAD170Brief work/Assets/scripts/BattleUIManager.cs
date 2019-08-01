using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    private Button Attackbutton;
    private Button Attack;
    private Button Health;
    private Button Defense;

    public Image phealthBarFill;
    public Image ehealthBarFill;

    public BattleManager bManager;

    public event System.Action CallAttack;
    public event System.Action CallDefense;
    public event System.Action CallHealth;
    public event System.Action CallAttackButton;

    public Text[] combatLogLines;
    public List<string> combatLog;

    void Awake()
    {
        bManager = GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManager>();
        bManager.UpdateHealth += UpdateHealthBar;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DebugLogTest());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthBar(bool isplayer, float health)
    {
        if(isplayer)
        {
            phealthBarFill.fillAmount = health;
        }
        else
        {
            ehealthBarFill.fillAmount = health;
        }
    }

    public void CallAttackButtonEvent()
    {
        Debug.Log("Attacked");
        CallAttackButton();
    }

    public void CallAttackEvent()
    {
        Debug.Log("Health increased");
        CallAttack();
    }

    public void CallDefendEvent()
    {
        Debug.Log("defense increase");
        CallDefense();
    }

    public void CallHealthEvent()
    {
        Debug.Log("Health increased");
        CallHealth();
    }
    //Function for updating text
    public void UpdateCombatLog()
    {
        /*string p = "Player Attacks Enemy";
        string e = "Enemy Attacks Player";
        combatLog.Insert(0, p);
        combatLog.Insert(1, e);*/

        combatLog.Insert(0, " Player Attacks Enemy________");


        if (combatLog.Count > combatLogLines.Length)
        {
            combatLog.RemoveAt(combatLog.Count - 1);
        }

        for(int i = 0; i < combatLog.Count; i++)
        {
           if(bManager.GetComponent<BattleManager>().combatState == BattleManager.CombatState.Playerturn)
            {
                combatLogLines[i].text = combatLog[i].Insert(i, "Player Attacks Enemy___________");
            }
           else if(bManager.GetComponent<BattleManager>().combatState == BattleManager.CombatState.Enemyturn)
            {
                combatLogLines[i].text = combatLog[i].Insert(i, "Enemy Attacks Player___________");
            }
        }
        StartCoroutine(DebugLogTest());
    }

    IEnumerator DebugLogTest()
    {
        int randomNumber = Random.Range(1, 1000);

        yield return new WaitForSeconds(3f);
        UpdateCombatLog();
    }
}
