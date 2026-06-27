using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Header("追従対象")]
    [SerializeField, Tooltip("追従するカートのTransform")]
    Transform _target;

    [Header("追従パラメータ")]
    [SerializeField, Tooltip("カートからの後方距離 (m)")]
    float _distance = 5.5f;

    [SerializeField, Tooltip("カートからの高さ (m)")]
    float _height = 2.5f;

    [SerializeField, Tooltip("注視点のカート前方オフセット (m)")]
    float _lookAheadDistance = 3f;

    [SerializeField, Tooltip("位置の追従補間速度")]
    float _positionLerpSpeed = 5f;

    [SerializeField, Tooltip("回転の追従補間速度")]
    float _rotationLerpSpeed = 5f;

    void LateUpdate()
    {
        if (_target == null)
        {
            return;
        }

        Vector3 desiredPosition =
            _target.position - _target.forward * _distance + Vector3.up * _height;
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            _positionLerpSpeed * Time.deltaTime
        );

        Vector3 lookAtPoint = _target.position + _target.forward * _lookAheadDistance;
        Quaternion desiredRotation = Quaternion.LookRotation(lookAtPoint - transform.position);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRotation,
            _rotationLerpSpeed * Time.deltaTime
        );
    }
}
