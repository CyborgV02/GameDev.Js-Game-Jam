#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    private Battle? currentBattle;
    public static Action<BattleStartPayload> OnBattleStart;
    public static Action? OnBattleEnd;

    void Awake()
    {
        InputController.OnMove += HandleMove;
        InputController.OnActionZ += HandleActionZ;
        InputController.OnActionX += HandleActionX;
        InputController.OnActionC += HandleActionC;
        OnBattleStart += InitBattle;
    }

    void InitBattle(BattleStartPayload payload)
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

    private void HandleMove(Vector2 direction)
    {

    }
    private void HandleActionZ()
    {

    }

    private void HandleActionX()
    {

    }

    private void HandleActionC()
    {

    }

    private void UseItem(Character character /*, item object*/)
    {
        // Implement item usage logic
    }

}