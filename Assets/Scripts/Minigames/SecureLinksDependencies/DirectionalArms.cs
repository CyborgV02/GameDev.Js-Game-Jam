using System.Collections.Generic;
using UnityEngine;

public class DirectionalArms
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public Arm[] arms = new Arm[4];
    Direction currentDirection = RandomizeDirection();
    public float DecisionTime = 1.5f;
    public float ArmMoveDistance = 15f;
    public float InputResetTimer = 0.5f;
    public bool hasTriedInput = false;


    public DirectionalArms(ref Arm[] arms)
    {
        this.arms = arms;
    }

    static Direction RandomizeDirection()
    {
        return (Direction)Random.Range(0, 4);
    }

    static Direction? GetInputDirection(Vector2 input)
    {
        if (input.y > 0.5f) return Direction.Up;
        if (input.y < -0.5f) return Direction.Down;
        if (input.x > 0.5f) return Direction.Right;
        if (input.x < -0.5f) return Direction.Left;
        return null;
    }
    
    static Vector3 DirectionToVector(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up: return new Vector3(0f, 1f, 0f);
            case Direction.Down: return new Vector3(0f, -1f, 0f);
            case Direction.Left: return new Vector3(-1f, 0f, 0f);
            case Direction.Right: return new Vector3(1f, 0f, 0f);
            default: return Vector3.zero;
        }
    }

    public bool CheckInput(Vector2 input)
    {
        if (InputResetTimer > 0f)
        {
            return false; // Ignore input during reset timer
        }
        InputResetTimer = 0.5f;
        hasTriedInput = true;
        Direction? inputDirection = GetInputDirection(input);
        if (inputDirection != null && inputDirection == currentDirection)
        {
            // Correct input, reset arm and damage enemy
            arms[(int)currentDirection].Reset();
            return true;
        }
        else
        {
            // Incorrect input, Damage player
            return false;
        }
    }
    void MoveArm(Direction dir)
    {
        arms[(int)dir].Attack();
        arms[(int)dir].armObject.transform.position += -DirectionToVector(dir) * ArmMoveDistance; // Move arm in the direction it's attacking
    }

    public void Update()
    {
        DecisionTime -= Time.deltaTime;
        InputResetTimer -= Time.deltaTime;
        if (DecisionTime <= 0f)
        {
            hasTriedInput = false;
            currentDirection = RandomizeDirection();
            arms[(int)currentDirection].Attack();
            DecisionTime = 1.5f; // Reset decision time
        }
        foreach (Arm arm in arms)
        {
            arm.SecureUpdate();
        }
    }

}
