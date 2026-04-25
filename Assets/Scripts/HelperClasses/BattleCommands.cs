public enum CommandType
{
    Attack,
    Boom,
    UseItem,
    UseAbilityNoTarget,
    UseAbilityOffensive,
    UseAbilitySupportive
}

public class BattleCommands<T>
{
    private Ability selectedAbility;
    private T character;
    private Character target;
    private Enemy enemyTarget;
    private InventoryItem itemTarget; // TODO: Define item object
    private CommandType commandType;

    public Ability SelectedAbility { get { return selectedAbility; } }
    public T Character { get { return character; } }
    public Character Target { get { return target; } }
    public Enemy EnemyTarget { get { return enemyTarget; } }
    public InventoryItem ItemTarget { get { return itemTarget; } }
    public CommandType CommandType { get { return commandType; } }

    public BattleCommands(T character, CommandType commandType)
    {
        this.character = character;
        this.commandType = commandType;
    }

    public BattleCommands<T> Set(BattleCommandsPayload payload)
    {
        this.selectedAbility = payload.selectedAbility;
        this.target = payload.target;
        this.enemyTarget = payload.enemyTarget;
        this.itemTarget = payload.itemTarget;
        return this;
    }
}