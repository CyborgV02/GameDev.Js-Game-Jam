using UnityEngine;

public class DischargeAbility : Ability
{
    private const string MinigameName = "Discharge";

    public DischargeAbility(string name, int chargeLevel, bool unlocked = false, bool needsTarget = false, bool isOffensive = false)
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
