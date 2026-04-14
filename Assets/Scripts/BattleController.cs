using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    private Battle currentBattle;
    public Action<Character[], Enemy[]> OnBattleStart;
    public Action<BattleState> OnBattleEnd;

    void Awake()
    {
        InputController.OnMove += HandleMove;
        InputController.OnActionZ += HandleActionZ;
        InputController.OnActionX += HandleActionX;
        InputController.OnActionC += HandleActionC;
        OnBattleStart += InitBattle;
    }
    
    void InitBattle(Character[] allies, Enemy[] enemies)
    {
        foreach (var ally in allies)
        {
            if (ally.GetType() == typeof(MainCharacter))
            {
                currentBattle = new Battle(ally, allies, enemies);
                currentBattle.StartBattle();
                break;
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