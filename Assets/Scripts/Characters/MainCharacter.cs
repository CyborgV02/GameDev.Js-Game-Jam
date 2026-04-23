using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : Character
{
    private static Ability[] defaultAbilities = new Ability[]
    {
       new NodeTransferAbility("Node Transfer", 1, true, true, true)
    };
    public MainCharacter(Sprite[] sprites) : base("Tyr", 100, 1, 0, defaultAbilities)
    {
        sprite = sprites;
    }
    
}
