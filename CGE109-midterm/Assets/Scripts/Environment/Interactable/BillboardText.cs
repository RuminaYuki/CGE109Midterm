using UnityEngine;

public class BillboardText : MonoBehaviour
{
    public Camera mainCamera;

    void Start()
    {
        // หา MainCamera อัตโนมัติ
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCamera == null) return;

        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
    }
}
