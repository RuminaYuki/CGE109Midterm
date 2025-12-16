using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public enum SoundEnemyType
{
    Idle, Chase
}

[System.Serializable]
public class SoundEnemyState
{
    public SoundEnemyType Type;
    public AudioClip audioClip;
}

[RequireComponent(typeof(AudioSource), typeof(NavMeshAgent))]
public class SoundEnemyManager : MonoBehaviour
{
    [SerializeField] private List<SoundEnemyState> soundState;
    [SerializeField] private float fadeTime = 0.3f;

    [Header("Start Time Settings")]
    [SerializeField] private bool randomizeStartTime = true;
    [SerializeField] private float fixedStartTime = 0.3f;

    private AudioSource audioSource;
    private NavMeshAgent navMeshAgent;

    private SoundEnemyType currentType;
    private bool hasTypeSet = false;
    private Coroutine fadeRoutine;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        audioSource.loop = true;
    }

    private void Update()
    {
        var targetType = navMeshAgent.velocity.sqrMagnitude > 0.0001f
            ? SoundEnemyType.Chase
            : SoundEnemyType.Idle;

        if (hasTypeSet && targetType == currentType)
            return;

        AudioClip clip = GetClip(targetType);
        if (clip == null)
            return;

        currentType = targetType;
        hasTypeSet = true;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeToClip(clip));
    }

    private IEnumerator FadeToClip(AudioClip newClip)
    {
        float startVolume = audioSource.volume;

        // 🔻 Fade Out
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeTime);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.clip = newClip;

        // 🎲 ตั้งเวลาเริ่มเสียง
        if (newClip != null && newClip.length > 0f)
        {
            float startTime;

            if (randomizeStartTime)
            {
                startTime = Random.Range(0f, newClip.length);
            }
            else
            {
                startTime = Mathf.Clamp(fixedStartTime, 0f, newClip.length - 0.01f);
            }

            audioSource.time = startTime;
        }

        audioSource.Play();

        // 🔺 Fade In
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f, startVolume, t / fadeTime);
            yield return null;
        }

        audioSource.volume = startVolume;
    }

    private AudioClip GetClip(SoundEnemyType type)
    {
        foreach (var s in soundState)
        {
            if (s.Type == type)
                return s.audioClip;
        }

        return null;
    }
}
