using UnityEngine;

public class KartController : MonoBehaviour
{
    [Header("カート性能")]
    [SerializeField, Tooltip("最高速度 (m/s)")]
    float _maxSpeed = 20f;

    [SerializeField, Tooltip("加速度 (m/s^2)")]
    float _acceleration = 10f;

    [SerializeField, Tooltip("ブレーキ減速度 (m/s^2)")]
    float _brakeDeceleration = 15f;

    [SerializeField, Tooltip("旋回速度 (度/秒、最大ステアリング時)")]
    float _turnSpeedDegreesPerSecond = 90f;

    KartModel _model;

    public float Steer { get; set; }
    public bool Accelerate { get; set; }
    public bool Brake { get; set; }
    public ItemHolder ItemHolder { get; } = new();

    void Awake()
    {
        _model = new KartModel(
            _maxSpeed,
            _acceleration,
            _brakeDeceleration,
            _turnSpeedDegreesPerSecond,
            transform.position,
            transform.eulerAngles.y
        );
    }

    void Update()
    {
        _model.Tick(Steer, Accelerate, Brake, Time.deltaTime);
        transform.position = _model.Position;
        transform.rotation = Quaternion.Euler(0f, _model.Heading, 0f);
    }
}
