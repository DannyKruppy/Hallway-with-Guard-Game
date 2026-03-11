using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    Collider endCollider;
    public CharacterControllerScript character;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        endCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && character.hasGold == true)
        {
            SceneManager.LoadScene(5);
        }
    }
}
