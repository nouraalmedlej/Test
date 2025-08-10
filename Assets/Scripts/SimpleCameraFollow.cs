using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -6);
    public float followSpeed = 8f;
    public float rotateSpeed = 120f; // دوران بالكاميرا بالماوس

    float yaw;
    void LateUpdate()
    {
        if (!target) return;

        
        yaw += Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        Quaternion rot = Quaternion.Euler(0, yaw, 0);

        Vector3 desiredPos = target.position + rot * offset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, followSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}

