using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayRandomSound : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;

    public AudioSource AudioSource => audioSource; // 👈 เพิ่มบรรทัดนี้

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void playWithRandom()
    {
        if (audioClips == null || audioClips.Length == 0)
            return;

        int r = Random.Range(0, audioClips.Length);
        audioSource.clip = audioClips[r];
        audioSource.Play();
    }
}
