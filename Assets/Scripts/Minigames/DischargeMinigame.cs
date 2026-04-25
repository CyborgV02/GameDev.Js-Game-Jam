using UnityEngine;

public class DischargeMinigame : MonoBehaviour, IMinigame
{
    [SerializeField] private float _duration = 6f;
    [SerializeField] private int _allyDamage = 20;
    [SerializeField] private int _enemyDamage = 25;
    [SerializeField] private string _name = "Discharge";
    [SerializeField] private int chargeTarget = 12;

    private bool isMinigameActive = false;
    private float remainingTime;
    private ChargeCore chargeCore;

    public string minigameName => _name;
    public float duration => _duration;
    public int allyDamage => _allyDamage;
    public int enemyDamage => _enemyDamage;

    void OnEnable()
    {
        InputController.OnActionZ += HandleZ;
    }

    void OnDisable()
    {
        InputController.OnActionZ -= HandleZ;
    }

    void Update()
    {
        UpdateMinigame(Time.deltaTime);
    }

    public void StartMinigame()
    {
        remainingTime = _duration;
        chargeCore = new ChargeCore(chargeTarget, 0.5f);
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

        if (chargeCore == null)
        {
            chargeCore = new ChargeCore(chargeTarget, 0.5f);
        }

        chargeCore.Tick(deltaTime);
        remainingTime -= deltaTime;
        if (remainingTime > 0f)
        {
            return;
        }

        if (chargeCore.IsCharged())
        {
            DamageEnemies(BattleController.CurrentBattle?.enemies);
        }
        else
        {
            DamageAllies(BattleController.CurrentBattle?.allies);
        }

        EndMinigame();
    }

    public void HandleInput(Vector2 input)
    {
    }

    private void HandleZ()
    {
        if (!isMinigameActive)
        {
            return;
        }

        if (chargeCore == null)
        {
            chargeCore = new ChargeCore(chargeTarget, 0.5f);
        }

        if (!chargeCore.Overcharge())
        {
            DamageAllies(BattleController.CurrentBattle?.allies);
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
