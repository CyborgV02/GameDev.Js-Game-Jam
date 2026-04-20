using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : Character
{
    private static Ability[] defaultAbilities = new Ability[]
    {
        
    };
    public MainCharacter() : base("Tyr", 100, 1, 0, defaultAbilities) { }
    
}
