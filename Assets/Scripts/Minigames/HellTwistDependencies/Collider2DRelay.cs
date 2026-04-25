using System;
using UnityEngine;

public class Collider2DRelay : MonoBehaviour
{
    public event Action<Collider2D> TriggerEntered;
    public event Action<Collision2D> CollisionEntered;

    void OnTriggerEnter2D(Collider2D other)
    {
        TriggerEntered?.Invoke(other);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        CollisionEntered?.Invoke(collision);
    }
}