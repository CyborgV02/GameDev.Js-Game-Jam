using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy
{
    public string Name { get; private set; }
    public int Hp { get; private set; }
    public bool IsDefeated => Hp <= 0;
    public bool IsBoomed { get; private set; }

    public float attackSpeed { get; private set; } // Used for attacks, minigame navigation, etc.
    public Dictionary<string, int> damage { get; private set; }
    private Dictionary<string, int> defaultDamage = new Dictionary<string, int>()
    {
        {"Basic", 10},
        {"Node", 20},
        {"Secure", 10},
        {"Discharge", 25},
        {"HellTwist", 30}
    };

    public Enemy(string name, int hp, float attackSpeed = 1f, Dictionary<string, int> damage = null)
    {
        this.Name = name;
        this.Hp = hp;
        this.damage = damage ?? defaultDamage;
        this.attackSpeed = attackSpeed;
    }

    public void TakeDamage(int damage)
    {
        Hp -= damage;
        if (Hp < 0) Hp = 0;
    }

    public void Boom()
    {
        IsBoomed = true;
    }
}
