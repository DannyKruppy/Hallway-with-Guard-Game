using UnityEngine;
using UnityEngine.InputSystem;

public class BlowWallUp : MonoBehaviour
{
    Collider ground;
    public CharacterControllerScript character;

    public GameObject curiousText;
    public GameObject blowUpText;
    public GameObject builtWall;
    public GameObject explodedWall;

    public bool isBlown;
    private bool playerInTrigger;

    private Controls controls;
    private InputAction interactInput;

    public AudioSource Explosion;

    public ParticleSystem explosionVFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controls = new Controls();
        controls.Player.Enable();

        interactInput = controls.Player.Interact;

        blowUpText.SetActive(false);
        curiousText.SetActive(false);

        builtWall.SetActive(true);
        explodedWall.SetActive(false);

        playerInTrigger = false;
        isBlown = false;
    }

    void Update()
    {
        if(playerInTrigger == true && interactInput.WasPressedThisFrame())
        {
            Explosion.Play();

            explosionVFX.Clear();
            explosionVFX.Play();

            Destroy(builtWall);
            blowUpText.SetActive(false);
            character.hasDynamite = false;

            explodedWall.SetActive(true);

            isBlown = true;
        }

        if(isBlown)
        {
            blowUpText.SetActive(false);
            curiousText.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && character.hasDynamite == false)
        {
            curiousText.SetActive(true);
        }

        if(other.CompareTag("Player") && character.hasDynamite == true)
        {
            blowUpText.SetActive(true);
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            blowUpText.SetActive(false);
            curiousText.SetActive(false);
            playerInTrigger = false;
        }
    }
}
