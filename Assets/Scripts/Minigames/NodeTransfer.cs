using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeTransfer : MonoBehaviour
{
    [SerializeField] private GameObject battleSquare;
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Node node;
    [SerializeField] private Node GoalNode;
    [SerializeField] private Node objectiveNode;
    [SerializeField] private Node enemyNode;

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
}
