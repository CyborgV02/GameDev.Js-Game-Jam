using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum BattleState
{
    Start,
    PlayerTurn,
    EnemyTurn,
    Victory,
    Defeat
}

public class Battle
{
    public Character player;
    public Character[] allies;
    public Enemy[] enemies;
    public BattleState state = BattleState.Start;
    public BattleText battleText;
    private int chargeLevel = 0;

    public int ChargeLevel { get { return chargeLevel; } }

    public Battle(Character player, Character[] allies, Enemy[] enemies, BattleText battleText = null)
    {
        this.player = player;
        this.allies = allies;
        this.enemies = enemies;
        this.battleText = battleText ?? new DefaultBattleText();
    }

    public Battle(Character player, Character[] allies, Enemy[] enemies)
    {
        this.player = player;
        this.allies = allies;
        this.enemies = enemies;
        this.battleText = new DefaultBattleText();
    }

    void ChargeTurn()
    {
        if (chargeLevel >= 3)
            chargeLevel = 3; // Max charge level
        else
            chargeLevel++;
    }

    public void StartBattle()
    {
        // TODO: Implement battle start logic (e.g., display UI, initialize variables)
        state = BattleState.PlayerTurn;
        chargeLevel = 2;
    }

    public void EndBattle(BattleState result)
    {
        // TODO: Implement battle end logic (e.g., display victory/defeat screen, reward player)
        state = result;
    }

}
