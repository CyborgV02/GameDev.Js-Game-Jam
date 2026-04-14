using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
    private string name;
    private int chargeLevel;
    private bool unlocked = false;

    public Ability(string name, int chargeLevel, bool unlocked = false)
    {
        this.name = name;
        this.chargeLevel = chargeLevel;
        this.unlocked = unlocked;
    }

    public virtual void Use()
    {
        // TODO: Implement ability logic here
        if (!unlocked)
        {
            Debug.Log($"Ability {name} is locked!");
            return;
        }
        Debug.Log($"Using ability: {name} with charge level: {chargeLevel}");
    }
}
