using UnityEngine;

public class FollowRotation : MonoBehaviour
{
    [Header("Target Object")]
    public Transform target; // Object ที่ต้องการหมุนตาม

    [Header("Settings")]
    public bool useLocalRotation = false; // ถ้า true จะใช้ rotation local แทน global

    void Update()
    {
        if (target == null) return;

        if (useLocalRotation)
        {
            // หมุนตาม rotation local ของ target
            transform.localRotation = target.localRotation;
        }
        else
        {
            // หมุนตาม rotation global ของ target
            transform.rotation = target.rotation;
        }
    }
}
