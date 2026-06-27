using UnityEngine;

public sealed class KartModel
{
    readonly float _maxSpeed;
    readonly float _acceleration;
    readonly float _brakeDeceleration;
    readonly float _turnSpeedDegreesPerSecond;

    public float Speed { get; private set; }
    public float Heading { get; private set; }
    public Vector3 Position { get; private set; }

    public KartModel(
        float maxSpeed,
        float acceleration,
        float brakeDeceleration,
        float turnSpeedDegreesPerSecond,
        Vector3 initialPosition = default,
        float initialHeading = 0f
    )
    {
        _maxSpeed = maxSpeed;
        _acceleration = acceleration;
        _brakeDeceleration = brakeDeceleration;
        _turnSpeedDegreesPerSecond = turnSpeedDegreesPerSecond;
        Position = initialPosition;
        Heading = initialHeading;
    }

    public void Tick(float steer, bool accelerate, bool brake, float deltaTime)
    {
        if (accelerate)
        {
            Speed = Mathf.Min(Speed + _acceleration * deltaTime, _maxSpeed);
        }
        else if (brake)
        {
            Speed = Mathf.Max(Speed - _brakeDeceleration * deltaTime, 0f);
        }

        float clampedSteer = Mathf.Clamp(steer, -1f, 1f);
        Heading += clampedSteer * _turnSpeedDegreesPerSecond * deltaTime;

        Vector3 forward = Quaternion.Euler(0f, Heading, 0f) * Vector3.forward;
        Position += forward * Speed * deltaTime;
    }
}
