using Gamekit3D;
using UnityEditor.Timeline.Actions;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundButtonManager : MonoBehaviour
{
    AudioSource audioSource;
    PlayerMovement PlayerMovement;

    [SerializeField] private AudioClip[] audioClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (!PlayerMovement)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            PlayerMovement = player.GetComponent<PlayerMovement>();
        }
    }

    public void Play()
    {
        audioSource.clip = audioClip[0];
        audioSource.Play();
    }

    public void PlayCheckbyKeycard1()
    {
        if (PlayerMovement.KeyCard)
        {
            audioSource.clip = audioClip[0];
            audioSource.Play();
        }
        else if (!PlayerMovement.KeyCard)
        {
            audioSource.clip = audioClip[1];
            audioSource.Play();
        }
    }
    public void PlayCheckbyKeycard2()
    {
        if (PlayerMovement.KeyCard2)
        {
            audioSource.clip = audioClip[0];
            audioSource.Play();
        }
        else if (!PlayerMovement.KeyCard2)
        {
            audioSource.clip = audioClip[1];
            audioSource.Play();
        }
    }

}
