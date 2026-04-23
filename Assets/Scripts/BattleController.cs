#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    private static Battle? currentBattle;
    public static Action<BattleStartPayload>? OnBattleStart;
    public static Action<Character, Character, InventoryItem>? UseItemAction;
    public static Action<Character, Ability, Character?, Enemy?>? UseAbilityAction;
    public static Action<Character, Enemy>? UseAttackAction;
    public static Action? OnBattleEnd;
    public static Action? OnMinigameStarted;
    public static Action? OnMinigameEnded;
    public static Battle? CurrentBattle {get => currentBattle;}
    public Queue<BattleCommands<Character>> commandQueue = new Queue<BattleCommands<Character>>();
    public List<GameObject> allowedMinigames = new List<GameObject>();
    private IMinigame? pendingMinigame;
    public static BattleController Instance;
    void Awake()
    {
        Instance = this;
        OnBattleStart += InitBattle;
        UseItemAction += UseItem;
        UseAbilityAction += UseAbility;
        UseAttackAction += UseAttack;
    }

    public void RequestBattleStart(Character[] allies, Enemy[] enemies)
    {
        InitBattle(new BattleStartPayload(BattleStartPayload.PayloadType.EntityReference, allies, enemies));
    }

    private void InitBattle(BattleStartPayload payload)
    {
        Debug.Log("Initializing battle with payload: " + payload);
        if (payload.payloadType == BattleStartPayload.PayloadType.EntityReference && payload.allies != null && payload.enemies != null)
        {
            Debug.Log("Initializing battle with entity references");
            pendingMinigame = null;
            foreach (var ally in payload.allies)
            {
                if (ally.GetType() == typeof(MainCharacter))
                {
                    currentBattle = new Battle(ally, payload.allies, payload.enemies);
                    currentBattle.StartBattle();
                    OnBattleStart?.Invoke(new BattleStartPayload(BattleStartPayload.PayloadType.BattleData, null, null, currentBattle));
                    break;
                }
            }
        }
    }

    private void UseItem(Character character, Character target, InventoryItem item) // TODO: Define item object
    {
        // TODO: Implement item usage logic
        commandQueue.Enqueue(new BattleCommands<Character>(character, CommandType.UseItem).Set(new BattleCommandsPayload(target, null, item, null))); // Placeholder for item command
    }

    private void UseAbility(Character character, Ability ability, Character? target = null, Enemy? enemyTarget = null)
    {
        if (currentBattle == null)
        {
            Debug.LogError("No active battle is available!");
            return;
        }

        if (currentBattle.ChargeLevel < ability.ChargeLevel && character.Abilities.Contains(ability))
        {
            Debug.Log($"Not enough charge to use {ability.Name}!");
            return;
        }

        if (ability.NeedsTarget && target == null && enemyTarget == null)
        {
            Debug.Log($"Ability {ability.Name} requires a target!");
            return;
        }

        if (!ability.NeedsTarget)
        {
            commandQueue.Enqueue(new BattleCommands<Character>(character, CommandType.UseAbilityNoTarget).Set(new BattleCommandsPayload(null, null, null, ability)));
            return;
        }

        if (enemyTarget != null)
        {
            commandQueue.Enqueue(new BattleCommands<Character>(character, CommandType.UseAbilityOffensive).Set(new BattleCommandsPayload(null, enemyTarget, null, ability)));
        }
        else if (target != null)
        {
            commandQueue.Enqueue(new BattleCommands<Character>(character, CommandType.UseAbilitySupportive).Set(new BattleCommandsPayload(target, null, null, ability)));
        }
    }

    private void UseAttack(Character character, Enemy enemyTarget)
    {
        commandQueue.Enqueue(new BattleCommands<Character>(character, CommandType.Attack).Set(new BattleCommandsPayload(null, enemyTarget, null, null)));
    }

    private void CommitCommands()
    {
        while (commandQueue.Count > 0)
        {
            var command = commandQueue.Dequeue();
            switch (command.CommandType)
            {
                case CommandType.Attack:
                    if (command.EnemyTarget != null)
                    {
                        command.EnemyTarget.TakeDamage(command.Character.AttackDamage * command.Character.Level / 4);
                    }
                    break;
                case CommandType.UseItem:
                    if (command.ItemTarget != null && command.Target != null)
                    {
                        command.ItemTarget.Use(command.Target);
                    }
                    break;
                case CommandType.UseAbilityNoTarget:
                    if (command.SelectedAbility != null)
                    {
                        command.SelectedAbility.Use(currentBattle);
                    }
                    break;
                case CommandType.UseAbilityOffensive:
                    if (command.SelectedAbility != null && command.EnemyTarget != null)
                    {
                        command.SelectedAbility.Use(currentBattle);
                    }
                    break;
                case CommandType.UseAbilitySupportive:
                    if (command.SelectedAbility != null && command.Target != null)
                    {
                        command.SelectedAbility.Use(currentBattle);
                    }
                    break;
            }
        }

        if (pendingMinigame != null)
        {
            pendingMinigame.StartMinigame();
            pendingMinigame = null;
            OnMinigameStarted?.Invoke();
        }
    }

    public void SubmitSelections(BattleSelectionBatch selectionBatch)
    {
        if (currentBattle == null)
        {
            Debug.LogError("No active battle to submit selections for!");
            return;
        }

        foreach (var selection in selectionBatch.selections)
        {
            if (!selection.IsComplete)
            {
                Debug.LogWarning("Skipping incomplete battle selection.");
                continue;
            }

            switch (selection.selectionType)
            {
                case BattleSelectionType.Attack:
                    if (selection.actor != null && selection.selectedEnemyTarget != null)
                    {
                        UseAttack(selection.actor, selection.selectedEnemyTarget);
                    }
                    break;
                case BattleSelectionType.Hack:
                    if (selection.actor != null && selection.selectedAbility != null)
                    {
                        UseAbility(selection.actor, selection.selectedAbility, selection.selectedAllyTarget, selection.selectedEnemyTarget);
                    }
                    break;
                case BattleSelectionType.Item:
                    if (selection.actor != null && selection.selectedItem != null && selection.selectedAllyTarget != null)
                    {
                        UseItem(selection.actor, selection.selectedAllyTarget, selection.selectedItem);
                    }
                    break;
                case BattleSelectionType.Boom:
                    if (selection.actor != null && selection.selectedEnemyTarget != null)
                    {
                        UseAttack(selection.actor, selection.selectedEnemyTarget);
                    }
                    break;
            }
        }

        CommitCommands();
    }
    
    internal void InstantiateMinigame(string minigameName)
    {
        if (currentBattle == null)
        {
            Debug.LogError("No active battle to instantiate minigame for!");
            return;
        }

        if (MinigameFactory.Instance == null)
        {
            Debug.LogError("MinigameFactory.Instance is null!");
            return;
        }

        IMinigame? minigameInstance = MinigameFactory.Instance.CreateMinigame(minigameName);
        if (minigameInstance == null)
        {
            Debug.LogError($"Failed to create minigame instance for {minigameName}!");
            return;
        }

        if (pendingMinigame != null)
        {
            Debug.LogWarning($"A minigame is already pending start. Ignoring additional minigame request: {minigameName}");
            return;
        }

        pendingMinigame = minigameInstance;
    }

    public void NotifyMinigameEnded()
    {
        OnMinigameEnded?.Invoke();
    }
}