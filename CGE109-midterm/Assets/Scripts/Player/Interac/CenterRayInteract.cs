using UnityEngine;

public class CenterRayInteract : MonoBehaviour
{
    [Header("Settings")]
    public float interactDistance = 5f;       // ระยะตรวจจับสูงสุด
    public float detectRadius = 0.5f;         // รัศมีตรวจรอบจุดเล็ง
    public LayerMask interactableLayer;

    [Header("Debug")]
    public Transform currentTarget;

    public Camera cam;

    void Start()
    {

    }

    void Update()
    {
        DetectNearestInteractable();
    }

    void DetectNearestInteractable()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit[] hits = Physics.SphereCastAll(ray, detectRadius, interactDistance, interactableLayer);

        if (hits.Length > 0)
        {
            // หาอันที่ใกล้ที่สุดจากกล้อง
            float nearestDist = Mathf.Infinity;
            Transform nearestTarget = null;

            foreach (var hit in hits)
            {
                if (hit.collider == null) continue;

                float dist = hit.distance;
                if (dist < nearestDist)
                {
                    nearestDist = dist;
                    nearestTarget = hit.collider.transform;
                }
            }

            currentTarget = nearestTarget;
            if (currentTarget != null)
            {
                //Debug.Log("🎯 ใกล้ที่สุด: " + currentTarget.name);
                InteractableShowLabel interShow = currentTarget.GetComponent<InteractableShowLabel>();
                interShow.OnFogusByPlayer();
            }
        }
        else
        {
            currentTarget = null;
            //Debug.Log("🎯 ไม่มีใกล้ที่สุด: ");
        }
    }

    void OnDrawGizmosSelected()
    {
        if (cam == null) return;

        Gizmos.color = Color.yellow;
        // แสดงเส้นจากจุดกลางจอ
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Gizmos.DrawRay(ray.origin, ray.direction * interactDistance);
        // แสดงทรงกลมปลาย ray (ประมาณจุดตรวจ)
        Gizmos.DrawWireSphere(ray.origin + ray.direction * interactDistance * 0.5f, detectRadius);
    }
}
