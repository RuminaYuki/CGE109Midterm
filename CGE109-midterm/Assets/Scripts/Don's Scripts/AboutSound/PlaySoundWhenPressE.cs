using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class PlaySoundWhenPressE : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private Transform parent;
    bool isplayed;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (parent == null) return;

        if (parent.transform.childCount <= 1 && !isplayed)
        {
            audioSource.Play();
            isplayed = true;
        }
    }
}
