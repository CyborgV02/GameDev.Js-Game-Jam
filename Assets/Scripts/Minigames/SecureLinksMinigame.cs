using UnityEngine;

public class SecureLinksMinigame : MonoBehaviour, IMinigame
{
    [SerializeField] private float _duration = 12f;
    [SerializeField] private int _allyDamage = 10;
    [SerializeField] private int _enemyDamage = 10;
    [SerializeField] private string _name = "Secure Links";
    [SerializeField] private int requiredLinks = 5;
    [SerializeField] private GameObject[] armPrefabs;
    [SerializeField] private GameObject indicatorPrefab;
    private Arm[] arms;
    private DirectionalArms directionalArms;
    private DirectionalArms.Direction indicatorDirection = DirectionalArms.Direction.Left;
    private IndicatorState currentIndicatorState = IndicatorState.Neutral;

    private enum IndicatorState
    {
        Neutral,
        Success,
        Failure,
    }

    private bool isMinigameActive = true; // TEMPORARY, set to true for testing
    private float remainingTime = 12f; // Fallback duration for testing

    public string minigameName => _name;
    public float duration => _duration;
    public int allyDamage => _allyDamage;
    public int enemyDamage => _enemyDamage;

    void Awake()
    {
        arms = new Arm[4];
        for (int i = 0; i < 4; i++)
        {
            arms[i] = new Arm(armPrefabs[i], armPrefabs[i].GetComponent<Animator>(), armPrefabs[i].GetComponent<SpriteRenderer>());
            arms[i].OnDamaged += () => DamageAllies(BattleController.CurrentBattle?.allies);
        }
        directionalArms = new DirectionalArms(ref arms);
    }
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
        isMinigameActive = true;
    }

    void HandleTimer(float deltaTime)
    {
        remainingTime -= deltaTime;
        if (remainingTime <= 0f)
        {
            if (requiredLinks > 0)
                DamageAllies(BattleController.CurrentBattle?.allies);

            EndMinigame();
        }
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
        directionalArms.Update();
        HandleTimer(deltaTime);
        if (directionalArms.InputResetTimer <= 0f && currentIndicatorState != IndicatorState.Neutral)
        {
            indicatorPrefab.GetComponent<SpriteRenderer>().color = Color.white; // Indicate reset state
            currentIndicatorState = IndicatorState.Neutral;
        }
    }

    void AdjustIndicator(Vector2 input)
    {
        if (input.y > 0.5f)
        {
            indicatorDirection = DirectionalArms.Direction.Up;
            indicatorPrefab.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        }
        else if (input.y < -0.5f)
        {
            indicatorDirection = DirectionalArms.Direction.Down;
            indicatorPrefab.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
        else if (input.x > 0.5f)
        {
            indicatorDirection = DirectionalArms.Direction.Right;
            indicatorPrefab.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
        else if (input.x < -0.5f)
        {
            indicatorDirection = DirectionalArms.Direction.Left;
            indicatorPrefab.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    public void HandleInput(Vector2 input)
    {
        if (!isMinigameActive)
        {
            return;
        }

        if (currentIndicatorState != IndicatorState.Neutral || directionalArms.hasTriedInput)
        {
            return; // Ignore input if we're still showing success/failure from the last input
        }
        if (directionalArms.CheckInput(input))
        {
            AdjustIndicator(input);
            indicatorPrefab.GetComponent<SpriteRenderer>().color = Color.green; // Indicate success
            DamageEnemies(BattleController.CurrentBattle?.enemies);
            currentIndicatorState = IndicatorState.Success;
            requiredLinks--;
            if (requiredLinks <= 0)
            {
                EndMinigame();
            }
        } else
        {
            AdjustIndicator(input);
            indicatorPrefab.GetComponent<SpriteRenderer>().color = Color.red; // Indicate failure
            DamageAllies(BattleController.CurrentBattle?.allies);
            currentIndicatorState = IndicatorState.Failure;
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
