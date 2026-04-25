using UnityEngine;

public class HellTwistMinigame : MonoBehaviour, IMinigame
{
    [SerializeField] private float _duration = 8f;
    [SerializeField] private int _allyDamage = 30;
    [SerializeField] private int _enemyDamage = 30;
    [SerializeField] private string _name = "Hell Twist";
    
    [SerializeField] private GameObject mainGearObject;
    [SerializeField] private GameObject wrenchObject;
    [SerializeField] private GameObject[] enemyGearObjects;

    private Gear mainGear;
    private Gear[] enemyGears;

    public const float rotationSpeedMainGear = 100f;
    public const float rotationSpeedEnemyGear = 50f;

    public const float directionChangeInterval = 2f;

    private bool isMinigameActive = true; // Set to true to start the minigame immediately for testing
    private float remainingTime = 8f;
    private float directionChangeTimer = 2f;
    private bool GearClockwise = true;

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

    void Awake()
    {
        remainingTime = _duration;
        mainGear = new Gear(mainGearObject, isMainGear: true);
        enemyGears = new Gear[enemyGearObjects.Length];
        for (int i = 0; i < enemyGearObjects.Length; i++)
        {
            enemyGears[i] = new Gear(enemyGearObjects[i], isMainGear: false);
        }
        mainGear.SetWrench(wrenchObject);
        mainGear.OnDamaged += () =>
        {
            DamageAllies(BattleController.CurrentBattle?.allies);
        };
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

        remainingTime -= deltaTime;
        directionChangeTimer -= deltaTime;

        if (!GearClockwise)
        {
            deltaTime = -deltaTime;
        }
        
        if (directionChangeTimer <= 0f)
        {
            GearClockwise = !GearClockwise;
            directionChangeTimer = directionChangeInterval;
        }

        foreach (Gear enemyGear in enemyGears)
        {
            enemyGear.Rotate(rotationSpeedEnemyGear * deltaTime);
        }
        mainGear.AutoRotation(rotationSpeedEnemyGear * deltaTime);

        if (remainingTime > 0f)
        {
            return;
        }
    }

    public void HandleInput(Vector2 input)
    {
        if (!isMinigameActive)
        {
            return;
        }

        float rotationAmount = input.x * rotationSpeedMainGear * Time.deltaTime;
        mainGear.Rotate(rotationAmount);

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
