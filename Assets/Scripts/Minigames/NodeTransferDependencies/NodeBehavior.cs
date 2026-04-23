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
        if (other.CompareTag("Player"))
        {
            if (nodeType == NodeType.Objective)
                NodeTransfer.ObjectiveCollected?.Invoke();
            else if (nodeType == NodeType.Goal)
                NodeTransfer.GoalCollected?.Invoke();
            else if (nodeType == NodeType.Enemy)
                NodeTransfer.PlayerDamaged?.Invoke();
        }
    }
}
