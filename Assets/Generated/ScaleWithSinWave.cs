using UnityEngine;

[RequireComponent(typeof(Transform))]
public class ScaleWithSinWave : MonoBehaviour
{
    [Tooltip("Minimum scale of the object.")]
    public float minScale = 0.5f;

    [Tooltip("Maximum scale of the object.")]
    public float maxScale = 1.5f;

    [Tooltip("Frequency of the sin wave.")]
    public float frequency = 1f;

    [Tooltip("Maximum offset to add to the frequency.")]
    public float maxOffset = 0.1f;

    [Tooltip("Whether to add a random offset to the time.")]
    public bool addRandomOffset = true;

    private Transform _transform;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        float offset = addRandomOffset ? Random.Range(0f, maxOffset) : 0f;
        float scale = Mathf.Lerp(minScale, maxScale, Mathf.Sin((Time.time + offset) * frequency));
        _transform.localScale = new Vector3(scale, scale, scale);
    }
}