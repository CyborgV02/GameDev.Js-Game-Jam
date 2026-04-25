using UnityEngine;

public class HellTwistMinigame : MonoBehaviour, IMinigame
{
    [SerializeField] private float _duration = 8f;
    [SerializeField] private int _allyDamage = 30;
    [SerializeField] private int _enemyDamage = 30;
    [SerializeField] private string _name = "Hell Twist";
    [SerializeField] private int requiredDodges = 6;

    private bool isMinigameActive = false;
    private float remainingTime;
    private float dodgeWindow = 0.5f;
    private float dodgeTimer;
    private TwistPattern twistPattern;

    public string minigameName => _name;
    public float duration => _duration;
    public int allyDamage => _allyDamage;
    public int enemyDamage => _enemyDamage;

    void OnEnable()
    {
        InputController.OnMove += HandleInput;
    }

    void OnDisable()
    {
        InputController.OnMove -= HandleInput;
    }

    void Update()
    {
        UpdateMinigame(Time.deltaTime);
    }

    public void StartMinigame()
    {
        remainingTime = _duration;
        dodgeTimer = dodgeWindow;
        twistPattern = new TwistPattern(requiredDodges);
        isMinigameActive = true;
    }

    public void EndMinigame()
    {
        isMinigameActive = false;
        if (BattleController.Instance != null)
        {
            BattleController.Instance.NotifyMinigameEnded();
        }
        Destroy(gameObject);
    }

    public void UpdateMinigame(float deltaTime)
    {
        if (!isMinigameActive)
        {
            return;
        }

        if (twistPattern == null)
        {
            twistPattern = new TwistPattern(requiredDodges);
        }

        remainingTime -= deltaTime;
        dodgeTimer -= deltaTime;

        if (dodgeTimer <= 0f)
        {
            dodgeTimer = dodgeWindow;
            twistPattern.StepPattern();
        }

        if (twistPattern.IsComplete())
        {
            DamageEnemies(BattleController.CurrentBattle?.enemies);
            EndMinigame();
            return;
        }

        if (remainingTime > 0f)
        {
            return;
        }

        DamageAllies(BattleController.CurrentBattle?.allies);

        EndMinigame();
    }

    public void HandleInput(Vector2 input)
    {
        if (!isMinigameActive)
        {
            return;
        }

        if (input.sqrMagnitude <= 0.25f)
        {
            return;
        }

        if (twistPattern == null)
        {
            twistPattern = new TwistPattern(requiredDodges);
        }

        if (twistPattern.RegisterDodge(input))
        {
            dodgeTimer = dodgeWindow;
        }
    }

    public void DamageAllies(Character[] allies)
    {
        if (!isMinigameActive || allies == null)
        {
            return;
        }

        foreach (Character ally in allies)
        {
            ally.TakeDamage(_allyDamage);
        }
    }

    public void DamageEnemies(Enemy[] enemies)
    {
        if (!isMinigameActive || enemies == null)
        {
            return;
        }

        foreach (Enemy enemy in enemies)
        {
            enemy.TakeDamage(_enemyDamage);
        }
    }
}
