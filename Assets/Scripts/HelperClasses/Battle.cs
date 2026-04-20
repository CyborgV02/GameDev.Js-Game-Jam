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

    public Battle(Character player, Character[] allies, Enemy[] enemies)
    {
        this.player = player;
        this.allies = allies;
        this.enemies = enemies;
    }

    public void StartBattle()
    {
        // TODO: Implement battle start logic (e.g., display UI, initialize variables)
        state = BattleState.PlayerTurn;
    }

    public void EndBattle(BattleState result)
    {
        // TODO: Implement battle end logic (e.g., display victory/defeat screen, reward player)
        state = result;
    }

}
