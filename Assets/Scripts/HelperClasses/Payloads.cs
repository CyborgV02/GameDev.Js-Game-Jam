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
}