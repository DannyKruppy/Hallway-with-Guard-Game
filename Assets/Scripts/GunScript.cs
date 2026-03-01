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

    private int ammo = 6;
    private int spareAmmo = 0;

    public TextMeshProUGUI reserveText;
    public GameObject textHolder;

    public int SpareAmmo
    {
        get
        {
            return spareAmmo;
        }
        set
        {
            spareAmmo = value;
            reserveText.text = "+ " + spareAmmo;
            if (spareAmmo == 0)
                textHolder.SetActive(false);
            else
                textHolder.SetActive(true);
        }
    }

    public int Ammo
    {
        get
        {
            return ammo;
        }
        set
        {
            ammo = value;
            HandleAmmo();
        }
    }

    [Header("Ammo Counter Parameters")]
    public float uiRotateSpeed = 6f;

    private Quaternion ammoTargetRotation;

    public GameObject ammoMeter;
    public GameObject chamber1;
    public GameObject chamber2;
    public GameObject chamber3;
    public GameObject chamber4;
    public GameObject chamber5;
    public GameObject chamber6;

    void Start()
    {
        HandleAmmo();
        reserveText.text = "+ " + spareAmmo;
        if (spareAmmo == 0)
            textHolder.SetActive(false);
        else
            textHolder.SetActive(true);
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            FireWeapon();
        }

        if (Keyboard.current.rKey.wasPressedThisFrame && spareAmmo > 0 && ammo < 6)
        {
            HandleSpareAmmo();
        }

        ammoMeter.transform.localRotation = Quaternion.Slerp(
            ammoMeter.transform.localRotation,
            ammoTargetRotation,
            uiRotateSpeed * Time.deltaTime
        );
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
            Ammo--;

            RaycastHit hit;
            if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hit, 1000))
            {
                Instantiate(hitEffect, hit.point, Quaternion.identity);
            }

            transform.position = recoilObject.position;
            transform.rotation = recoilObject.rotation;
        }
    }

    void HandleAmmo()
    {
        Debug.Log("HandleAmmo called on " + gameObject.name);

        switch (ammo)
        {
            case 0:
                chamber1.SetActive(false);
                chamber2.SetActive(false);
                chamber3.SetActive(false);
                chamber4.SetActive(false);
                chamber5.SetActive(false);
                chamber6.SetActive(false);
                break;
            case 1:
                chamber1.SetActive(false);
                chamber2.SetActive(false);
                chamber3.SetActive(false);
                chamber4.SetActive(false);
                chamber5.SetActive(false);
                chamber6.SetActive(true);
                break;
            case 2:
                chamber1.SetActive(false);
                chamber2.SetActive(false);
                chamber3.SetActive(false);
                chamber4.SetActive(false);
                chamber5.SetActive(true);
                chamber6.SetActive(true);
                break;
            case 3:
                chamber1.SetActive(false);
                chamber2.SetActive(false);
                chamber3.SetActive(false);
                chamber4.SetActive(true);
                chamber5.SetActive(true);
                chamber6.SetActive(true);
                break;
            case 4:
                chamber1.SetActive(false);
                chamber2.SetActive(false);
                chamber3.SetActive(true);
                chamber4.SetActive(true);
                chamber5.SetActive(true);
                chamber6.SetActive(true);
                break;
            case 5:
                chamber1.SetActive(false);
                chamber2.SetActive(true);
                chamber3.SetActive(true);
                chamber4.SetActive(true);
                chamber5.SetActive(true);
                chamber6.SetActive(true);
                break;
            default:
                chamber1.SetActive(true);
                chamber2.SetActive(true);
                chamber3.SetActive(true);
                chamber4.SetActive(true);
                chamber5.SetActive(true);
                chamber6.SetActive(true);
                break;
        }

        float anglePerBullet = 60f;

        float targetAngle = (7 - ammo) * anglePerBullet;

        ammoTargetRotation = Quaternion.Euler(0f, 0f, targetAngle);
    }

    private void HandleSpareAmmo()
    {
        Ammo++;
        spareAmmo--;
        reserveText.text = "+ " + spareAmmo;
        if (spareAmmo == 0)
            textHolder.SetActive(false);
        else
            textHolder.SetActive(true);
    }
}
