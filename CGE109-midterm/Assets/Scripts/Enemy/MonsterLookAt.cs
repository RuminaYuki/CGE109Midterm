using UnityEngine;
using System.Collections;

public class MonsterLookAt : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 5f; // ความเร็วในการหมุน

    private Quaternion originalRotation; // ทิศทางเดิม
    private bool isLooking = false;

    void Start()
    {
        // เก็บทิศทางตอนเริ่มเกมไว้
        originalRotation = transform.rotation;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Noise"))
        {
            if (!isLooking)
                StartCoroutine(LookAtThenBack(other.transform.position));
        }
    }

    private IEnumerator LookAtThenBack(Vector3 targetPosition)
    {
        isLooking = true;

        // ---- ขั้นที่ 1: คำนวณทิศทางเฉพาะแนวราบ (ล็อกแกน Y) ----
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f; // ไม่ให้เงย/ก้ม
        if (direction == Vector3.zero)
        {
            isLooking = false;
            yield break;
        }
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // ---- ขั้นที่ 2: หมุนไปหาตำแหน่งเป้าหมาย ----
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.5f)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * 100f * Time.deltaTime
            );
            yield return null;
        }

        transform.rotation = targetRotation; // ให้หันตรงเป๊ะ

        // ---- ขั้นที่ 3: หลังจาก "หันจนเสร็จ" แล้ว รอ 1 วินาที ----
        yield return new WaitForSeconds(1.5f);

        // ---- ขั้นที่ 4: หมุนกลับทิศทางเดิม (เฉพาะแกน Y เช่นกัน) ----
        Vector3 originalForward = originalRotation * Vector3.forward;
        originalForward.y = 0f;
        Quaternion flatOriginalRotation = Quaternion.LookRotation(originalForward);

        while (Quaternion.Angle(transform.rotation, flatOriginalRotation) > 0.5f)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                flatOriginalRotation,
                rotationSpeed * 100f * Time.deltaTime
            );
            yield return null;
        }

        transform.rotation = flatOriginalRotation; // กลับทิศเดิมเป๊ะ
        isLooking = false;
    }
}
