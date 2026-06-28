using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GreenShellProjectile : MonoBehaviour
{
    KartController _owner;
    float _speed;
    float _lifetime;
    float _stunDuration;

    void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    public void Launch(KartController owner, ItemData itemData)
    {
        _owner = owner;
        _speed = itemData.ShellSpeed;
        _lifetime = itemData.ShellLifetime;
        _stunDuration = itemData.StunDuration;
    }

    void Update()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;

        _lifetime -= Time.deltaTime;
        if (_lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    // 壁では反射せず、発射者以外のレーサーに命中した時のみスタンを付与する(docs/spec/05-items.md)。
    void OnTriggerEnter(Collider other)
    {
        KartController target = other.GetComponent<KartController>();
        if (target == null || target == _owner)
        {
            return;
        }

        target.ApplyStun(_stunDuration);
        Destroy(gameObject);
    }
}
