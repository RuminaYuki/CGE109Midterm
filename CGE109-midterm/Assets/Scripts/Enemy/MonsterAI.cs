using UnityEngine;
using System.Collections;

public class MonsterAI : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 5f;

    [Header("Player Detection")]
    public float detectRadius = 10f;
    public float detectAngle = 60f;
    public string playerTag = "Player";
    public LayerMask obstructionMask;

    [Header("Player Lost Timeout (seconds)")]
    public float loseSightDelay = 1f;
    private float loseSightTimer = 0f;

    private Quaternion originalRotation;
    private bool isLookingAtSound = false;
    private Transform player;
    private bool canSeePlayer;
    private bool trackingPlayer = false;
    private Vector3 lastKnownPlayerPosition; // << เพิ่ม: เก็บตำแหน่งสุดท้ายที่เห็น

    public Transform headTransform;

    void Start()
    {
        originalRotation = transform.rotation;
        player = GameObject.FindGameObjectWithTag(playerTag)?.transform;
    }

    void Update()
    {
        DetectPlayer();

        if (canSeePlayer && player != null)
        {
            // รีเซ็ตเวลาเมื่อเห็นผู้เล่น
            loseSightTimer = 0f;
            trackingPlayer = true;
            lastKnownPlayerPosition = player.position; // << เก็บตำแหน่งล่าสุด

            RotateTowards(player.position);
        }
        else if (trackingPlayer)
        {
            loseSightTimer += Time.deltaTime;

            // ถ้าไม่เห็นผู้เล่นตามเวลาที่กำหนด → หยุดติดตาม
            if (loseSightTimer >= loseSightDelay)
            {
                trackingPlayer = false;
                loseSightTimer = 0f;

                // กลับไปตรวจเสียง / ลาดตระเวนตามปกติ
                StartCoroutine(ReturnToIdleRoutine());
            }
            else
            {
                // ถ้ายังอยู่ในช่วงเวลาติดตาม (แม้จะถูกบังแล้ว) ให้หมุนตามตำแหน่งสุดท้ายที่จำได้
                RotateTowards(lastKnownPlayerPosition);
            }
        }
    }

    private IEnumerator ReturnToIdleRoutine()
    {
        // กลับไปตำแหน่งเดิมก่อนโดนเบี่ยงเบน
        Quaternion flatOriginalRotation = GetFlatRotation(headTransform.transform.position + (originalRotation * Vector3.forward));

        while (!canSeePlayer && Quaternion.Angle(headTransform.transform.rotation, flatOriginalRotation) > 0.5f)
        {
            headTransform.transform.rotation = Quaternion.RotateTowards(
                headTransform.transform.rotation,
                flatOriginalRotation,
                rotationSpeed * 100f * Time.deltaTime
            );
            yield return null;
        }

        isLookingAtSound = false;
    }

    private void DetectPlayer()
    {
        canSeePlayer = false;
        if (player == null) return;

        Vector3 dirToPlayer = (player.position - headTransform.transform.position).normalized;

        // <<< ประกาศและคำนวณระยะทาง
        float distanceToPlayer = Vector3.Distance(headTransform.transform.position, player.position);

        if (distanceToPlayer <= detectRadius)
        {
            float angle = Vector3.Angle(headTransform.transform.forward, dirToPlayer);
            if (angle < detectAngle / 2)
            {
                RaycastHit hit;

                // <<< ใช้ Physics.Raycast เพื่อตรวจสอบสิ่งกีดขวางเท่านั้น
                // ถ้า Raycast ยิงไปโดน collider ที่อยู่ใน obstructionMask ก่อนถึงตัวผู้เล่น
                if (Physics.Raycast(headTransform.transform.position, dirToPlayer, out hit, distanceToPlayer, obstructionMask))
                {
                    // เจอสิ่งกีดขวาง (Obstacle) ก่อนถึงผู้เล่น = ถูกบัง
                    canSeePlayer = false;
                }
                else
                {
                    // ไม่เจอสิ่งกีดขวางก่อนถึงผู้เล่น = มองเห็น
                    canSeePlayer = true;

                    // ถ้ากำลังตรวจเสียงอยู่ → ยกเลิก
                    StopAllCoroutines();
                    isLookingAtSound = false;
                }
            }
        }

        // <<< ลบโค้ด RaycastAll เดิมออกแล้ว
    }


    void OnTriggerEnter(Collider other)
    {
        print("Here");

        if (other.CompareTag("Noise") && !isLookingAtSound && !trackingPlayer)
        {
            StartCoroutine(LookAtThenBack(other.transform.position));
        }
    }

    private IEnumerator LookAtThenBack(Vector3 targetPosition)
    {
        isLookingAtSound = true;

        Quaternion targetRotation = GetFlatRotation(targetPosition);

        while (!canSeePlayer && Quaternion.Angle(headTransform.transform.rotation, targetRotation) > 0.5f)
        {
            RotateTowards(targetPosition);
            yield return null;
        }

        headTransform.transform.rotation = targetRotation;

        if (canSeePlayer)
        {
            isLookingAtSound = false;
            yield break;
        }

        yield return new WaitForSeconds(1.2f);

        Quaternion flatOriginalRotation = GetFlatRotation(headTransform.transform.position + (originalRotation * Vector3.forward));

        while (!canSeePlayer && Quaternion.Angle(headTransform.transform.rotation, flatOriginalRotation) > 0.5f)
        {
            headTransform.transform.rotation = Quaternion.RotateTowards(
                headTransform.transform.rotation,
                flatOriginalRotation,
                rotationSpeed * 100f * Time.deltaTime
            );
            yield return null;
        }

        isLookingAtSound = false;
    }

    private Quaternion GetFlatRotation(Vector3 target)
    {
        Vector3 direction = (target - headTransform.transform.position);
        direction.y = 0;

        if (direction.sqrMagnitude == 0)
            return headTransform.transform.rotation;

        return Quaternion.LookRotation(direction);
    }

    private void RotateTowards(Vector3 target)
    {
        Quaternion targetRot = GetFlatRotation(target);

        headTransform.transform.rotation = Quaternion.RotateTowards(
            headTransform.transform.rotation,
            targetRot,
            rotationSpeed * 100f * Time.deltaTime
        );
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(headTransform.transform.position, detectRadius);

        Vector3 right = Quaternion.Euler(0, detectAngle / 2, 0) * headTransform.transform.forward * detectRadius;
        Vector3 left = Quaternion.Euler(0, -detectAngle / 2, 0) * transform.forward * detectRadius;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(headTransform.transform.position, headTransform.transform.position + right);
        Gizmos.DrawLine(headTransform.transform.position, headTransform.transform.position + left);
    }
}