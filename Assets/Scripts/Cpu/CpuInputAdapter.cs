using UnityEngine;

public class CpuInputAdapter : MonoBehaviour
{
    [SerializeField, Tooltip("操作対象のKartController")]
    KartController _kartController;

    [SerializeField, Tooltip("ウェイポイントを保持するCourseDataアセット")]
    CourseData _courseData;

    [SerializeField, Tooltip("ウェイポイント到達判定の距離 (m)")]
    float _waypointReachThreshold = 2f;

    CpuDriverModel _model;

    void Awake()
    {
        _model = new CpuDriverModel(_courseData.Waypoints, _waypointReachThreshold);
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
