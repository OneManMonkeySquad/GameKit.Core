using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

public enum EaseType
{
    Linear,
    InSine, OutSine, InOutSine,
    InQuad, OutQuad, InOutQuad,
    InCubic, OutCubic, InOutCubic,
    InQuart, OutQuart, InOutQuart,
    InQuint, OutQuint, InOutQuint,
    InExpo, OutExpo, InOutExpo,
    InCirc, OutCirc, InOutCirc,
    InBack, OutBack, InOutBack,
    InElastic, OutElastic, InOutElastic,
    InBounce, OutBounce, InOutBounce
}

/// <summary>
/// https://easings.net
/// </summary>
public static class Ease
{
    public static double Evaluate(EaseType type, double t) => type switch
    {
        EaseType.Linear => t,
        EaseType.InSine => InSine(t),
        EaseType.OutSine => OutSine(t),
        EaseType.InOutSine => InOutSine(t),
        EaseType.InQuad => InQuad(t),
        EaseType.OutQuad => OutQuad(t),
        EaseType.InOutQuad => InOutQuad(t),
        EaseType.InCubic => InCubic(t),
        EaseType.OutCubic => OutCubic(t),
        EaseType.InOutCubic => InOutCubic(t),
        EaseType.InQuart => InQuart(t),
        EaseType.OutQuart => OutQuart(t),
        EaseType.InOutQuart => InOutQuart(t),
        EaseType.InQuint => InQuint(t),
        EaseType.OutQuint => OutQuint(t),
        EaseType.InOutQuint => InOutQuint(t),
        EaseType.InExpo => InExpo(t),
        EaseType.OutExpo => OutExpo(t),
        EaseType.InOutExpo => InOutExpo(t),
        EaseType.InCirc => InCirc(t),
        EaseType.OutCirc => OutCirc(t),
        EaseType.InOutCirc => InOutCirc(t),
        EaseType.InBack => InBack(t),
        EaseType.OutBack => OutBack(t),
        EaseType.InOutBack => InOutBack(t),
        EaseType.InElastic => InElastic(t),
        EaseType.OutElastic => OutElastic(t),
        EaseType.InOutElastic => InOutElastic(t),
        EaseType.InBounce => InBounce(t),
        EaseType.OutBounce => OutBounce(t),
        EaseType.InOutBounce => InOutBounce(t),
        _ => throw new Exception("invalid ease"),
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double InSine(double t) => math.sin(1.5707963 * t);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double OutSine(double t) => 1 + math.sin(1.5707963 * (--t));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double InOutSine(double t) => 0.5 * (1 + math.sin(3.1415926 * (t - 0.5)));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double InQuad(double t) => t * t;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double OutQuad(double t) => t * (2 - t);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double InOutQuad(double t) => t < 0.5 ? 2 * t * t : t * (4 - 2 * t) - 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double InCubic(double t) => t * t * t;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double OutCubic(double t) => 1 + (--t) * t * t;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double InOutCubic(double t) => t < 0.5 ? 4 * t * t * t : 1 + (--t) * (2 * (--t)) * (2 * t);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double InQuart(double t)
    {
        t *= t;
        return t * t;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double OutQuart(double t)
    {
        t = (--t) * t;
        return 1 - t * t;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double InOutQuart(double t)
    {
        if (t < 0.5)
        {
            t *= t;
            return 8 * t * t;
        }
        else
        {
            t = (--t) * t;
            return 1 - 8 * t * t;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double InQuint(double t)
    {
        double t2 = t * t;
        return t * t2 * t2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double OutQuint(double t)
    {
        double t2 = (--t) * t;
        return 1 + t * t2 * t2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double InOutQuint(double t)
    {
        double t2;
        if (t < 0.5)
        {
            t2 = t * t;
            return 16 * t * t2 * t2;
        }
        else
        {
            t2 = (--t) * t;
            return 1 + 16 * t * t2 * t2;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double InExpo(double t) => (math.pow(2, 8 * t) - 1) / 255;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double OutExpo(double t) => 1 - math.pow(2, -8 * t);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double InOutExpo(double t)
    {
        if (t < 0.5)
        {
            return (math.pow(2, 16 * t) - 1) / 510;
        }
        else
        {
            return 1 - 0.5 * math.pow(2, -16 * (t - 0.5));
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double InCirc(double t) => 1 - math.sqrt(1 - t);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double OutCirc(double t)
    {
        return math.sqrt(t);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double InOutCirc(double t)
    {
        if (t < 0.5)
        {
            return (1 - math.sqrt(1 - 2 * t)) * 0.5;
        }
        else
        {
            return (1 + math.sqrt(2 * t - 1)) * 0.5;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double InBack(double t) => t * t * (2.70158 * t - 1.70158);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double OutBack(double t) => 1 + (--t) * t * (2.70158 * t + 1.70158);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double InOutBack(double t)
    {
        if (t < 0.5)
        {
            return t * t * (7 * t - 2.5) * 2;
        }
        else
        {
            return 1 + (--t) * t * 2 * (7 * t + 2.5);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double InElastic(double t)
    {
        double t2 = t * t;
        return t2 * t2 * math.sin(t * math.PI * 4.5);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double OutElastic(double t)
    {
        double t2 = (t - 1) * (t - 1);
        return 1 - t2 * t2 * math.cos(t * math.PI * 4.5);
    }

    public static double InOutElastic(double t)
    {
        double t2;
        if (t < 0.45)
        {
            t2 = t * t;
            return 8 * t2 * t2 * math.sin(t * math.PI * 9);
        }
        else if (t < 0.55)
        {
            return 0.5 + 0.75 * math.sin(t * math.PI * 4);
        }
        else
        {
            t2 = (t - 1) * (t - 1);
            return 1 - 8 * t2 * t2 * math.sin(t * math.PI * 9);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double InBounce(double t)
    {
        return math.pow(2, 6 * (t - 1)) * math.abs(math.sin(t * math.PI * 3.5));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double OutBounce(double t)
    {
        return 1 - math.pow(2, -6 * t) * math.abs(math.cos(t * math.PI * 3.5));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double InOutBounce(double t)
    {
        if (t < 0.5)
        {
            return 8 * math.pow(2, 8 * (t - 1)) * math.abs(math.sin(t * math.PI * 7));
        }
        else
        {
            return 1 - 8 * math.pow(2, -8 * t) * math.abs(math.sin(t * math.PI * 7));
        }
    }
}