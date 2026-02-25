using UnityEngine;

public class Pivot : MonoBehaviour
{
    public Transform targetCamera;
    public float rotationSpeed = 5f;

    void LateUpdate()
    {
        transform.position = targetCamera.position;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetCamera.rotation,
            rotationSpeed * Time.deltaTime
        );
    }
}
