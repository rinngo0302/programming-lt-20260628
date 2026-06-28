using System.Collections;
using R3;
using UnityEngine;

public class CpuInputAdapter : MonoBehaviour
{
    [SerializeField, Tooltip("操作対象のKartController")]
    KartController _kartController;

    [SerializeField, Tooltip("ウェイポイントを保持するCourseDataアセット")]
    CourseData _courseData;

    [SerializeField, Tooltip("ウェイポイント到達判定の距離 (m)")]
    float _waypointReachThreshold = 2f;

    [SerializeField, Tooltip("アイテム取得後、使用するまでのランダム遅延の最小値 (秒)")]
    float _itemUseDelayMin = 0.5f;

    [SerializeField, Tooltip("アイテム取得後、使用するまでのランダム遅延の最大値 (秒)")]
    float _itemUseDelayMax = 2f;

    CpuDriverModel _model;

    void Awake()
    {
        _model = new CpuDriverModel(_courseData.Waypoints, _waypointReachThreshold);
        _kartController.ItemHolder.HeldItem.Subscribe(OnHeldItemChanged).AddTo(this);
    }

    // アイテム取得後、すぐには使わずランダムな短い遅延後に使用する(docs/spec/06-cpu-ai.md)。
    void OnHeldItemChanged(ItemData item)
    {
        if (item != null)
        {
            StartCoroutine(UseItemAfterDelay());
        }
    }

    IEnumerator UseItemAfterDelay()
    {
        yield return new WaitForSeconds(Random.Range(_itemUseDelayMin, _itemUseDelayMax));
        _kartController.UseItem();
    }

    void Update()
    {
        _kartController.Steer = _model.ComputeSteer(
            _kartController.transform.position,
            _kartController.transform.eulerAngles.y
        );
        _kartController.Accelerate = true;
        _kartController.Brake = false;
    }
}
