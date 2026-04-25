using UnityEngine;

public class HellTwistAbility : Ability
{
    private const string MinigameName = "HellTwist";

    public HellTwistAbility(string name, int chargeLevel, bool unlocked = false, bool needsTarget = false, bool isOffensive = false)
        : base(name, chargeLevel, unlocked, needsTarget, isOffensive)
    {
    }

    public override void Use(Battle battle)
    {
        if (!Unlocked)
        {
            return;
        }

        BattleController.Instance.InstantiateMinigame(MinigameName);
    }
}
