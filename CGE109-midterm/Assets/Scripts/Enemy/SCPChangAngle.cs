using UnityEngine;

public class SCPChangAngle : MonoBehaviour
{
    public GameObject SCP;
    public MonsterAI monsterAI;
    public Transform Traget;

    [Header("Rotation")]
    public float rotationSpeed = 120f;
    public float stopTurningAngle = 1f; // องศาที่ถือว่าหันเสร็จ

    public bool IsTurning;

    private void Update()
    {
        if (IsTurning)
        {
            monsterAI.StopAllCoroutines();
            monsterAI.SetStateIdle();
            RotateTowards(Traget.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsTurning = true;
        }
    }

    public void RotateTowards(Vector3 target)
    {
        // ทิศทางไปยังเป้าหมาย (ตัดแกน Y ออก → แนวนอนเท่านั้น)
        Vector3 direction = target - SCP.transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
        {
            IsTurning = false;
            
            return;
        }

        // สร้าง rotation เป้าหมาย
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        float angle = Quaternion.Angle(SCP.transform.rotation, targetRotation);

        IsTurning = angle > stopTurningAngle;

        if (IsTurning)
        {
            SCP.transform.rotation = Quaternion.RotateTowards(
                SCP.transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
        else
        {
            monsterAI.setOriginalROtation();
        }
    }
}
