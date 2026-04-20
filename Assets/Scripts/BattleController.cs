#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    private Battle? currentBattle;
    public static Action<BattleStartPayload> OnBattleStart;
    public static Action<Character> UseItemAction;
    public static Action<Character, Ability> UseMoveAction;
    public static Action<Character> UseAttackAction;
    public static Action? OnBattleEnd;

    void Awake()
    {
        OnBattleStart += InitBattle;
        UseItemAction += UseItem;
        // UseMoveAction += UseMove;
        // UseAttackAction += UseAttack;
    }

    private void InitBattle(BattleStartPayload payload)
    {
        if (payload.payloadType == BattleStartPayload.PayloadType.BattleData && payload.allies != null && payload.enemies != null)
        {
            foreach (var ally in payload.allies)
            {
                if (ally.GetType() == typeof(MainCharacter))
                {
                    currentBattle = new Battle(ally, payload.allies, payload.enemies);
                    currentBattle.StartBattle();
                    break;
                }
            }
        }
    }

    private void UseItem(Character character /*, item object*/) // TODO: Define item object
    {
        // TODO: Implement item usage logic
    }

    private void UseMove(Character character, Ability ability) {
        
    }

}