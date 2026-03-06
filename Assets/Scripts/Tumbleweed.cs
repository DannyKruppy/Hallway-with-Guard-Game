using UnityEngine;

public class Tumbleweed : MonoBehaviour
{
    public float moveForce = 2f;        // Initial linear force for rolling
    public float torqueForce = 5f;      // Initial rotational torque for spinning
    public float lifetime = 5f;         // Time before fading & destroying

    private Rigidbody rb;               
    private Material mat;
    private Color color;

    void Start()
    {
        rb = GetComponent<Rigidbody>();     // Hook rigidbody component

        Vector3 dir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;      // Choose a random horizontal direction to roll
        rb.AddForce(dir * moveForce, ForceMode.Impulse);    // Apply linear impulse to roll tumbleweed
        rb.AddTorque(Random.onUnitSphere * torqueForce, ForceMode.Impulse);     // Apply random torque for spin

        // Material setup for fade effect
        mat = GetComponent<Renderer>().material;
        color = mat.color;

        // Start coroutine to fade out and destroy over time
        StartCoroutine(FadeOut());
    }

    System.Collections.IEnumerator FadeOut()
    {
        float t = 0;    // Timer for fade

        while (t < lifetime)    // Loop until lifetime expires
        {
            t += Time.deltaTime;    // Increment timer

            // Linearly interpolate alpha from 1 -> 0, applying new alpha while keeping RGB
            float alpha = Mathf.Lerp(1f, 0f, t / lifetime);
            mat.color = new Color(color.r, color.g, color.b, alpha);

            yield return null;      // Wait for next frame
        }

        Destroy(gameObject);        // Remove tumbleweed from scene
    }
}