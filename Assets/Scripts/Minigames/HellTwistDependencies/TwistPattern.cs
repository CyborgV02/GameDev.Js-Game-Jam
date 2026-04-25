using UnityEngine;

public class TwistPattern
{
    public int RequiredDodgeCount { get; }
    public int CurrentDodgeCount { get; private set; }
    public Vector2 CurrentThreatDirection { get; private set; }

    public TwistPattern(int requiredDodgeCount)
    {
        RequiredDodgeCount = Mathf.Max(1, requiredDodgeCount);
        CurrentThreatDirection = Vector2.up;
    }

    public void StepPattern()
    {
        if (CurrentThreatDirection == Vector2.up)
        {
            CurrentThreatDirection = Vector2.right;
        }
        else if (CurrentThreatDirection == Vector2.right)
        {
            CurrentThreatDirection = Vector2.down;
        }
        else if (CurrentThreatDirection == Vector2.down)
        {
            CurrentThreatDirection = Vector2.left;
        }
        else
        {
            CurrentThreatDirection = Vector2.up;
        }
    }

    public bool RegisterDodge(Vector2 input)
    {
        if (input == Vector2.zero)
        {
            return false;
        }

        if (Vector2.Dot(input.normalized, CurrentThreatDirection.normalized) > 0.25f)
        {
            CurrentDodgeCount++;
            StepPattern();
            return true;
        }

        CurrentDodgeCount = Mathf.Max(0, CurrentDodgeCount - 1);
        StepPattern();
        return false;
    }

    public bool IsComplete()
    {
        return CurrentDodgeCount >= RequiredDodgeCount;
    }
}
