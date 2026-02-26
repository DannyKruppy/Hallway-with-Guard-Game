using UnityEngine;
using UnityEngine.InputSystem;

public class AmmoPickupScript : MonoBehaviour
{
    public GameObject pickupText;
    public GunScript gunScript;

    private bool isPlayer = false;

    private void Start()
    {
        pickupText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pickupText.SetActive(true);
            isPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pickupText.SetActive(false);
            isPlayer = false;
        }
    }

    private void Update()
    {
        if (isPlayer)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                gunScript.ammo++;
                Destroy(gameObject);
            }
        }
    }
}
