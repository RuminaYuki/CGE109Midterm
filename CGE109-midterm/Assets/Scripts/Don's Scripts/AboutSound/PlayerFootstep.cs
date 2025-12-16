using UnityEngine;

public enum TypeOfSoundWalk
{
    Normal,
    Vent
}

[System.Serializable]
public class SoundWalk
{
    public TypeOfSoundWalk typeOfSound;
    public AudioClip audioClipWalk;
}

public class PlayerFootstep : MonoBehaviour
{
    [Header("Sound")]
    public AudioSource audioSource;
    public SoundWalk[] soundWalks;

    [Header("Step Settings")]
    public float speedThreshold = 0.1f;
    public float startDelay = 0.1f;
    public float stopExtraTime = 0.2f;

    [Header("Run / Pitch Settings")]
    public KeyCode runKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    public float normalPitch = 1f;
    public float runPitch = 1.3f;
    public float crouchPitch = 0.85f;
    public float pitchLerpSpeed = 10f;

    [Header("Volume Settings")]
    public float normalVolume = 1f;
    public float crouchVolume = 0.4f;
    public float volumeLerpSpeed = 10f;

    [Header("CheckGround")]
    public float distance = 1f;
    public float radius = 0.25f;
    public LayerMask environmentMask;

    [Header("Debug")]
    public bool logSurfaceChange = false;

    private Vector3 lastPosition;
    private float startTimer = 0f;
    private float stopTimer = 0f;

    private TypeOfSoundWalk currentSurface = TypeOfSoundWalk.Normal;
    private AudioClip currentClip = null;

    void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        lastPosition = transform.position;
    }

    void Update()
    {
        float dt = Time.deltaTime;

        // =====================================================
        // คำนวณความเร็ว (เฉพาะแกน X,Z ไม่เอา Y)
        // =====================================================
        Vector3 delta = transform.position - lastPosition;
        Vector3 horizontalDelta = new Vector3(delta.x, 0f, delta.z);
        float speed = horizontalDelta.magnitude / Mathf.Max(dt, 0.0001f);
        lastPosition = transform.position;

        bool isMoving = speed > speedThreshold;

        // =====================================================
        // Start / Stop เสียงเดิน
        // =====================================================
        if (isMoving)
        {
            stopTimer = 0f;

            if (!audioSource.isPlaying)
            {
                startTimer += dt;
                if (startTimer >= startDelay)
                {
                    StartOrRefreshLoop(keepTime: false);
                    startTimer = 0f;
                }
            }
            else
            {
                RefreshIfSurfaceChanged();
            }
        }
        else
        {
            startTimer = 0f;

            if (audioSource.isPlaying)
            {
                stopTimer += dt;
                if (stopTimer >= stopExtraTime)
                {
                    StopLoop();
                    stopTimer = 0f;
                }
            }
            else
            {
                stopTimer = 0f;
            }
        }

        // =====================================================
        // Pitch (เดิน / วิ่ง / ย่อ)
        // =====================================================
        if (audioSource != null)
        {
            float targetPitch = normalPitch;

            if (isMoving)
            {
                if (Input.GetKey(crouchKey))
                    targetPitch = crouchPitch;
                else if (Input.GetKey(runKey))
                    targetPitch = runPitch;
            }

            audioSource.pitch = Mathf.Lerp(
                audioSource.pitch,
                targetPitch,
                dt * pitchLerpSpeed
            );
        }

        // =====================================================
        // Volume (ปกติ / ย่อง)
        // =====================================================
        if (audioSource != null)
        {
            float targetVolume = Input.GetKey(crouchKey)
                ? crouchVolume
                : normalVolume;

            audioSource.volume = Mathf.Lerp(
                audioSource.volume,
                targetVolume,
                dt * volumeLerpSpeed
            );
        }
    }

    // =====================================================
    // PUBLIC API
    // =====================================================
    public void ForceRefreshFootstepSound()
    {
        var newSurface = DetectSurfaceType();
        bool surfaceChanged = newSurface != currentSurface;
        currentSurface = newSurface;

        if (audioSource != null && audioSource.isPlaying)
        {
            StartOrRefreshLoop(keepTime: true);
        }
        else if (surfaceChanged && logSurfaceChange)
        {
            Debug.Log($"[Footstep] Surface updated (not playing): {currentSurface}");
        }
    }

    // =====================================================
    // INTERNAL
    // =====================================================
    void RefreshIfSurfaceChanged()
    {
        var newSurface = DetectSurfaceType();
        if (newSurface == currentSurface) return;

        currentSurface = newSurface;

        if (logSurfaceChange)
            Debug.Log($"[Footstep] Surface changed to: {currentSurface}");

        StartOrRefreshLoop(keepTime: true);
    }

    void StartOrRefreshLoop(bool keepTime)
    {
        if (audioSource == null || soundWalks == null || soundWalks.Length == 0)
            return;

        AudioClip clip = GetClipBySurface(currentSurface);

        if (clip == null)
        {
            clip = GetClipBySurface(TypeOfSoundWalk.Normal);
            if (clip == null) return;
        }

        if (audioSource.isPlaying && currentClip == clip) return;

        ApplyLoopClip(clip, keepTime);
    }

    void ApplyLoopClip(AudioClip clip, bool keepTime)
    {
        if (clip == null || audioSource == null) return;

        float t = (audioSource.isPlaying && keepTime)
            ? audioSource.time
            : Random.Range(0f, clip.length);

        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.time = Mathf.Repeat(t, clip.length);

        if (!audioSource.isPlaying)
            audioSource.Play();

        currentClip = clip;
    }

    AudioClip GetClipBySurface(TypeOfSoundWalk surface)
    {
        foreach (var sw in soundWalks)
        {
            if (sw != null && sw.typeOfSound == surface)
                return sw.audioClipWalk;
        }
        return null;
    }

    TypeOfSoundWalk DetectSurfaceType()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 0.1f;

        bool grounded = Physics.SphereCast(
            origin,
            radius,
            Vector3.down,
            out hit,
            distance,
            environmentMask,
            QueryTriggerInteraction.Ignore
        );

        if (!grounded)
            return currentSurface;

        if (hit.collider.CompareTag("Vent"))
            return TypeOfSoundWalk.Vent;

        return TypeOfSoundWalk.Normal;
    }

    void StopLoop()
    {
        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();

        currentClip = null;
    }
}
