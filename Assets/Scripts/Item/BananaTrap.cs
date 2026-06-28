using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BananaTrap : MonoBehaviour
{
    KartController _owner;
    float _lifetime;
    float _stunDuration;

    void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    public void Place(KartController owner, ItemData itemData)
    {
        _owner = owner;
        _lifetime = itemData.BananaLifetime;
        _stunDuration = itemData.StunDuration;
    }

    void Update()
    {
        _lifetime -= Time.deltaTime;
        if (_lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    // 設置者以外が踏むとスタンを付与し、一定時間で自然消滅する(docs/spec/05-items.md)。
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
