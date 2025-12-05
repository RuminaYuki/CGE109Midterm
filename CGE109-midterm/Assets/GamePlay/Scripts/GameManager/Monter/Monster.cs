using UnityEngine;

public class Monster : MonoBehaviour
{
    [HideInInspector] public Vector3 initialPosition;
    [HideInInspector] public Quaternion initialRotation;

    void Start()
    {
        // เซฟ pos/rot ทันทีที่ scene load (ก่อน Update)
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // Register ตัวเองกับ manager
        MonsterManager.Instance.RegisterMonster(this);

        //Debug.Log("Monster registered: " + gameObject.name + " at " + initialPosition);
    }
}