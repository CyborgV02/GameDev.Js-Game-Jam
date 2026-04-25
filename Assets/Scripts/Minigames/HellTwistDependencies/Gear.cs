using System;
using UnityEngine;

public class Gear
{
    private bool isMainGear = false;
    private GameObject gearObject;
    private GameObject wrenchObject = null;
    private bool clockwise;
    private Rigidbody2D gearRigidbody;
    private Collider2D gearCollider;
    private Collider2DRelay colliderRelay;
    public Action OnDamaged;

    public Gear(GameObject gearObject, bool isMainGear, bool clockwise = true)
    {
        this.gearObject = gearObject;
        this.isMainGear = isMainGear;
        this.clockwise = clockwise;
        if (isMainGear)
        {
            gearRigidbody = gearObject.GetComponent<Rigidbody2D>();
            if (gearRigidbody == null)
            {
                Debug.LogError("Main gear must have a Rigidbody2D component.");
            }
        }
    }

    public void SetWrench(GameObject wrench)
    {
        wrenchObject = wrench;

        colliderRelay = wrenchObject.GetComponent<Collider2DRelay>();
        if (colliderRelay == null)
        {
            colliderRelay = wrenchObject.AddComponent<Collider2DRelay>();
        }
        // colliderRelay.TriggerEntered += HandleTriggerEntered;
        colliderRelay.CollisionEntered += HandleCollisionEntered;
    }

    public void Rotate(float angle)
    {
        if (isMainGear && gearRigidbody != null)
        {
            float torque = clockwise ? -angle : angle;
            gearRigidbody.AddTorque(torque, ForceMode2D.Force);
        }
        else
        {
            gearObject.transform.Rotate(Vector3.forward, clockwise ? -angle : angle);
        }
    }

    public void AutoRotation(float angle)
    {
        if (isMainGear)
        {
            Rotate(angle);
        }
    }

    public void Dispose()
    {
        if (colliderRelay == null)
        {
            return;
        }

        // colliderRelay.TriggerEntered -= HandleTriggerEntered;
        colliderRelay.CollisionEntered -= HandleCollisionEntered;
    }

    private void HandleCollisionEntered(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            OnDamaged?.Invoke();
        }
    }

}
