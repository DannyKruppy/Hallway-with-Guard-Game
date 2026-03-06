using UnityEngine;
using TMPro; // needed for TextMeshPro

public class Duck : MonoBehaviour
{
    private AudioSource quackAudio;
    private bool playerInRange = false;

    public TextMeshProUGUI interactPrompt; // assign this in Inspector

    void Start()
    {
        quackAudio = GetComponent<AudioSource>();
        interactPrompt.gameObject.SetActive(false); // hide by default
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!quackAudio.isPlaying)
            {
                quackAudio.Play();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactPrompt.gameObject.SetActive(true); // show UI
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactPrompt.gameObject.SetActive(false); // hide UI
        }
    }
}