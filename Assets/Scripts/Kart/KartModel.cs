using R3;
using UnityEngine;

public sealed class KartModel
{
    readonly float _maxSpeed;
    readonly float _acceleration;
    readonly float _brakeDeceleration;
    readonly float _turnSpeedDegreesPerSecond;
    readonly ReactiveProperty<bool> _isStunned = new(false);
    float _stunTimer;
    float _boostMultiplier = 1f;
    float _boostTimer;

    public float Speed { get; private set; }
    public float Heading { get; private set; }
    public Vector3 Position { get; private set; }
    public ReadOnlyReactiveProperty<bool> IsStunned => _isStunned;

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

    // スタン中は速度を0付近まで落として操作不能にする(docs/spec/05-items.md)。
    public void ApplyStun(float duration)
    {
        _isStunned.Value = true;
        _stunTimer = duration;
        Speed = 0f;
    }

    // 使用した瞬間から一定時間、最高速度を超えて速度上昇する(docs/spec/05-items.md)。
    public void ApplyBoost(float duration, float speedMultiplier)
    {
        _boostMultiplier = speedMultiplier;
        _boostTimer = duration;
        Speed = _maxSpeed * speedMultiplier;
    }

    public void Tick(float steer, bool accelerate, bool brake, float deltaTime)
    {
        if (_isStunned.CurrentValue)
        {
            _stunTimer -= deltaTime;
            if (_stunTimer <= 0f)
            {
                _isStunned.Value = false;
            }
            return;
        }

        if (_boostTimer > 0f)
        {
            _boostTimer -= deltaTime;
            if (_boostTimer <= 0f)
            {
                _boostMultiplier = 1f;
            }
        }

        float effectiveMaxSpeed = _maxSpeed * _boostMultiplier;

        if (accelerate)
        {
            Speed = Mathf.Min(Speed + _acceleration * deltaTime, effectiveMaxSpeed);
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
