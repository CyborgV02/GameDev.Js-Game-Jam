public abstract class BattleText
{
    public string[,] Texts { get; }
    protected string[] EnemyName { get; set; }
    protected string[] AllyName { get; set; }
    public int currentEnemyNameIndex { get; set; }
    public int currentAllyNameIndex { get; set; }
    public int currentMoveText { get; set; }
    public string CustomText { get; set; }

    public BattleText(string[,] texts, string[] enemyName, string[] allyName)
    {
        Texts = texts;
        EnemyName = enemyName;
        AllyName = allyName;
    }

    public BattleText(string[] enemyName, string[] allyName)
    {
        Texts = new string[,]
        {
            {$"{currentEnemyNameIndex} has approached!", "Prepare for battle!", ""},
            {$"{currentAllyNameIndex} has used {currentMoveText}", $"{currentAllyNameIndex} has used {currentMoveText}", ""},
            {"The robot metal's cracks slightly.", "It becomes angry.", ""},
            {"The robot's systems are failing.", "It's getting weaker.", ""},
            {"The robot's state is critical.", "Now is your chance!", ""},
            {"The robot has been defeated!", $"You have earned {CustomText}.", $"You have earned {CustomText}."}
        };
        EnemyName = enemyName;
        AllyName = allyName;
    }
    public BattleText()
    {
        Texts = new string[,]
        {
            {$"{currentEnemyNameIndex} has approached!", "Prepare for battle!", ""},
            {$"{currentAllyNameIndex} has used {currentMoveText}", $"{currentAllyNameIndex} has used {currentMoveText}", ""},
            {"The robot metal's cracks slightly.", "It becomes angry.", ""},
            {"The robot's systems are failing.", "It's getting weaker.", ""},
            {"The robot's state is critical.", "Now is your chance!", ""},
            {"The robot has been defeated!", $"You have earned {CustomText}.", $"You have earned {CustomText}."}
        };
        EnemyName = new string[0];
        AllyName = new string[0];
    }

    public string[] SetText(BattleTextPayload payload)
    {
        if (payload.currentEnemyNameIndex >= 0 && payload.currentEnemyNameIndex < EnemyName.Length)
        {
            currentEnemyNameIndex = payload.currentEnemyNameIndex;
        }
        if (payload.currentAllyNameIndex >= 0 && payload.currentAllyNameIndex < AllyName.Length)
        {
            currentAllyNameIndex = payload.currentAllyNameIndex;
        }
        if (payload.currentMoveText >= 0 && payload.currentMoveText < Texts.GetLength(0))
        {
            currentMoveText = payload.currentMoveText;
        }
        if (!string.IsNullOrEmpty(payload.customText))
        {
            CustomText = payload.customText;
        }
        return GetRow(Texts, currentMoveText);
    }

    private string[] GetRow(string[,] source, int rowIndex)
    {
        int columns = source.GetLength(1);
        string[] row = new string[columns];

        for (int column = 0; column < columns; column++)
        {
            row[column] = source[rowIndex, column];
        }

        return row;
    }
}