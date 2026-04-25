using UnityEngine;

public class HellTwistMinigame : MonoBehaviour, IMinigame
{
    [SerializeField] private float _duration = 8f;
    [SerializeField] private int _allyDamage = 30;
    [SerializeField] private int _enemyDamage = 150;
    [SerializeField] private string _name = "Hell Twist";
    
    [SerializeField] private GameObject mainGearObject;
    [SerializeField] private GameObject wrenchObject;
    [SerializeField] private GameObject[] enemyGearObjects;
    [SerializeField] private float mainGearMinAngle = -20f;
    [SerializeField] private float mainGearMaxAngle = 70f;
    [SerializeField] private float inputSpeedInfluence = 0.6f;
    [SerializeField] private float minimumSpeedScale = 5f;
    [SerializeField] private float maximumSpeedScale = 20f;

    private Gear mainGear;
    private Gear[] enemyGears;

    public const float rotationSpeedMainGear = 150f;
    public const float rotationSpeedEnemyGear = 50f;

    public const float directionChangeInterval = 2f;

    private bool isMinigameActive = true; // Set to true to start the minigame immediately for testing
    private float remainingTime = 8f;
    private float directionChangeTimer = 2f;
    private bool GearClockwise = true;
    private float currentInputX;
    private int timesDamaged = 0;

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
        mainGear = new Gear(mainGearObject, isMainGear: true, clockwise: true, minMainAngle: mainGearMinAngle, maxMainAngle: mainGearMaxAngle);
        enemyGears = new Gear[enemyGearObjects.Length];
        for (int i = 0; i < enemyGearObjects.Length; i++)
        {
            enemyGears[i] = new Gear(enemyGearObjects[i], isMainGear: false);
        }
        mainGear.SetWrench(wrenchObject);
        mainGear.OnDamaged += () =>
        {
            DamageAllies(BattleController.CurrentBattle?.allies);
            timesDamaged++;
        };
        mainGear.OnBoundaryHit += () =>
        {
            GearClockwise = !GearClockwise;
            directionChangeTimer = directionChangeInterval;
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

    void WinMinigame()
    {
        isMinigameActive = false;
        DamageEnemies(BattleController.CurrentBattle?.enemies);
        EndMinigame();
    }
    
    public void EndMinigame()
    {
        isMinigameActive = false;
        InputController.OnMove -= HandleInput;
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

        float currentDirection = GearClockwise ? 1f : -1f;
        float inputAlongCurrentDirection = currentInputX * currentDirection;
        float speedScale = Mathf.Clamp(1f + (inputAlongCurrentDirection * inputSpeedInfluence), minimumSpeedScale, maximumSpeedScale);
        mainGear.AutoRotation(rotationSpeedEnemyGear * deltaTime * speedScale);

        if (remainingTime <= 0f)
        {
            WinMinigame();
        }
    }

    public void HandleInput(Vector2 input)
    {
        if (!isMinigameActive)
        {
            return;
        }
        currentInputX = -Mathf.Clamp(input.x, -1f, 1f);
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
            BattleController.NotifyAllyDamaged(ally);
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
            enemy.TakeDamage(_enemyDamage / (1 + timesDamaged)); // Reduce damage for each time player got damaged
            BattleController.NotifyEnemyDamaged(enemy);
        }
    }
}
