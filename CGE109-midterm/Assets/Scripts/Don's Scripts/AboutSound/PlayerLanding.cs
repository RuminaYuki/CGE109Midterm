using UnityEngine;

public class LandingAudio : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource landingSource;
    public AudioClip landingClip;
    [Range(0f, 1f)]
    public float landingVolume = 1f;

    [Header("Landing Condition")]
    [Tooltip("ต้องตกเร็วกว่า (ค่าติดลบ) เท่านี้ ถึงจะเล่นเสียง")]
    public float minDownSpeed = 3.5f;

    [Tooltip("กันเสียงซ้อนจากเฟรมกระตุก")]
    public float cooldown = 0.1f;

    [Header("Ground Check")]
    public float distance = 1f;
    public float radius = 0.25f;
    public LayerMask groundMask;

    // -----------------------
    private Vector3 lastPosition;
    private float prevYSpeed;
    private bool wasGrounded;
    private float cooldownTimer;

    void Awake()
    {
        if (landingSource == null)
            landingSource = GetComponent<AudioSource>();

        lastPosition = transform.position;
        wasGrounded = IsGrounded();
    }

    void Update()
    {
        float dt = Time.deltaTime;
        cooldownTimer -= dt;

        // คำนวณ Y speed (ใช้เฉพาะ landing)
        float ySpeed = (transform.position.y - lastPosition.y) / Mathf.Max(dt, 0.0001f);
        lastPosition = transform.position;

        bool groundedNow = IsGrounded();

        // ---- LANDING CHECK ----
        if (!wasGrounded && groundedNow)
        {
            TryPlayLanding(prevYSpeed);
        }

        wasGrounded = groundedNow;
        prevYSpeed = ySpeed;
    }

    void TryPlayLanding(float previousYSpeed)
    {
        if (landingClip == null) return;
        if (cooldownTimer > 0f) return;

        // ต้องเป็นการ "ตกลง"
        if (previousYSpeed <= -minDownSpeed)
        {
            landingSource.PlayOneShot(landingClip, landingVolume);
            cooldownTimer = cooldown;
        }
    }

    bool IsGrounded()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;

        return Physics.SphereCast(
            origin,
            radius,
            Vector3.down,
            out _,
            distance,
            groundMask,
            QueryTriggerInteraction.Ignore
        );
    }
}

