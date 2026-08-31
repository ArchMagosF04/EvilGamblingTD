using UnityEngine;

public class GizmoMeasurer : MonoBehaviour
{
    [Header("Distance Radius")]
    [SerializeField] private Color distanceColor = Color.yellow;
    [SerializeField] private bool showDistance;
    [SerializeField] private float distanceToMeasure;

    [Header("Angle Measure")]
    [SerializeField] private Color angleColor = Color.red;
    [SerializeField] private bool showAngle;
    [SerializeField, Range(0, 360)] private float angleToMeasure;

    private void OnDrawGizmos()
    {
        if (showDistance)
        {
            Gizmos.color = distanceColor;

            Gizmos.DrawWireSphere(transform.position, distanceToMeasure);
        }
        if (showAngle)
        {
            Gizmos.color = angleColor;

            Gizmos.DrawRay(transform.position, Quaternion.Euler(0, angleToMeasure / 2, 0) * transform.right * distanceToMeasure);
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -angleToMeasure / 2, 0) * transform.right * distanceToMeasure);
        }
    }
}
