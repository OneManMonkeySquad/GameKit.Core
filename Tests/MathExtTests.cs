using NUnit.Framework;
using Unity.Mathematics;

[TestFixture]
public class MathExtTests
{
    [Test]
    public void FromToRotation_OrthogonalVectors_CorrectRotation()
    {
        float3 from = new float3(1, 0, 0);
        float3 to = new float3(0, 1, 0);
        quaternion q = MathExt.FromToRotation(from, to);
        float3 result = math.rotate(q, from);

        Assert.That(math.distance(result, math.normalize(to)), Is.LessThan(1e-4));
    }

    [Test]
    public void FindNearestPointOnRay_PointBehindOrigin_ClampedToOrigin()
    {
        float3 origin = float3.zero;
        float3 direction = new float3(1, 0, 0);
        float3 point = new float3(-5, 0, 0);

        float3 nearest = MathExt.FindNearestPointOnRay(origin, direction, point);
        Assert.AreEqual(origin, nearest);
    }

    [Test]
    public void FindNearestPointOnLine_PointOutside_ClampedToEdge()
    {
        float3 start = float3.zero;
        float3 end = new float3(1, 0, 0);
        float3 point = new float3(5, 0, 0);

        float3 nearest = MathExt.FindNearestPointOnLine(start, end, point);
        Assert.AreEqual(end, nearest);
    }

    [Test]
    public void Repeat_ValueBeyondLength_CorrectWrapped()
    {
        Assert.AreEqual(2f, MathExt.Repeat(7f, 5f));
        Assert.AreEqual(0f, MathExt.Repeat(10f, 5f));
        Assert.AreEqual(1f, MathExt.Repeat(6f, 5f));
    }

    [Test]
    public void DeltaAngleDegrees_WrapsProperly()
    {
        float delta = MathExt.DeltaAngleDegrees(350f, 10f);
        Assert.AreEqual(20f, delta, 0.01f);
    }

    [Test]
    public void SignedAngle_Between90DegreesVectors_ReturnsMinus90()
    {
        float3 a = new float3(1, 0, 0); // X
        float3 b = new float3(0, 0, 1); // Z
        float angle = MathExt.SignedAngle(a, b, new float3(0, 1, 0));
        Assert.AreEqual(-90f, angle, 1e-3f);
    }

    [Test]
    public void RotateTowards_DoesNotExceedMaxAngle()
    {
        float from = 0;
        float to = 90;
        float rotated = MathExt.RotateTowards(from, to, 10f);
        Assert.AreEqual(10f, rotated, 0.01f);
    }

    [Test]
    public void LerpAngle_ShortestPathChosen()
    {
        float result = MathExt.LerpAngle(350f, 10f, 0.5f);
        Assert.That(result, Is.EqualTo(0f).Within(1f)); // 350->10 shortest path = 0 at halfway
    }

    [Test]
    public void SmoothDamp_ReachesTargetEventually()
    {
        float velocity = 0;
        float result = MathExt.SmoothDamp(0, 10, ref velocity, 0.5f, 100f, 0.1f);
        Assert.That(result, Is.GreaterThan(0).And.LessThan(10));
    }

    [Test]
    public void SmoothDampAngleDegrees_CorrectlyWraps()
    {
        float velocity = 0;
        float result = MathExt.SmoothDampAngleDegrees(350, 10, ref velocity, 0.3f, 100f, 0.1f);
        Assert.That(result, Is.GreaterThan(350).Or.LessThan(10));
    }

    [Test]
    public void SmoothDampFloat3_WorksAsExpected()
    {
        float3 velocity = float3.zero;
        float3 result = MathExt.SmoothDamp(new float3(0, 0, 0), new float3(1, 1, 1), ref velocity, 0.5f, 100f, 0.1f);

        Assert.That(math.length(result), Is.GreaterThan(0).And.LessThan(1.5f));
    }
}
