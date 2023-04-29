using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    public AudioClip soundClip;

    [Tooltip("Time in seconds before playing the sound.")]
    public float delayTime = 1f;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Invoke("PlaySound", delayTime);
    }

    private void PlaySound()
    {
        audioSource.PlayOneShot(soundClip);
    }
}