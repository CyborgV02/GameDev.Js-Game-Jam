using UnityEngine;

public class SecureLinkCircuit
{
    public int RequiredLinks { get; }
    public int CurrentLinks { get; private set; }
    public bool IsPrimed { get; private set; }

    public SecureLinkCircuit(int requiredLinks)
    {
        RequiredLinks = Mathf.Max(1, requiredLinks);
        CurrentLinks = 0;
        IsPrimed = true;
    }

    public void Prime()
    {
        IsPrimed = true;
    }

    public bool LinkPulse()
    {
        if (!IsPrimed)
        {
            return false;
        }

        CurrentLinks = Mathf.Min(RequiredLinks, CurrentLinks + 1);
        IsPrimed = false;
        return true;
    }

    public bool StabilizePulse()
    {
        if (IsPrimed)
        {
            return false;
        }

        CurrentLinks = Mathf.Min(RequiredLinks, CurrentLinks + 1);
        IsPrimed = true;
        return true;
    }

    public bool IsComplete()
    {
        return CurrentLinks >= RequiredLinks;
    }

    public void BreakCircuit()
    {
        CurrentLinks = Mathf.Max(0, CurrentLinks - 1);
        IsPrimed = true;
    }
}
