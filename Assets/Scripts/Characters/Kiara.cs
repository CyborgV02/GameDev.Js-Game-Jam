using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiara : Character
{
    private static Ability[] defaultAbilities = new Ability[]
    {
         
    };
    public Kiara(Sprite[] sprites) : base("Kiara", 100, 1, 0, defaultAbilities)
    {
        sprite = sprites;
    }
}

    
