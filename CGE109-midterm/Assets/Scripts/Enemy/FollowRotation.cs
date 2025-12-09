using UnityEngine;

public class FollowObj : MonoBehaviour
{
    [Header("Target Object")]
    public Transform target; // Object ที่ต้องการหมุนตาม

    //[Header("Settings")]
    //public bool useLocalRotation = false; // ถ้า true จะใช้ rotation local แทน global

    void Update()
    {
        if (target == null) return;

        transform.position = target.position;



        Vector3 euler = target.eulerAngles;
        euler.x = transform.eulerAngles.x;  // ใช้ Y ของตัวเองแทน

        transform.rotation = Quaternion.Euler(euler);

    }
}
