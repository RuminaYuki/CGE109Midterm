using UnityEngine;

public class PlayerFootstep : MonoBehaviour
{
    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip loopFootstep;

    [Header("Step Settings")]
    public float speedThreshold = 0.1f;
    public float startDelay = 0.1f;
    public float stopExtraTime = 0.2f;

    [Header("Run / Pitch Settings")]
    public KeyCode runKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    public float normalPitch = 1f;
    public float runPitch = 1.3f;
    public float crouchPitch = 0.85f;   // ⬅️ pitch ตอนย่อตัว
    public float pitchLerpSpeed = 10f;

    [Header("Volume Settings")]
    public float normalVolume = 1f;
    public float crouchVolume = 0.4f;
    public float volumeLerpSpeed = 10f;

    private Vector3 lastPosition;
    private float startTimer = 0f;
    private float stopTimer = 0f;

    void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        lastPosition = transform.position;
    }

    void Update()
    {
        // --------------------------
        // คำนวณความเร็ว
        // --------------------------
        Vector3 delta = transform.position - lastPosition;
        float speed = delta.magnitude / Mathf.Max(Time.deltaTime, 0.0001f);
        lastPosition = transform.position;

        bool isMoving = speed > speedThreshold;
        float dt = Time.deltaTime;

        // --------------------------
        // จัดการ Start / Stop เสียงเดิน
        // --------------------------
        if (isMoving)
        {
            stopTimer = 0f;

            if (!audioSource.isPlaying)
            {
                startTimer += dt;
                if (startTimer >= startDelay)
                {
                    StartLoopRandom();
                    startTimer = 0f;
                }
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

        // --------------------------
        // ปรับ PITCH (เดิน / วิ่ง / ย่อ)
        // --------------------------
        if (audioSource != null)
        {
            float targetPitch = normalPitch;

            if (isMoving)
            {
                if (Input.GetKey(crouchKey))
                {
                    // ⬇️ ย่อ = ใช้ crouchPitch และกันไม่ให้ run มีผล
                    targetPitch = crouchPitch;
                }
                else if (Input.GetKey(runKey))
                {
                    // 🏃 วิ่ง (มีผลเฉพาะถ้าไม่ได้กดย่ออยู่)
                    targetPitch = runPitch;
                }
            }

            audioSource.pitch =
                Mathf.Lerp(audioSource.pitch, targetPitch, dt * pitchLerpSpeed);
        }

        // --------------------------
        // ปรับ VOLUME (เดินปกติ / ย่อง)
        // --------------------------
        if (audioSource != null)
        {
            float targetVolume = Input.GetKey(crouchKey)
                                ? crouchVolume
                                : normalVolume;

            audioSource.volume =
                Mathf.Lerp(audioSource.volume, targetVolume, dt * volumeLerpSpeed);
        }
    }

    void StartLoopRandom()
    {
        if (audioSource == null || loopFootstep == null) return;

        audioSource.clip = loopFootstep;
        audioSource.loop = true;

        audioSource.time = Random.Range(0f, loopFootstep.length);
        audioSource.Play();

        audioSource.pitch = normalPitch;
        audioSource.volume = normalVolume;
    }

    void StopLoop()
    {
        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();
    }
}
