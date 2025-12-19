using UnityEngine;

public class OBJ : MonoBehaviour
{
    [HideInInspector] public Vector3 initialPosition;
    [HideInInspector] public Quaternion initialRotation;

    void Start()
    {
        // เซฟ pos/rot ทันทีที่ scene load (ก่อน Update)
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // Register ตัวเองกับ manager
        OBJManager.Instance.RegisterOBJ(this.gameObject);

        //Debug.Log("Monster registered: " + gameObject.name + " at " + initialPosition);
    }
}
