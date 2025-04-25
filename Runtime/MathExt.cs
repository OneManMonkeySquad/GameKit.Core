using System.Runtime.CompilerServices;
using Unity.Mathematics;

public static class MathExt
{
    public const float TAU = 6.2831853071795862f;
    public const double TAU_DBL = 6.2831853071795862;

    /// <summary>
    /// Returns a quaternion rotating from one vector to another.
    /// </summary>
    public static quaternion FromToRotation(float3 from, float3 to)
    {
        var fromNorm = math.normalize(from);
        var toNorm = math.normalize(to);
        float dot = math.dot(fromNorm, toNorm);
        dot = math.clamp(dot, -1f, 1f);

        float3 axis = math.cross(fromNorm, toNorm);
        if (math.lengthsq(axis) < 1e-6f) // Handle near-parallel case
            axis = math.any(fromNorm != new float3(0, 1, 0)) ? math.cross(fromNorm, new float3(0, 1, 0)) : math.cross(fromNorm, new float3(1, 0, 0));

        return quaternion.AxisAngle(math.normalize(axis), math.acos(dot));
    }

    /// <summary>
    /// Returns the closest point on a ray from a given point.
    /// </summary>
    public static float3 FindNearestPointOnRay(float3 origin, float3 direction, float3 point)
    {
        var dirNormalized = math.normalize(direction);
        float dotP = math.dot(point - origin, dirNormalized);
        dotP = math.max(dotP, 0); // Only points in the direction of the ray
        return origin + dirNormalized * dotP;
    }

    /// <summary>
    /// Returns the closest point on a line segment to a given point.
    /// </summary>
    public static float3 FindNearestPointOnLine(float3 start, float3 end, float3 point)
    {
        float3 heading = end - start;
        float magnitudeMax = math.length(heading);
        float3 headingNormalized = math.normalize(heading);
        float dotP = math.dot(point - start, headingNormalized);
        dotP = math.clamp(dotP, 0f, magnitudeMax);
        return start + headingNormalized * dotP;
    }

    /// <summary>
    /// Loops the value t so that it is never larger than length and never smaller than 0.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Repeat(float t, float length)
    {
        return t - length * math.floor(t / length);
    }

    /// <summary>
    /// Returns the shortest difference between two angles in degrees.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DeltaAngleDegrees(float current, float target)
    {
        float delta = Repeat(target - current, 360.0f);
        if (delta > 180.0f)
            delta -= 360.0f;
        return delta;
    }

    /// <summary>
    /// Returns the signed angle in degrees from 180 to -180 between two float3s using the global up axis.
    /// </summary>
    public static float SignedAngle(float3 from, float3 to) => SignedAngle(from, to, math.up());

    /// <summary>
    /// Returns the signed angle in degrees between two float3s relative to the given axis.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float SignedAngle(float3 from, float3 to, float3 axis)
    {
        float angle = math.acos(math.clamp(math.dot(math.normalize(from), math.normalize(to)), -1f, 1f));
        float sign = math.sign(math.dot(axis, math.cross(from, to)));
        return math.degrees(angle) * sign;
    }

    /// <summary>
    /// Rotates the angle 'from' towards 'to' by a maximum of 'maxAngle' degrees.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float RotateTowards(float from, float to, float maxAngle)
        => math.degrees(RotateTowardsRad(math.radians(from), math.radians(to), math.radians(maxAngle)));

    static float RotateTowardsRad(float currentAngle, float targetAngle, float maxRotation)
    {
        float delta = math.atan2(math.sin(targetAngle - currentAngle), math.cos(targetAngle - currentAngle));
        float rotation = math.clamp(delta, -maxRotation, maxRotation);
        return currentAngle + rotation;
    }

    /// <summary>
    /// Linearly interpolates between two angles in degrees while ensuring the shortest path is taken.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float LerpAngle(float from, float to, float weight)
        => math.degrees(LerpAngleRad(math.radians(from), math.radians(to), weight));

    static float LerpAngleRad(float from, float to, float weight)
    {
        float delta = math.atan2(math.sin(to - from), math.cos(to - from));
        return math.fmod(from + delta * weight + TAU, TAU); // Normalize to [0, TAU)
    }

    /// <summary>
    /// Gradually changes a value towards a desired goal over time.
    /// </summary>
    public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
    {
        smoothTime = math.max(0.0001F, smoothTime);
        float omega = 2F / smoothTime;

        float x = omega * deltaTime;
        float exp = 1F / (1F + x + 0.48F * x * x + 0.235F * x * x * x);
        float change = current - target;
        float originalTo = target;

        float maxChange = maxSpeed * smoothTime;
        change = math.clamp(change, -maxChange, maxChange);
        target = current - change;

        float temp = (currentVelocity + omega * change) * deltaTime;
        currentVelocity = (currentVelocity - omega * temp) * exp;
        float output = target + (change + temp) * exp;

        if ((originalTo - current > 0.0F) == (output > originalTo))
        {
            output = originalTo;
            currentVelocity = (output - originalTo) / deltaTime;
        }

        return output;
    }

    /// <summary>
    /// Gradually changes an angle in degrees toward a desired goal.
    /// </summary>
    public static float SmoothDampAngleDegrees(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
    {
        target = current + DeltaAngleDegrees(current, target);
        return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
    }

    /// <summary>
    /// Gradually changes a float3 towards a desired goal over time.
    /// </summary>
    public static float3 SmoothDamp(float3 current, float3 target, ref float3 currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
    {
        smoothTime = math.max(0.0001F, smoothTime);
        float omega = 2F / smoothTime;

        float x = omega * deltaTime;
        float exp = 1F / (1F + x + 0.48F * x * x + 0.235F * x * x * x);

        float3 change = current - target;
        float3 originalTo = target;

        float maxChange = maxSpeed * smoothTime;
        float sqrmag = math.lengthsq(change);
        if (sqrmag > maxChange * maxChange)
        {
            change = math.normalize(change) * maxChange;
        }

        target = current - change;
        float3 temp = (currentVelocity + omega * change) * deltaTime;
        currentVelocity = (currentVelocity - omega * temp) * exp;
        float3 output = target + (change + temp) * exp;

        float3 origMinusCurrent = originalTo - current;
        float3 outMinusOrig = output - originalTo;
        if (math.dot(origMinusCurrent, outMinusOrig) > 0)
        {
            output = originalTo;
            currentVelocity = (output - originalTo) / deltaTime;
        }

        return output;
    }
}
