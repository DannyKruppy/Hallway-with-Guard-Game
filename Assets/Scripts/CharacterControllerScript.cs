using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class CharacterControllerScript : MonoBehaviour
{
    private CharacterController controller;

    public float speed = 5.0f;
    public float gravity = -9.8f;
    Vector3 velocity;

    public Transform mainCamera;

    public float mouseSens;
    private float xRotation;

    public bool hasDynamite;
    public GameObject dynamite;

    public bool hasGold;
    public GameObject gold;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hasDynamite = false;
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        MouseLook();
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

        if (Keyboard.current.wKey.isPressed)
            Application.Quit();
    }

    void Movement()
    {
        float forwardInput = 0f;
        float sideInput = 0f;

        if (Keyboard.current.wKey.isPressed)
            forwardInput += 1f;

        if (Keyboard.current.sKey.isPressed)
            forwardInput -= 1f;

        if (Keyboard.current.aKey.isPressed)
            sideInput -= 1f;

        if (Keyboard.current.dKey.isPressed)
            sideInput += 1f;

        Vector3 move = (transform.right * sideInput + transform.forward * forwardInput).normalized;

        controller.Move(move * speed * Time.deltaTime);
    }

    void MouseLook()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * mouseSens;
        float mouseY = mouseDelta.y * mouseSens;

        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera up/down
        xRotation -= mouseY;
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
}
