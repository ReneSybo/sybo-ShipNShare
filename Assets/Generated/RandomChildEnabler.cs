using UnityEngine;

public class RandomChildEnabler : MonoBehaviour
{
    [Tooltip("Minimum time to wait before enabling a child object.")]
    public float minWaitTime = 1f;

    [Tooltip("Maximum time to wait before enabling a child object.")]
    public float maxWaitTime = 5f;

    [Tooltip("List of child objects to enable/disable.")]
    public GameObject[] childObjects;

    private void Start()
    {
        // Start the coroutine to enable/disable child objects.
        StartCoroutine(EnableDisableChildObjects());
    }

    private System.Collections.IEnumerator EnableDisableChildObjects()
    {
        while (true)
        {
            // Wait for a random amount of time.
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            // Enable a random child object.
            int randomIndex = Random.Range(0, childObjects.Length);
            childObjects[randomIndex].SetActive(true);

            // Wait for 3 seconds.
            yield return new WaitForSeconds(3f);

            // Disable the previously enabled child object.
            childObjects[randomIndex].SetActive(false);
        }
    }
}