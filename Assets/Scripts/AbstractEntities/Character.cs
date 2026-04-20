using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class Character
{
    private string name = "Ally";
    private int hp = 100;
    private int currentHp = 100;
    private int level = 1;
    private int exp = 0;
    private int attackDamage = 10;
    private Sprite[] sprite;
    private Ability[] abilities;

    public string Name { get => name; }
    public int Hp { get => hp; }
    public int Level { get => level; }
    public int Exp { get => exp; }
    public int AttackDamage { get => attackDamage; }
    public Ability[] Abilities { get => abilities; }
    public Sprite[] Sprite { get => sprite; }
    public int CurrentHp { get => currentHp;}

    public Character(string name, int hp, int level, int exp, Ability[] abilities)
    {
        this.name = name;
        this.hp = hp;
        this.level = level;
        this.exp = exp;
        this.abilities = abilities;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp < 0) currentHp = 0;
    }

    public void Heal(int amount)
    {
        if (hp + amount > 100) hp = 100;
        else hp += amount;
    }

    public bool GainExp(int amount)
    {
        exp += amount;
        return CheckLevelUp();
    }

    private bool CheckLevelUp()
    {
        int expToLevelUp = level * 100; // 100 EXP per level
        if (exp >= expToLevelUp)
        {
            level++;
            exp -= expToLevelUp;
            return true;
            // Optionally increase stats or unlock abilities here
        }
        return false;
    }
    

}
