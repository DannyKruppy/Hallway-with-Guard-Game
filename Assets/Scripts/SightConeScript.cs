using UnityEngine;

public class SightConeScript : MonoBehaviour
{
    public EnemyScript enemyScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyScript.playerInCone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyScript.playerInCone = false;
        }
    }
}
