using UnityEngine;

public class SecureLinksMinigame : MonoBehaviour, IMinigame
{
    [SerializeField] private float _duration = 8f;
    [SerializeField] private int _allyDamage = 10;
    [SerializeField] private int _enemyDamage = 10;
    [SerializeField] private string _name = "Secure Links";
    [SerializeField] private int requiredLinks = 5;

    private bool isMinigameActive = false;
    private float remainingTime;
    private SecureLinkCircuit linkCircuit;
    private float feedbackTimer;

    public string minigameName => _name;
    public float duration => _duration;
    public int allyDamage => _allyDamage;
    public int enemyDamage => _enemyDamage;

    void OnEnable()
    {
        InputController.OnActionZ += HandleZ;
        InputController.OnActionX += HandleX;
    }

    void OnDisable()
    {
        InputController.OnActionZ -= HandleZ;
        InputController.OnActionX -= HandleX;
    }

    void Update()
    {
        UpdateMinigame(Time.deltaTime);
    }

    public void StartMinigame()
    {
        remainingTime = _duration;
        linkCircuit = new SecureLinkCircuit(requiredLinks);
        feedbackTimer = 0f;
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

        if (linkCircuit == null)
        {
            linkCircuit = new SecureLinkCircuit(requiredLinks);
        }

        remainingTime -= deltaTime;
        feedbackTimer -= deltaTime;

        if (feedbackTimer <= 0f)
        {
            feedbackTimer = 0.25f;
            linkCircuit.Prime();
        }

        if (linkCircuit.IsComplete())
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

        if (linkCircuit == null)
        {
            linkCircuit = new SecureLinkCircuit(requiredLinks);
        }

        bool advanced = input.y >= 0f ? linkCircuit.LinkPulse() : linkCircuit.StabilizePulse();
        if (!advanced)
        {
            linkCircuit.BreakCircuit();
        }

        feedbackTimer = 0.25f;
    }

    private void HandleZ()
    {
        HandleInput(Vector2.up);
    }

    private void HandleX()
    {
        HandleInput(Vector2.down);
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
