using UnityEngine;

// CPUは現在のウェイポイントへの方向にステアリングし、常時加速する(docs/spec/06-cpu-ai.md)。
public sealed class CpuDriverModel
{
    readonly Vector3[] _waypoints;
    readonly float _waypointReachThreshold;

    public int CurrentWaypointIndex { get; private set; }

    public CpuDriverModel(Vector3[] waypoints, float waypointReachThreshold = 2f)
    {
        _waypoints = waypoints;
        _waypointReachThreshold = waypointReachThreshold;
    }

    public float ComputeSteer(Vector3 currentPosition, float currentHeadingDegrees)
    {
        Vector3 toTarget = GetVectorToCurrentWaypoint(currentPosition);

        if (toTarget.magnitude <= _waypointReachThreshold)
        {
            CurrentWaypointIndex = (CurrentWaypointIndex + 1) % _waypoints.Length;
            toTarget = GetVectorToCurrentWaypoint(currentPosition);
        }

        float targetHeadingDegrees = Mathf.Atan2(toTarget.x, toTarget.z) * Mathf.Rad2Deg;
        float angleDifference = Mathf.DeltaAngle(currentHeadingDegrees, targetHeadingDegrees);
        return Mathf.Clamp(angleDifference / 45f, -1f, 1f);
    }

    Vector3 GetVectorToCurrentWaypoint(Vector3 currentPosition)
    {
        Vector3 toTarget = _waypoints[CurrentWaypointIndex] - currentPosition;
        toTarget.y = 0f;
        return toTarget;
    }
}
