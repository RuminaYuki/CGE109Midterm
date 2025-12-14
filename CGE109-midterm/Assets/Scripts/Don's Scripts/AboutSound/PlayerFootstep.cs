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
        // --------------------------
        // คำนวณความเร็ว
        // --------------------------
        Vector3 delta = transform.position - lastPosition;
        float speed = delta.magnitude / Mathf.Max(Time.deltaTime, 0.0001f);
        lastPosition = transform.position;

        bool isMoving = speed > speedThreshold;
        float dt = Time.deltaTime;

        // --------------------------
        // Start / Stop เสียงเดิน
        // --------------------------
        if (isMoving)
        {
            stopTimer = 0f;

            if (!audioSource.isPlaying)
            {
                startTimer += dt;
                if (startTimer >= startDelay)
                {
                    StartOrRefreshLoop(keepTime: false); // เริ่มเดินใหม่: สุ่มเวลา
                    startTimer = 0f;
                }
            }
            else
            {
                // เดินอยู่แล้ว: ถ้าพื้นเปลี่ยน ให้เปลี่ยนคลิปทันที
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

        // --------------------------
        // ปรับ PITCH (เดิน / วิ่ง / ย่อ)
        // --------------------------
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

            audioSource.pitch = Mathf.Lerp(audioSource.pitch, targetPitch, dt * pitchLerpSpeed);
        }

        // --------------------------
        // ปรับ VOLUME (ปกติ / ย่อง)
        // --------------------------
        if (audioSource != null)
        {
            float targetVolume = Input.GetKey(crouchKey) ? crouchVolume : normalVolume;
            audioSource.volume = Mathf.Lerp(audioSource.volume, targetVolume, dt * volumeLerpSpeed);
        }
    }

    // =========================================================
    // PUBLIC API: เรียกตอน "หยิบสิ่งใหม่ / Equip / เปลี่ยนโหมด"
    // =========================================================
    public void ForceRefreshFootstepSound()
    {
        // ถ้ากำลังเดินอยู่ -> เปลี่ยนคลิปทันที
        // ถ้าไม่ได้เดิน -> แค่อัปเดต surface ไว้ (พอเดินจะใช้ค่าล่าสุด)
        var newSurface = DetectSurfaceType();

        bool surfaceChanged = newSurface != currentSurface;
        currentSurface = newSurface;

        if (audioSource != null && audioSource.isPlaying)
        {
            // ถ้ากำลังเล่นอยู่: refresh แม้พื้นไม่เปลี่ยนก็ได้ (กรณีคุณอยากเปลี่ยนตามของที่ถือ)
            StartOrRefreshLoop(keepTime: true);
        }
        else
        {
            // ไม่ได้เล่น: ยังไม่ต้อง Play แต่จำ surface ไว้
            if (surfaceChanged && logSurfaceChange)
                Debug.Log($"[Footstep] Surface updated (not playing): {currentSurface}");
        }
    }

    // =========================================================
    // INTERNAL
    // =========================================================

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
        if (audioSource == null || soundWalks == null || soundWalks.Length == 0) return;

        // หา clip ตาม currentSurface
        AudioClip clip = GetClipBySurface(currentSurface);

        // fallback ถ้าหาไม่เจอ
        if (clip == null)
        {
            // ลอง fallback เป็น Normal
            clip = GetClipBySurface(TypeOfSoundWalk.Normal);
            if (clip == null) return;
        }

        // ถ้าคลิปไม่เปลี่ยน และกำลังเล่นอยู่ ก็ไม่ต้องทำอะไร
        if (audioSource.isPlaying && currentClip == clip) return;

        ApplyLoopClip(clip, keepTime);
    }

    void ApplyLoopClip(AudioClip clip, bool keepTime)
    {
        if (clip == null || audioSource == null) return;

        float t;
        if (audioSource.isPlaying && keepTime)
        {
            // เก็บตำแหน่งเวลาปัจจุบันไว้ให้ต่อเนียน ๆ
            t = audioSource.time;
        }
        else
        {
            // เริ่มใหม่: สุ่มเวลาตำแหน่งในคลิป
            t = Random.Range(0f, clip.length);
        }

        audioSource.clip = clip;
        audioSource.loop = true;

        // กัน time เกินคลิปใหม่
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

        // ยกขึ้นนิดนึง กัน start point ฝังพื้น
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
        {
            // ถ้าไม่โดนอะไร ให้ใช้ค่าปัจจุบัน (กันกระตุก)
            return currentSurface;
        }

        // เปลี่ยน logic ตาม tag ที่คุณใช้จริง
        if (hit.collider.CompareTag("Vent"))
            return TypeOfSoundWalk.Vent;

        return TypeOfSoundWalk.Normal;
    }

    void StopLoop()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        currentClip = null;
    }
}
