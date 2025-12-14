using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySoundWhenMoving : MonoBehaviour
{
    Vector3 lastPos;
    float speed;

    AudioSource audioSource;
    [SerializeField] private AudioClip clip;

    [Header("Volume Fade")]
    [SerializeField] float maxVolume = 1f;
    [SerializeField] float fadeSpeed = 5f;
    [SerializeField] float speedThreshold = 0.1f;

    void Start()
    {
        lastPos = transform.position;
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.volume = 0f;
    }

    void Update()
    {
        Vector3 delta = transform.position - lastPos;
        speed = delta.magnitude / Mathf.Max(Time.deltaTime, 0.0001f);
        lastPos = transform.position;

        bool isMoving = speed > speedThreshold;

        if (isMoving)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();

            audioSource.volume = Mathf.Lerp(
                audioSource.volume,
                maxVolume,
                Time.deltaTime * fadeSpeed
            );
        }
        else
        {
            audioSource.volume = Mathf.Lerp(
                audioSource.volume,
                0f,
                Time.deltaTime * fadeSpeed
            );

            // เงียบจริงแล้วค่อย Stop
            if (audioSource.volume < 0.01f && audioSource.isPlaying)
                audioSource.Stop();
        }
    }
}

