using UnityEngine;
using UnityEngine.InputSystem;

public class GunScript : MonoBehaviour
{
    public Transform target;
    public Transform recoilObject;
    public float followSpeed = 20f;
    public float rotationFollowSpeed = 1f;

    void LateUpdate()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            transform.position = recoilObject.position;
            transform.rotation = recoilObject.rotation;
        }

        transform.position = Vector3.Lerp(transform.position, target.position, followSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, rotationFollowSpeed * Time.deltaTime);
    }
}
