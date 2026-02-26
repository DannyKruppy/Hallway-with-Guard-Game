using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class GunScript : MonoBehaviour
{
    public Transform target;
    public Transform adsTarget;
    public Transform recoilObject;
    public float followSpeed = 20f;
    public float rotationFollowSpeed = 1f;

    public Transform raycastOrigin;
    public GameObject hitEffect;

    public int ammo = 60;

    public TextMeshProUGUI ammoText;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            FireWeapon();
        }
        ammoText.text = ("Ammo: " + ammo);
    }

    void LateUpdate()
    {
        if (Mouse.current.rightButton.isPressed)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, adsTarget.localPosition, followSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, adsTarget.rotation, rotationFollowSpeed * Time.deltaTime);
        } else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, target.localPosition, followSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, rotationFollowSpeed * Time.deltaTime);
        }
    }

    void FireWeapon()
    {
        if (ammo > 0)
        {
            ammo--;

            RaycastHit hit;
            if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hit, 1000))
            {
                Instantiate(hitEffect, hit.point, Quaternion.identity);
            }

            transform.position = recoilObject.position;
            transform.rotation = recoilObject.rotation;
        }
    }
}
