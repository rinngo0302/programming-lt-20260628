using R3;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    [SerializeField, Tooltip("通過順のインデックス(0始まり)。最後の番号がスタート/ゴールライン")]
    int _index;

    readonly Subject<Collider> _passed = new();

    public int Index => _index;
    public Observable<Collider> Passed => _passed;

    void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        _passed.OnNext(other);
    }

    void OnDestroy()
    {
        _passed.Dispose();
    }
}
