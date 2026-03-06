using UnityEngine;
using UnityEngine.InputSystem;

public class GoldScript : MonoBehaviour
{
    public GameObject pickupText;
    public CharacterControllerScript character;

    private bool isPlayer = false;

    private Controls controls;
    private InputAction interactInput;

    private void Start()
    {
        controls = new Controls();
        controls.Player.Enable();

        interactInput = controls.Player.Interact;

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
            if (interactInput.WasPressedThisFrame())
            {
                character.hasGold = true;
                Destroy(gameObject);
            }
        }
    }
}
