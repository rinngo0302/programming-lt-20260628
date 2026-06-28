using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ItemBox : MonoBehaviour
{
    [SerializeField, Tooltip("抽選対象のアイテム一覧")]
    ItemData[] _itemPool;

    [SerializeField, Tooltip("取得後、再出現するまでの待機時間 (秒)")]
    float _respawnDelay = 5f;

    Collider _collider;
    MeshRenderer[] _renderers;

    void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
        _renderers = GetComponentsInChildren<MeshRenderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        KartController kart = other.GetComponent<KartController>();
        if (kart == null)
        {
            return;
        }

        ItemData picked = _itemPool[Random.Range(0, _itemPool.Length)];
        if (!kart.ItemHolder.TryAcquire(picked))
        {
            return;
        }

        StartCoroutine(RespawnAfterDelay());
    }

    IEnumerator RespawnAfterDelay()
    {
        SetAvailable(false);
        yield return new WaitForSeconds(_respawnDelay);
        SetAvailable(true);
    }

    void SetAvailable(bool isAvailable)
    {
        _collider.enabled = isAvailable;
        foreach (MeshRenderer meshRenderer in _renderers)
        {
            meshRenderer.enabled = isAvailable;
        }
    }
}
