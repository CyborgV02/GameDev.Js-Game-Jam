using System;
using UnityEngine;

public class Gear
{
    private bool isMainGear = false;
    private GameObject gearObject;
    private bool clockwise;
    private float minMainAngle;
    private float maxMainAngle;
    private float currentMainAngle;
    public Action OnDamaged;
    public Action OnBoundaryHit;

    public Gear(GameObject gearObject, bool isMainGear, bool clockwise = true, float minMainAngle = -90f, float maxMainAngle = 90f)
    {
        this.gearObject = gearObject;
        this.isMainGear = isMainGear;
        this.clockwise = clockwise;
        this.minMainAngle = minMainAngle;
        this.maxMainAngle = maxMainAngle;

        if (this.minMainAngle >= this.maxMainAngle)
        {
            this.minMainAngle = -90f;
            this.maxMainAngle = 90f;
            Debug.LogWarning("Invalid main gear angle bounds. Falling back to -90..90.");
        }

        if (isMainGear)
        {
            currentMainAngle = NormalizeSignedAngle(gearObject.transform.localEulerAngles.z);
            currentMainAngle = Mathf.Clamp(currentMainAngle, this.minMainAngle, this.maxMainAngle);
            ApplyMainAngle(currentMainAngle);
        }
    }

    public void SetWrench(GameObject wrench)
    {
        // Kept for compatibility with existing callers. Collision is no longer used by main gear.
    }

    public void Rotate(float angle)
    {
        if (isMainGear)
        {
            float signedDelta = clockwise ? -angle : angle;
            float nextAngle = currentMainAngle + signedDelta;
            float reflectedAngle = ReflectIntoRange(nextAngle, minMainAngle, maxMainAngle, out bool reflected);

            currentMainAngle = reflectedAngle;
            ApplyMainAngle(currentMainAngle);

            if (reflected)
            {
                OnDamaged?.Invoke();
                OnBoundaryHit?.Invoke();
            }
        }
        else
        {
            gearObject.transform.RotateAround(gearObject.transform.position, Vector3.forward, clockwise ? -angle : angle);
        }
    }

    public void AutoRotation(float angle)
    {
        if (isMainGear)
        {
            Rotate(angle);
        }
    }

    private void ApplyMainAngle(float angle)
    {
        Vector3 euler = gearObject.transform.localEulerAngles;
        euler.z = angle;
        gearObject.transform.localEulerAngles = euler;
    }

    private static float NormalizeSignedAngle(float rawAngle)
    {
        float angle = rawAngle % 360f;
        if (angle > 180f)
        {
            angle -= 360f;
        }
        return angle;
    }

    private static float ReflectIntoRange(float value, float min, float max, out bool reflected)
    {
        reflected = false;
        float result = value;

        // Reflect at boundaries so motion remains continuous and never teleports.
        while (result < min || result > max)
        {
            reflected = true;
            if (result > max)
            {
                result = max - (result - max);
            }
            else if (result < min)
            {
                result = min + (min - result);
            }
        }

        return result;
    }

}
