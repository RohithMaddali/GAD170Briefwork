using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public int health;
    public int attack;
    public int attspeed;
    public int defense;
    public int luck;
    public int level = 0;
    public int Enemylvl = 0;
    public int expGained;
    public int TotalExp;
    public int reqExp;

    public bool isDefeated;

    public enum StatusEffect
    {
        none,
        sleep,
        stun,
        poison
    }
    public enum Skills
    {
        Poison,
        Flame,
        Ice,
        Crit,
        DoubleAtt,
    }

    public StatusEffect myStatus;
    public StatusEffect attackEffect;
    public void Attacked(int DMG, StatusEffect incEffect)
    {
        health -= DMG - defense;
        myStatus = incEffect;
        if (health <= 0)
            isDefeated = true;
    }

}
