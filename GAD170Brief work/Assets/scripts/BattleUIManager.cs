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
    private Button HardReset;

    public Image phealthBarFill;
    public Image ehealthBarFill;

    public BattleManager bManager;

    public event System.Action CallAttack;
    public event System.Action CallDefense;
    public event System.Action CallHealth;
    public event System.Action CallAttackButton;
    public event System.Action CallHardReset;

    public Text[] combatLogLines;

    

    void Awake()
    {
        bManager = GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManager>();
        bManager.UpdateHealth += UpdateHealthBar;
    }

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(DebugLogTest());
    }
    

    // Update is called once per frame
    void Update()
    {
        if (bManager.GetComponent<BattleManager>().combatState == BattleManager.CombatState.Victory)
        {
            combatLogLines[0].text = "You Have Defeated the Enemy";
            combatLogLines[1].text = "You Have Defeated the Enemy";
        }
        else if (bManager.GetComponent<BattleManager>().combatState == BattleManager.CombatState.Loss)
        {
            combatLogLines[0].text = "You have lost all of your health";
            combatLogLines[1].text = "You have lost all of your health";
        }

        if(bManager.GetComponent<BattleManager>().combatState == BattleManager.CombatState.Enemyturn)
        {
            //Attackbutton.GetComponent<Button>().interactable = false;
           // Debug.Log("Button is disabled.");
        }
    }

    public void UpdateHealthBar(int isplayer, float health)
    {
        if(isplayer == 1)
        {
            phealthBarFill.fillAmount = health;
        }
        else if(isplayer == 2)
        {
            ehealthBarFill.fillAmount = health;
        }
    }

    public void CallAttackButtonEvent()
    {
        if (bManager.GetComponent<BattleManager>().combatState != BattleManager.CombatState.Victory && bManager.GetComponent<BattleManager>().combatState != BattleManager.CombatState.Loss)
        {
            combatLogLines[0].text = "Player Attacked the Enemy";
            StartCoroutine(Battlego());
            CallAttackButton();
        }
        
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
    public void CallHardResetEvent()
    {
        Debug.Log("Game Reset");
        CallHardReset();
    }

    IEnumerator Battlego()
    {
            yield return new WaitForSeconds(2f);
            combatLogLines[1].text = "Enemy Attacked Player";
            yield return new WaitForSeconds(1.5f);
            combatLogLines[0].text = "";
            combatLogLines[1].text = "";
        
    }

}
