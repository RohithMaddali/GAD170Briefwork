﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public float health;
    public int attack;
    public int attspeed;
    public float defense;
    public int luck;
    public int level;
    public int Enemylvl;
    public int expGained;
    public int reqExp;
    public float maxHP;

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
