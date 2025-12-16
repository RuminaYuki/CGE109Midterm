using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class PlaySoundWhenPressECheckByParent : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private Transform parent;
    [SerializeField] private int calParent; //เช็คว่า parent มีกี่ตัวแล้วค่อยเล่น
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

        if (parent.transform.childCount <= calParent && !isplayed) //เช็คว่า parent มีกี่ตัวแล้วค่อยเล่น
        {
            audioSource.Play();
            isplayed = true;
        }
    }
}
