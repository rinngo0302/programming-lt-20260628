using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("種類")]
    [SerializeField, Tooltip("アイテムの種類")]
    ItemType _type;

    [Header("キノコ(加速)")]
    [SerializeField, Tooltip("速度上昇の持続時間 (秒)")]
    float _mushroomBoostDuration = 2f;

    [SerializeField, Tooltip("通常速度に対する速度上昇率")]
    float _mushroomSpeedMultiplier = 1.5f;

    [Header("みどり甲羅(前方攻撃)")]
    [SerializeField, Tooltip("弾の飛行速度 (m/s)")]
    float _shellSpeed = 25f;

    [SerializeField, Tooltip("命中・消滅までの最大飛行時間 (秒)")]
    float _shellLifetime = 3f;

    [Header("バナナ(設置トラップ)")]
    [SerializeField, Tooltip("自然消滅までの持続時間 (秒)")]
    float _bananaLifetime = 20f;

    [Header("スタン(共通)")]
    [SerializeField, Tooltip("スタンによる操作不能の持続時間 (秒)")]
    float _stunDuration = 1.5f;

    public ItemType Type => _type;
    public float MushroomBoostDuration => _mushroomBoostDuration;
    public float MushroomSpeedMultiplier => _mushroomSpeedMultiplier;
    public float ShellSpeed => _shellSpeed;
    public float ShellLifetime => _shellLifetime;
    public float BananaLifetime => _bananaLifetime;
    public float StunDuration => _stunDuration;
}
