using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayRandomSound : MonoBehaviour
{
    AudioSource auidoSource;
    [SerializeField] AudioClip[] audioClips;

    private void Start()
    {
        auidoSource = GetComponent<AudioSource>();
    }

    public void playWithRandom()
    {
        int r = Random.Range(0, audioClips.Length);
        auidoSource.clip = audioClips[r];
        auidoSource.Play();
    }
}
