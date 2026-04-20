public class DefaultBattleText : BattleText
{
    public DefaultBattleText(string[,] texts, string[] enemyName, string[] allyName) : base(texts, enemyName, allyName)
    {
    }
    public DefaultBattleText(string[] enemyName, string[] allyName) : base(enemyName, allyName)
    {
    }

    public DefaultBattleText() : base()
    {
    }
}