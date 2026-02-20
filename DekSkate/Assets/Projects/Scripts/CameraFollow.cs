using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Settings")]
    public float smoothTime = 0.25f;
    public Vector3 offset = new Vector3(0, 2, -10);

    [Header("Limits")]
    public bool lockY = false;
    public float minX = -200f;
    public float maxX = 200f;

    private Vector3 _velocity = Vector3.zero;

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + offset;

        if (lockY)
        {
            targetPosition.y = transform.position.y;
        }

        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime);
    }
}
