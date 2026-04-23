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

public enum BattleSelectionType
{
    Attack,
    Hack,
    Item,
    Boom
}

public class BattleActionSelection
{
    public Character? actor;
    public BattleSelectionType selectionType;
    public Ability? selectedAbility;
    public InventoryItem? selectedItem;
    public Character? selectedAllyTarget;
    public Enemy? selectedEnemyTarget;

    public bool IsComplete
    {
        get
        {
            switch (selectionType)
            {
                case BattleSelectionType.Attack:
                case BattleSelectionType.Boom:
                    return actor != null && selectedEnemyTarget != null;
                case BattleSelectionType.Hack:
                    if (actor == null || selectedAbility == null)
                    {
                        return false;
                    }

                    if (selectedAbility.NeedsTarget)
                    {
                        return selectedAbility.IsOffensive ? selectedEnemyTarget != null : selectedAllyTarget != null;
                    }

                    return true;
                case BattleSelectionType.Item:
                    return actor != null && selectedItem != null && selectedAllyTarget != null;
                default:
                    return false;
            }
        }
    }
}

public class BattleSelectionBatch
{
    public List<BattleActionSelection> selections = new List<BattleActionSelection>();
}