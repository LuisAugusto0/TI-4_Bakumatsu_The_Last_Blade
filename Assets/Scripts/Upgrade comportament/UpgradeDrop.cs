using UnityEngine;

public class UpgradeDrop : MonoBehaviour
{
    // The prefab to instantiate
    public GameObject[] prefabs;
    public int[] weights;
    private int totalWeight;

    // The probability of spawning the object (0 to 1)
    public float spawnProbability = 0.2f;  // 20% chance to spawn

    // Start is called before the first frame update
    void Start()
    {
        // Calculate the total weight sum at the start
        totalWeight = 0;
        foreach (int weight in weights)
        {
            totalWeight += weight;
        }
    }

    public void DropItem()
    {
        // Check if prefabs and weights are assigned
        if (prefabs != null && weights != null && prefabs.Length == weights.Length)
        {
            // Generate a random value between 0 and 1 for the spawn probability
            float randomChance = Random.value;

            // If the random chance is less than or equal to the spawnProbability, proceed with item drop
            if (randomChance <= spawnProbability)
            {
                // Now, determine which prefab to spawn based on weights
                int selectedPrefabIndex = GetWeightedRandomIndex();

                // Instantiate the selected prefab at the current object's position
                Instantiate(prefabs[selectedPrefabIndex], transform.position, Quaternion.identity);
            }
        }
        else
        {
            Debug.LogWarning("Prefabs or weights are not assigned, or their lengths don't match.");
        }
    }

    // Method to get a weighted random index based on the provided weights
    private int GetWeightedRandomIndex()
    {
        // Generate a random value between 0 and the total weight
        int randomValue = Random.Range(0, totalWeight);

        // Iterate over the weights to find the correct index
        int accumulatedWeight = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            accumulatedWeight += weights[i];

            // Check if the random value is within the current accumulated weight range
            if (randomValue < accumulatedWeight)
            {
                return i;  // Return the index of the selected prefab
            }
        }

        // Fallback, in case something goes wrong (this should never happen)
        return 0;
    }
}
