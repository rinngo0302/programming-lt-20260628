using R3;
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

    [Header("アイテム")]
    [SerializeField, Tooltip("みどり甲羅の弾プレハブ")]
    GameObject _greenShellPrefab;

    [SerializeField, Tooltip("バナナの設置トラッププレハブ")]
    GameObject _bananaPrefab;

    KartModel _model;

    public float Steer { get; set; }
    public bool Accelerate { get; set; }
    public bool Brake { get; set; }
    public bool InputEnabled { get; set; } = true;
    public ItemHolder ItemHolder { get; } = new();
    public ReadOnlyReactiveProperty<bool> IsStunned => _model.IsStunned;

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

    // カウントダウン中など操作を受け付けない間は、入力値を無視して停止状態を保つ(docs/spec/01-scenes.md)。
    void Update()
    {
        if (InputEnabled)
        {
            _model.Tick(Steer, Accelerate, Brake, Time.deltaTime);
        }
        else
        {
            _model.Tick(0f, false, false, Time.deltaTime);
        }

        transform.position = _model.Position;
        transform.rotation = Quaternion.Euler(0f, _model.Heading, 0f);
    }

    public void ApplyStun(float duration)
    {
        _model.ApplyStun(duration);
    }

    public void UseItem()
    {
        ItemData item = ItemHolder.ConsumeItem();
        if (item == null)
        {
            return;
        }

        switch (item.Type)
        {
            case ItemType.Mushroom:
                _model.ApplyBoost(item.MushroomBoostDuration, item.MushroomSpeedMultiplier);
                break;
            case ItemType.GreenShell:
                FireGreenShell(item);
                break;
            case ItemType.Banana:
                PlaceBanana(item);
                break;
        }
    }

    void FireGreenShell(ItemData item)
    {
        if (_greenShellPrefab == null)
        {
            return;
        }

        GameObject projectileGo = Instantiate(
            _greenShellPrefab,
            transform.position + transform.forward,
            transform.rotation
        );
        projectileGo.GetComponent<GreenShellProjectile>().Launch(this, item);
    }

    void PlaceBanana(ItemData item)
    {
        if (_bananaPrefab == null)
        {
            return;
        }

        GameObject bananaGo = Instantiate(
            _bananaPrefab,
            transform.position - transform.forward,
            Quaternion.identity
        );
        bananaGo.GetComponent<BananaTrap>().Place(this, item);
    }
}
