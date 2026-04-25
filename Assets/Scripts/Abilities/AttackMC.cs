using UnityEngine;
using UnityEngine.UIElements;

public class AttackMC : Ability
{
    private const string MinigameName = "AttackMinigame";

    public AttackMC(string name, int chargeLevel, bool unlocked = true, bool needsTarget = false, bool isOffensive = false) : base(name, chargeLevel, unlocked, needsTarget, isOffensive)
    {

    }

    public override void Use(Battle battle)
    {
        if (!Unlocked)
        {
            return;
        }
        
        // Tell BattleController to instantiate the NodeTransfer minigame
        BattleController.Instance.InstantiateMinigame(MinigameName);
    }
}