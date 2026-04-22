using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeTransfer : MonoBehaviour, IMinigame
{
    [SerializeField] private GameObject battleSquare;
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Node node;
    [SerializeField] private Node GoalNode;
    [SerializeField] private Node objectiveNode;
    [SerializeField] private Node enemyNode;
    [SerializeField] private float _duration = 5f;
    [SerializeField] private int _allyDamage = 0;
    [SerializeField] private int _enemyDamage = 0;
    private string _name = "Node Transfer";

    public string name => _name;
    public float duration => _duration;
    public int allyDamage => _allyDamage;
    public int enemyDamage => _enemyDamage;

    void Start()
    {
        node = new Node(0, 0);
        objectiveNode = new Node(2, 2);
        enemyNode = new Node(1, 1);
        GoalNode = new Node(3, 3);
        Instantiate(nodePrefab, spawnPoint.position, Quaternion.identity);
    }

    void Update()
    {
        
    }

    public void StartMinigame()
    {
        // TODO: Implement StartMinigame logic
    }

    public void EndMinigame()
    {
        // TODO: Implement EndMinigame logic
    }

    public void UpdateMinigame(float deltaTime)
    {
        // TODO: Implement UpdateMinigame logic
    }

    public void HandleInput()
    {
        // TODO: Implement HandleInput logic
    }

    public void DamageAllies(Character[] allies)
    {
        // TODO: Implement DamageAllies logic
    }

    public void DamageEnemies(Enemy[] enemies)
    {
        // TODO: Implement DamageEnemies logic
    }
}
