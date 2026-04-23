using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class NodeTransfer : MonoBehaviour, IMinigame
{
    [SerializeField] private GameObject battleSquare;
    [SerializeField] private GameObject battleCamera;
    [SerializeField] private GameObject nodeObject;
    [SerializeField] private GameObject objectiveObject;
    [SerializeField] private GameObject enemyObject;
    [SerializeField] private GameObject goalObject;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Node node;
    [SerializeField] private Node GoalNode;
    [SerializeField] private Node objectiveNode;
    [SerializeField] private Node enemyNode;
    [SerializeField] private float _duration = 15f;
    [SerializeField] private int _allyDamage = 40;
    [SerializeField] private int _enemyDamage = 12;
    private string _name = "Node Transfer";
    private float objectiveScaleTime = 0f;
    private float enemyMoveTimer = 0f;
    private float enemyMoveInterval = 0.3f;

    public string name => _name;
    public float duration => _duration;
    public int allyDamage => _allyDamage;
    public int enemyDamage => _enemyDamage;

    private bool ObjectiveCollectedFlag = false;
    private bool GoalCollectedFlag = false;
    private bool isMinigameActive = false;
    public static Action ObjectiveCollected;
    public static Action GoalCollected;
    public static Action PlayerDamaged;

    void OnEnable()
    {
        ObjectiveCollected += () => {ObjectiveCollectedFlag = true; objectiveObject.SetActive(false); DamageEnemies(BattleController.CurrentBattle?.enemies);};
        GoalCollected += () => {GoalCollectedFlag = true; goalObject.SetActive(false); DamageEnemies(BattleController.CurrentBattle?.enemies); EndMinigame();};
        PlayerDamaged += () => DamageAllies(BattleController.CurrentBattle?.allies);
    }

    void OnDisable()
    {
        ObjectiveCollected = null;
        GoalCollected = null;
        PlayerDamaged = null;
    }
    void Awake()
    {
        InputController.OnMove += HandleInput;   
    }
    void Start()
    {
        InitMinigame();
    }

    void Update()
    {
        if (!isMinigameActive) return;
        battleCamera.transform.position = new Vector3(nodeObject.transform.position.x, nodeObject.transform.position.y, battleCamera.transform.position.z);
        UpdateMinigame(Time.deltaTime);
    }

    void InitMinigame()
    {
        int _enemySpawn = Random.Range(5, 12);
        int _objectiveSpawn = Random.Range(2, 12);
        int goalSpawn = Random.Range(2, 12);
        node = new Node(0, 0, nodeObject, this);
        objectiveNode = new Node(_objectiveSpawn, _objectiveSpawn, objectiveObject, this);
        enemyNode = new Node(_enemySpawn, _enemySpawn, enemyObject, this);
        GoalNode = new Node(goalSpawn, goalSpawn, goalObject, this);
    }

    public void StartMinigame()
    {
        isMinigameActive = true;
    }

    public void EndMinigame()
    {
        isMinigameActive = false;
    }

    public void UpdateMinigame(float deltaTime)
    {
        if (!isMinigameActive) return;
        // Move enemy towards the node at regular intervals
        enemyMoveTimer += deltaTime;
        if (enemyMoveTimer >= enemyMoveInterval && enemyNode != null && node != null)
        {
            enemyMoveTimer = 0f;
            (int,int) directionToNode = (0, 0);
            if (enemyNode.x < node.x)
            {
                directionToNode.Item1 = 1;
            }
            else if (enemyNode.x > node.x)
            {
                directionToNode.Item1 = -1;
            }
            if (enemyNode.y < node.y)
            {
                directionToNode.Item2 = 1;
            }
            else if (enemyNode.y > node.y)
            {
                directionToNode.Item2 = -1;
            }
            enemyNode.Move(directionToNode);
        }

        // Objective radar logic
        // The node keeps scaling up and down to indicate how close the player is to the objective. The closer they are, the larger the node gets.
        float distanceToObjective = 0f;
        if (ObjectiveCollectedFlag == false)
        {
            distanceToObjective = Vector2.Distance(new Vector2(node.x, node.y), new Vector2(objectiveNode.x, objectiveNode.y));
        } else if (GoalCollectedFlag == false)
        {
            distanceToObjective = Vector2.Distance(new Vector2(node.x, node.y), new Vector2(GoalNode.x, GoalNode.y));
        }
        float maxDistance = 20f;
        float proximityFactor = Mathf.Clamp01(1 - (distanceToObjective / maxDistance));
        
        // Bumping effect on player node: increases bump speed and intensity as player gets closer to objective
        objectiveScaleTime += Time.deltaTime;
        float bumpSpeed = Mathf.Lerp(1f, 6f, proximityFactor);
        float bumpIntensity = Mathf.Lerp(0f, 0.4f, proximityFactor);
        float baseScale = 2f;
        float bumpAmount = Mathf.Sin(objectiveScaleTime * bumpSpeed * Mathf.PI * 2f) * bumpIntensity;
        float playerScale = Mathf.Clamp(baseScale + bumpAmount, baseScale - bumpIntensity, 7f);
        
        if (nodeObject != null)
        {
            nodeObject.transform.localScale = new Vector3(playerScale, playerScale, 1);
        }
    }

    public void HandleInput(Vector2 input)
    {
        if (!isMinigameActive) return;
        node.Move((Mathf.RoundToInt(input.x), Mathf.RoundToInt(input.y)));
    }

    public void DamageAllies(Character[] allies)
    {
        // TODO: Implement DamageAllies logic
        for (int i = 0; i < allies.Length; i++)
        {
            allies[i].TakeDamage(allyDamage);
        }
    }

    public void DamageEnemies(Enemy[] enemies)
    {
        // TODO: Implement DamageEnemies logic
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].TakeDamage(enemyDamage);
        }
        int _enemySpawn = Random.Range(5, 12);
        enemyNode.SnapToPosition((_enemySpawn, _enemySpawn));
    }
}
