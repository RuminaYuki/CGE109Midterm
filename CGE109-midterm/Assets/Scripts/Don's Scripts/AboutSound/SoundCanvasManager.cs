using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(AudioSource), typeof(CanvasGroup))]
public class SoundCanvasManager : MonoBehaviour
{
    AudioSource audioSource;
    CanvasGroup canvasGroup;

    GameObject ambience;
    bool isCanvasVisible;

    [Header("Sound Settings")]
    public int loopCount = 4;
    public float fadeOutDuration = 1.5f;

    Coroutine playRoutine;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();

        FindAmbience();

        isCanvasVisible = canvasGroup.alpha >= 1f;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindAmbience();
    }

    void FindAmbience()
    {
        ambience = GameObject.FindGameObjectWithTag("Ambience");
    }

    void Update()
    {
        bool shouldBeVisible = canvasGroup.alpha >= 1f;
        if (shouldBeVisible == isCanvasVisible) return;

        isCanvasVisible = shouldBeVisible;

        if (isCanvasVisible)
            OnCanvasShown();
        else
            OnCanvasHidden();
    }

    // =========================
    // Canvas Events
    // =========================

    void OnCanvasShown()
    {
        if (playRoutine != null)
            StopCoroutine(playRoutine);

        playRoutine = StartCoroutine(PlayLoopAndFade());

        if (ambience != null)
            ambience.SetActive(false);
    }

    void OnCanvasHidden()
    {
        if (playRoutine != null)
        {
            StopCoroutine(playRoutine);
            playRoutine = null;
        }

        if (audioSource.isPlaying)
            audioSource.Stop();

        audioSource.volume = 1f;

        if (ambience != null)
            ambience.SetActive(true);
    }

    // =========================
    // Sound Logic
    // =========================

    IEnumerator PlayLoopAndFade()
    {
        audioSource.loop = false;
        audioSource.volume = 1f;

        for (int i = 0; i < loopCount; i++)
        {
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
        }

        // ---- Fade Out ----
        float startVolume = audioSource.volume;
        float t = 0f;

        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeOutDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = 1f;
        playRoutine = null;
    }
}
