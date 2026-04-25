using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum NodeType
{
    Goal,
    Enemy,
    Objective
}

[RequireComponent(typeof(CircleCollider2D))]
public class NodeBehavior : MonoBehaviour
{
    [SerializeField] private NodeType nodeType;
    private CircleCollider2D circleCollider;
    void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Node trigger entered by {other.gameObject.name}");
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player entered {nodeType} node trigger");
            if (nodeType == NodeType.Objective)
                NodeTransferMinigame.ObjectiveCollected?.Invoke();
            else if (nodeType == NodeType.Goal)
                NodeTransferMinigame.GoalCollected?.Invoke();
            else if (nodeType == NodeType.Enemy)
                NodeTransferMinigame.PlayerDamaged?.Invoke();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Node collision entered by {collision.gameObject.name}");
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Player collided with {nodeType} node");
        }
    }
}
