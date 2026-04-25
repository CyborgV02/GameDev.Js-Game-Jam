using UnityEngine;

public class ChargeCore
{
    public int ChargeTarget { get; }
    public int CurrentCharge { get; private set; }
    public float PulseInterval { get; }
    private float pulseTimer;

    public ChargeCore(int chargeTarget, float pulseInterval)
    {
        ChargeTarget = Mathf.Max(1, chargeTarget);
        PulseInterval = Mathf.Max(0.1f, pulseInterval);
        CurrentCharge = 0;
        pulseTimer = PulseInterval;
    }

    public void Tick(float deltaTime)
    {
        pulseTimer -= deltaTime;
        if (pulseTimer > 0f)
        {
            return;
        }

        pulseTimer = PulseInterval;
        CurrentCharge = Mathf.Min(ChargeTarget, CurrentCharge + 1);
    }

    public bool Overcharge()
    {
        if (CurrentCharge >= ChargeTarget)
        {
            return false;
        }

        CurrentCharge++;
        return true;
    }

    public bool IsCharged()
    {
        return CurrentCharge >= ChargeTarget;
    }
}
