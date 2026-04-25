using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMinigame : MonoBehaviour, IMinigame
{
    [SerializeField] private float _duration = 10f;
    [SerializeField] private int _allyDamage = 30;
    [SerializeField] private int _enemyDamage = 15;
    [SerializeField] private string _name = "Attack Minigame";
    [SerializeField] private GameObject playerBatObject;
    [SerializeField] private GameObject enemyArmObject;

    private bool isMinigameActive = true; // return to false once done testing

    public string minigameName => _name;
    public float duration => _duration;
    public int allyDamage => _allyDamage;
    public int enemyDamage => _enemyDamage;

    private Bat playerBat;
    private Arm enemyArm;

    void Awake()
    {
        playerBat = new Bat(playerBatObject.GetComponent<Animator>());
        enemyArm = new Arm(playerBat, enemyArmObject.GetComponent<Animator>(), enemyArmObject.GetComponent<SpriteRenderer>());
        enemyArm.OnDamaged += () => DamageEnemies(BattleController.CurrentBattle?.enemies);
        playerBat.OnDamaged += () => DamageAllies(BattleController.CurrentBattle?.allies);
    }

    void Update()
    {
        UpdateMinigame(Time.deltaTime);
    }

    void OnEnable()
    {
        InputController.OnActionZ += HandleZAction;
        InputController.OnActionX += HandleXAction;
    }

    void OnDisable()
    {
        InputController.OnActionZ -= HandleZAction;
        InputController.OnActionX -= HandleXAction;
    }

    public void StartMinigame()
    {
        Debug.Log("Attack Minigame Started");
        // Implement minigame logic here
        isMinigameActive = true;
    }

    public void EndMinigame()
    {
        Debug.Log("Attack Minigame Ended");
        // Implement end logic here
        isMinigameActive = false;
        Destroy(gameObject);
    }

    public void UpdateMinigame(float deltaTime)
    {
        playerBat.Update();
        enemyArm.Update();
    }

    public void HandleInput(Vector2 input)
    {
        // Nothing to do here
    }

    void HandleZAction()
    {
        playerBat.Attack();
    }

    void HandleXAction()
    {
        playerBat.Parry();
    }

    public void DamageAllies(Character[] allies)
    {
        if (!isMinigameActive || allies == null) return;

        foreach (Character ally in allies)
        {
            ally.TakeDamage(_allyDamage);
        }
    }

    public void DamageEnemies(Enemy[] enemies)
    {
        if (!isMinigameActive || enemies == null) return;

        foreach (Enemy enemy in enemies)
        {
            enemy.TakeDamage(_enemyDamage);
        }
    }
}
