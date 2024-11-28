using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeComportament : MonoBehaviour
{
    // Base speed and amplitude for floating
    public float baseSpeed = 1.0f;  // Base floating speed
    public float baseXAmplitude = 0.05f;  // Base floating amplitude
    public float baseYAmplitude = 0.2f;  // Base floating amplitude

    // Random factor for speed and amplitude
    public float speedRandomness = 0.2f;  // The randomness factor for speed
    public float amplitudeRandomness = 0.0f;  // The randomness factor for amplitude

    // The initial position of the object
    private Vector3 initialPosition;

    // Flickering variables
    public float flickerDelay = 2.0f;  // Delay before the flickering starts (in seconds)
    private bool isFlickering = false;
    private float flickerDuration = 1.0f;  // Total duration of one flicker (from 0 to 1 opacity)
    private int flickerCount = 5;  // Number of flicker cycles
    private float flickerTimer = 0f;
    private float flickerCycleTimer = 0f;
    private Renderer objectRenderer;

    // Start is called before the first frame update
    void Start()
    {
        // Save the initial position of the object
        initialPosition = transform.position;

        // Get the Renderer component to modify the material's opacity
        objectRenderer = GetComponent<Renderer>();

        // Optionally, you can randomize the initial speed and amplitude immediately
        RandomizeMovement();

        // Start the flickering process after a delay
        StartCoroutine(StartFlickeringAfterDelay(flickerDelay));
    }

    // Update is called once per frame
    void Update()
    {
        // Apply the floating motion with a random factor
        float xOffset = Mathf.Sin(Time.time * baseSpeed) * baseXAmplitude;  // Sinusoidal motion on x-axis
        float yOffset = Mathf.Cos(Time.time * baseSpeed) * baseYAmplitude;  // Sinusoidal motion on y-axis

        // Apply the new position to the object
        transform.position = new Vector3(initialPosition.x + xOffset, initialPosition.y + yOffset, initialPosition.z);

        // If the object is flickering, handle the opacity transition
        if (isFlickering)
        {
            flickerCycleTimer += Time.deltaTime;

            // Calculate flicker alpha value (oscillates between 0 and 1)
            float alpha = Mathf.Abs(Mathf.Sin(flickerCycleTimer * Mathf.PI));  // Sinusoidal flickering

            // Set the material's alpha (opacity)
            if (objectRenderer != null)
            {
                Color color = objectRenderer.material.color;
                color.a = alpha;
                objectRenderer.material.color = color;
            }

            // Once one cycle of flickering is done, decrease the flicker count
            if (flickerCycleTimer >= flickerDuration)
            {
                flickerCycleTimer = 0f;
                flickerCount--;

                if (flickerCount <= 0)
                {
                    // After 5 flickers, destroy the object
                    Destroy(gameObject);
                }
            }
        }
    }

    // Coroutine to start flickering after a certain delay
    private IEnumerator StartFlickeringAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isFlickering = true;  // Start the flickering process
    }

    // Randomizes the floating speed and amplitude periodically
    void RandomizeMovement()
    {
        // Randomize the speed and amplitude based on the given randomness factors
        baseSpeed += Random.Range(-speedRandomness, speedRandomness);
        baseXAmplitude += Random.Range(-amplitudeRandomness, amplitudeRandomness);
        baseYAmplitude += Random.Range(-amplitudeRandomness, amplitudeRandomness);

        // Ensure the amplitude remains positive to avoid negative floating values
        baseXAmplitude = Mathf.Abs(baseXAmplitude);
        baseYAmplitude = Mathf.Abs(baseYAmplitude);
    }
}
