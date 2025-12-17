using UnityEngine;
using System.Collections;

public class MonsterAI : MonoBehaviour
{
    public enum MonsterState { Idle, Investigating, Chasing, Lost }
    public MonsterState state = MonsterState.Idle;

    [Header("Transforms")]
    public Transform raycastTransform;   // จุดปล่อย Ray
    public Transform headTransform;      // หัวที่หมุน

    [Header("Vision Settings")]
    public float rotationSpeed = 5f;     // ความเร็วพื้นฐาน
    public float detectRadius = 10f;
    public float detectAngle = 60f;
    public LayerMask obstructionMask;
    public string playerTag = "Player";

    [Header("See Player Delay Event")]
    public float seePlayerDelay = 0.5f;
    private float seePlayerTimer = 0f;

    [Header("Memory / Delay")]
    public float loseSightDelay = 1f;
    private float loseSightTimer = 0f;

    [Header("Head Limit")]
    public float headMaxRotation = 60f; // หัวหมุนได้ +-60°
    public float bodyTurnSpeedMultiplier = 50f; // ความเร็วหมุนตัวตอนไล่ล่า (คูณจาก Base)

    [Header("Return Settings (New)")]
    [Tooltip("ตัวคูณความเร็วตอนหันกลับ (ค่าน้อย = กลับช้าๆ, ค่ามาก = กลับเร็ว)")]
    public float returnSpeedMultiplier = 5f; // <--- ปรับตรงนี้ถ้าอยากให้ช้า/เร็วขึ้น (เดิมมันเทียบเท่า 100f)

    private Transform player; //Main Camera
    public Transform playerOBJ;
    private Quaternion originalHeadRotation;
    private Quaternion originalBodyRotation;
    public Vector3 lastKnownPlayerPosition;
    private bool canSeePlayer = false;

    public Animator animator;
    public SpawnPoint spawnPoint;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag)?.transform;
        originalHeadRotation = transform.rotation;
        originalBodyRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        DetectPlayer();
        
        switch (state)
        {
            case MonsterState.Idle:
                IdleBehavior();
                break;

            case MonsterState.Investigating:
                InvestigateBehavior();
                break;

            case MonsterState.Chasing:
                ChaseBehavior();
                break;

            case MonsterState.Lost:
                LostBehavior();
                break;
        }
    }

    // -------------------- STATES --------------------

    void IdleBehavior()
    {
        if (canSeePlayer)
        {
            state = MonsterState.Chasing;
        }
    }

    void InvestigateBehavior()
    {
        RotateTowards(lastKnownPlayerPosition);

        if (Quaternion.Angle(headTransform.rotation, Quaternion.LookRotation(lastKnownPlayerPosition - headTransform.position)) < 2f)
        {
            StartCoroutine(ReturnToIdleAfterDelay());
        }

        if (canSeePlayer)
        {
            StopAllCoroutines();
            state = MonsterState.Chasing;
        }
    }

    void ChaseBehavior()
    {
        if (!player) return;

        lastKnownPlayerPosition = player.position;
        RotateTowards(lastKnownPlayerPosition);

        if (!canSeePlayer)
        {
            loseSightTimer += Time.deltaTime;

            if (loseSightTimer >= loseSightDelay)
            {
                loseSightTimer = 0f;
                
                state = MonsterState.Lost;
            }
        }
        else loseSightTimer = 0f;
    }

    void LostBehavior()
    {
        RotateTowards(lastKnownPlayerPosition);

        if (Quaternion.Angle(headTransform.rotation, Quaternion.LookRotation(lastKnownPlayerPosition - headTransform.position)) < 2f)
        {
            StartCoroutine(ReturnToIdleAfterDelay());
        }

        if (canSeePlayer)
        {
            StopAllCoroutines();
            state = MonsterState.Chasing;
        }
    }

    // -------------------- DETECT PLAYER --------------------

    private void DetectPlayer()
    {
        canSeePlayer = false;
        if (!player) return;

        Vector3 directionToPlayer = (player.position - raycastTransform.position).normalized;
        float distanceToPlayer = Vector3.Distance(raycastTransform.position, player.position);

        if (distanceToPlayer <= detectRadius)
        {
            float angle = Vector3.Angle(raycastTransform.forward, directionToPlayer);

            if (angle <= detectAngle / 2)
            {
                if (!Physics.Raycast(raycastTransform.position, directionToPlayer, distanceToPlayer, obstructionMask))
                {
                    canSeePlayer = true;
                    animator.SetBool("See Player", true);
                    Debug.Log("See player");
                }
            }
        }

        if (canSeePlayer)
        {
            lastKnownPlayerPosition = player.position;

            seePlayerTimer += Time.deltaTime;

            if (seePlayerTimer >= seePlayerDelay)
            {
                spawnPoint.GotoSpawnPoint(playerOBJ);
            }
        }
        else
        {
            seePlayerTimer = 0;
            
        }
    }

    // -------------------- ROTATION SYSTEM --------------------

    public void RotateTowards(Vector3 target)
    {
        // หมุนหัวเร็วๆ (ไล่ล่า)
        Vector3 directionToTarget = (target - headTransform.position).normalized;
        if (directionToTarget == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        headTransform.rotation = Quaternion.RotateTowards(
            headTransform.rotation,
            targetRotation,
            rotationSpeed * 100f * Time.deltaTime // 100f คือเร็วมาก
        );

        float yaw = GetLocalYawDifference();

        if (Mathf.Abs(yaw) > headMaxRotation)
        {
            Vector3 bodyDirection = (target - transform.position).normalized;
            bodyDirection.y = 0;

            if (bodyDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    Quaternion.LookRotation(bodyDirection),
                    rotationSpeed * bodyTurnSpeedMultiplier * Time.deltaTime
                );
            }
        }
    }

    private float GetLocalYawDifference()
    {
        Vector3 bodyForward = transform.forward;
        Vector3 headForward = headTransform.forward;
        bodyForward.y = 0;
        headForward.y = 0;

        float angle = Vector3.Angle(bodyForward, headForward);
        return Vector3.Cross(bodyForward, headForward).y < 0 ? -angle : angle;
    }

    // -------------------- RESET HEAD & BODY (SLOWLY) --------------------

    private IEnumerator ReturnToIdleAfterDelay()
    {
        yield return new WaitForSeconds(1.3f);
        animator.SetBool("See Player", false);
        while (Quaternion.Angle(headTransform.rotation, originalHeadRotation) > 0.1f ||
               Quaternion.Angle(transform.rotation, originalBodyRotation) > 0.1f)
        {
            // ใช้ returnSpeedMultiplier แทน 100f เพื่อให้ช้าลง
            float step = rotationSpeed * returnSpeedMultiplier * Time.deltaTime;

            // 1. หมุนหัวกลับช้าๆ
            headTransform.rotation = Quaternion.RotateTowards(
                headTransform.rotation,
                originalHeadRotation,
                step
            );

            // 2. หมุนตัวกลับช้าๆ
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                originalBodyRotation,
                step
            );

            yield return null;
        }

        headTransform.rotation = originalHeadRotation;
        transform.rotation = originalBodyRotation;

        state = MonsterState.Idle;
    }

    // -------------------- EVENTS & DEBUG --------------------

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Noise") && state != MonsterState.Chasing)
        {
            lastKnownPlayerPosition = other.transform.position;
            animator.SetBool("See Player", true);
            state = MonsterState.Investigating;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!raycastTransform) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(raycastTransform.position, detectRadius);
        Vector3 right = Quaternion.Euler(0, detectAngle / 2, 0) * raycastTransform.forward * detectRadius;
        Vector3 left = Quaternion.Euler(0, -detectAngle / 2, 0) * raycastTransform.forward * detectRadius;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(raycastTransform.position, raycastTransform.position + right);
        Gizmos.DrawLine(raycastTransform.position, raycastTransform.position + left);
    }

    public void SetStateIdle()
    {
        state = MonsterState.Idle;
    }

    public void setOriginalROtation()
    {
        Debug.Log("here");
        originalHeadRotation = transform.rotation;
        originalBodyRotation = transform.rotation;
    }
}