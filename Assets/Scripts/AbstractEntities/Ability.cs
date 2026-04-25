using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
    private string name;
    private int chargeLevel;
    private bool unlocked = false;
    private bool needsTarget = false;
    private bool isOffensive = false;

    public string Name { get => name; }
    public int ChargeLevel { get => chargeLevel; }
    public bool Unlocked { get => unlocked; }
    public bool NeedsTarget { get => needsTarget; }
    public bool IsOffensive { get => isOffensive; }

    public Ability(string name, int chargeLevel, bool unlocked = false, bool needsTarget = false, bool isOffensive = false)
    {
        this.name = name;
        this.chargeLevel = chargeLevel;
        this.unlocked = unlocked;
        this.needsTarget = needsTarget;
        this.isOffensive = isOffensive;
    }

    public void Unlock()
    {
        unlocked = true;
    }

    public virtual void Use(Battle battle)
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
