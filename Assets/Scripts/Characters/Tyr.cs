using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : Character
{
    private static Ability[] defaultAbilities = new Ability[]
    {
         new AttackMC("Bat Attack", 1, false, true, true),
         new NodeTransferAbility("Node Transfer", 1, true, true, true),
         new SecureLinksAbility("Secure Links", 1, true, false, false),
         new HellTwistAbility("Hell Twist", 1, true, false, true)
    };
    public Action OnDefeated; // Event triggered when the character is defeated
    public MainCharacter(Sprite[] sprites) : base("Tyr", 100, 1, 0, defaultAbilities)
    {
        sprite = sprites;
    }
    
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (Hp <= 0)
        {
            OnDefeated?.Invoke();
        }
    }
    
}
