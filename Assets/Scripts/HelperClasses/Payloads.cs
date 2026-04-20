#nullable enable
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTextPayload
{
    public int currentEnemyNameIndex;
    public int currentAllyNameIndex;
    public int currentMoveText;
    public string customText = "";

    public BattleTextPayload(int currentEnemyNameIndex = -1, int currentAllyNameIndex = -1, int currentMoveText = -1, string customText = "")
    {
        this.currentEnemyNameIndex = currentEnemyNameIndex;
        this.currentAllyNameIndex = currentAllyNameIndex;
        this.currentMoveText = currentMoveText;
        this.customText = customText;
    }
}

public class BattleStartPayload
{
    public enum PayloadType
    {
        EntityReference,
        BattleData
    }
    public PayloadType payloadType;
    public Character[]? allies;
    public Enemy[]? enemies;

    public Battle? battle;

    public BattleStartPayload(PayloadType payloadType, Character[]? allies = null, Enemy[]? enemies = null, Battle? battle = null)
    {
        this.payloadType = payloadType;
        this.allies = allies;
        this.enemies = enemies;
        this.battle = battle;
    }
}

public class BattleCommandsPayload
{
    public Character? target;
    public Enemy? enemyTarget;
    public InventoryItem? itemTarget;
    public Ability? selectedAbility;

    public BattleCommandsPayload(Character? target = null, Enemy? enemyTarget = null, InventoryItem? itemTarget = null, Ability? selectedAbility = null)
    {
        this.target = target;
        this.enemyTarget = enemyTarget;
        this.itemTarget = itemTarget;
        this.selectedAbility = selectedAbility;
    }
}