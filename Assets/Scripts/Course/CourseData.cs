using UnityEngine;

[CreateAssetMenu(fileName = "CourseData", menuName = "Course/CourseData")]
public class CourseData : ScriptableObject
{
    [Header("基本設定")]
    [SerializeField, Tooltip("総ラップ数")]
    int _lapCount = 3;

    [Header("チェックポイント")]
    [
        SerializeField,
        Tooltip("通過順に並べたチェックポイントのワールド座標。最後の要素がスタート/ゴールライン")
    ]
    Vector3[] _checkpoints;

    [Header("ウェイポイント(CPU AI用)")]
    [
        SerializeField,
        Tooltip("CPUが追従する目標地点のワールド座標(チェックポイントより細かい間隔を想定)")
    ]
    Vector3[] _waypoints;

    [Header("アイテムボックス")]
    [SerializeField, Tooltip("アイテムボックスを配置するワールド座標")]
    Vector3[] _itemBoxPositions;

    public int LapCount => _lapCount;
    public Vector3[] Checkpoints => _checkpoints;
    public Vector3[] Waypoints => _waypoints;
    public Vector3[] ItemBoxPositions => _itemBoxPositions;
}
