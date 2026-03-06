using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class CharacterControllerScript : MonoBehaviour
{
    private CharacterController controller;
    
    private Controls controls;
    private Vector2 lookInput;
    private Vector2 moveInput;
    private InputAction sprintInput;

    public float speed = 4.0f;
    public float sprintSpeed = 7.0f;
    public float gravity = -9.8f;
    Vector3 velocity;

    public Transform mainCamera;

    private float xRotation;

    public bool hasDynamite;
    public GameObject dynamite;

    public bool hasGold;
    public GameObject gold;

    public float maxSprint = 8f;
    public float drainRate = 1f;
    public float rechargeRate = 0.5f;
    public float sprintDelay = 2f;

    private float sprint;
    private float counter;

    public GameObject sprintBar;
    private Image sprintBarImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controls = new Controls();
        controls.Player.Enable();

        sprintInput = controls.Player.Sprint;

        hasDynamite = false;
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        sprintBarImage = sprintBar.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Look();
        Gravity();

        if(hasDynamite)
        {
            dynamite.SetActive(true);
        }
        else
        {
            dynamite.SetActive(false);
        }

        if (hasGold)
        {
            gold.SetActive(true);
        }
        else
        {
            gold.SetActive(false);
        }

        Recharge();

        sprintBarImage.fillAmount = sprint / maxSprint;

        if (Keyboard.current.escapeKey.isPressed)
            Application.Quit();
    }

    void Movement()
    {
        moveInput = controls.Player.Move.ReadValue<Vector2>();
        float forwardInput = moveInput.y;
        float sideInput = moveInput.x;

        Vector3 move = (transform.right * sideInput + transform.forward * forwardInput).normalized;

        if (sprintInput.IsPressed() && forwardInput > 0 && sprint > 0)
        {
            controller.Move(move * sprintSpeed * Time.deltaTime);
            sprint -= drainRate * Time.fixedDeltaTime;
            counter = 0;
        } else
        {
            controller.Move(move * speed * Time.deltaTime);
        }
    }

    void Look()
    {
        lookInput = controls.Player.Look.ReadValue<Vector2>();

        float lookX = lookInput.x;
        float lookY = lookInput.y;

        transform.Rotate(Vector3.up * lookX);

        // Rotate camera up/down
        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        mainCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void Gravity()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void Recharge()
    {
        if (sprint >= maxSprint)
        {
            sprint = maxSprint;
            return;
        }

        counter += Time.fixedDeltaTime;

        if (counter >= sprintDelay)
        {
            sprint += rechargeRate * Time.fixedDeltaTime;
        }
    }
}
