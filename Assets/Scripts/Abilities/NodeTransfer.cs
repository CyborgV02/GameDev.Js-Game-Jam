using UnityEngine;
using UnityEngine.UIElements;

public class NodeTransferAbility : Ability
{
    private const string MinigameName = "NodeTransfer";

    public NodeTransferAbility(string name, int chargeLevel, bool unlocked = false, bool needsTarget = false, bool isOffensive = false) : base(name, chargeLevel, unlocked, needsTarget, isOffensive)
    {}

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