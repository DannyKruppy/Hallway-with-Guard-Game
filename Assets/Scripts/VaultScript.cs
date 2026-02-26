using UnityEngine;

public class VaultScript : MonoBehaviour
{
    private bool isOpen = false;
    private bool isOpening = false;
    public Transform playerCamera;
    public float coneAngle = 40f;
    public float maxDistance = 7f;
    public GameObject promptUI;
    public GameObject dynamite;

    public float openAngle = 80f;
    public float openSpeed = 60f;

    private Quaternion openRotation;
    private Quaternion closedRotation;

    public Transform doorMesh;

    public CharacterControllerScript character;

    void Start()
    {
        promptUI.SetActive(false);

        closedRotation = transform.localRotation;
        openRotation = Quaternion.Euler(transform.localEulerAngles.x,transform.localEulerAngles.y - openAngle, transform.localEulerAngles.z);;
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsLookingAtDoor())
            promptUI.SetActive(false);

        else
        {
            promptUI.SetActive(true);

            if(Input.GetKeyDown(KeyCode.E) && !isOpen)
            {
                isOpening = true;
                isOpen = true;
                promptUI.SetActive(false);

                character.hasDynamite = false;
            }
        }

        if(isOpening)
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, openRotation, Time.deltaTime * openSpeed);

            if(Quaternion.Angle(transform.localRotation,openRotation) < 0.5f)
            {
                transform.rotation = openRotation;
                isOpening = false;
            }
        }

        if(isOpen)
        {
            promptUI.SetActive(false);
        }
    }
    
    bool IsLookingAtDoor()
    {
        Vector3 toDoor = doorMesh.position - playerCamera.position;
        float distance = toDoor.magnitude;

        if (distance > maxDistance)
            return false;

        toDoor.Normalize();

        //check if door is in front of camera
        if (Vector3.Dot(playerCamera.forward, toDoor) < 0)
            return false;

        //check if within cone angle
        float angle = Vector3.Angle(playerCamera.forward, toDoor);
        if (angle > coneAngle)
            return false;

        //check if player is in front of the vault (uses vault's Y as forward)
        Vector3 toPlayer = (playerCamera.position - transform.position).normalized;
        if (Vector3.Dot(transform.forward, toPlayer) < 0)
            return false;

        //raycast to check obstruction
        /* if (Physics.Raycast(playerCamera.position, toDoor, out RaycastHit hit, maxDistance))
        {
            if (hit.transform != doorMesh)
                return false;
        } */

        return true;
    }
}
