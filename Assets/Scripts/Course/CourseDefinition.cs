using System.Linq;
using UnityEngine;

public class CourseDefinition : MonoBehaviour
{
    [SerializeField, Tooltip("同期先のCourseDataアセット")]
    CourseData _courseData;

    [ContextMenu("Sync Checkpoints To CourseData")]
    public void SyncCheckpointsToCourseData()
    {
        Vector3[] positions = GetComponentsInChildren<Checkpoint>()
            .OrderBy(checkpoint => checkpoint.Index)
            .Select(checkpoint => checkpoint.transform.position)
            .ToArray();

        _courseData.SetCheckpoints(positions);
    }
}
