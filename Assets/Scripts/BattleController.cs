#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    private Battle? currentBattle;
    public static Action<BattleStartPayload>? OnBattleStart;
    public static Action<Character, Character, InventoryItem>? UseItemAction;
    public static Action<Character, Ability, Character?, Enemy?>? UseMoveAction;
    public static Action<Character, Enemy>? UseAttackAction;
    public static Action? OnBattleEnd;
    public Queue<BattleCommands<Character>> commandQueue = new Queue<BattleCommands<Character>>();

    void Awake()
    {
        OnBattleStart += InitBattle;
        UseItemAction += UseItem;
        UseMoveAction += UseMove;
        UseAttackAction += UseAttack;
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

    private void UseItem(Character character, Character target, InventoryItem item) // TODO: Define item object
    {
        // TODO: Implement item usage logic
        commandQueue.Enqueue(new BattleCommands<Character>(character, CommandType.UseItem).Set(new BattleCommandsPayload(target, null, item, null))); // Placeholder for item command
    }

    private void UseMove(Character character, Ability ability, Character? target = null, Enemy? enemyTarget = null)
    {
        if (currentBattle?.ChargeLevel < ability.ChargeLevel && character.Abilities.Contains(ability))
        {
            Debug.Log($"Not enough charge to use {ability.Name}!");
            return;
        }

        if (ability.NeedsTarget && target == null && enemyTarget == null)
        {
            Debug.Log($"Ability {ability.Name} requires a target!");
            return;
        }
        else
        {
            commandQueue.Enqueue(new BattleCommands<Character>(character, CommandType.UseAbilityNoTarget).Set(new BattleCommandsPayload(null, null, null, ability))); // Placeholder for no-target ability command
        }

        if (target != null)
        {
            commandQueue.Enqueue(new BattleCommands<Character>(character, CommandType.UseAbilitySupportive).Set(new BattleCommandsPayload(target, null, null, ability)));
        }
        else if (enemyTarget != null)
        {
            commandQueue.Enqueue(new BattleCommands<Character>(character, CommandType.UseAbilityOffensive).Set(new BattleCommandsPayload(null, enemyTarget, null, ability)));
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
    }
}