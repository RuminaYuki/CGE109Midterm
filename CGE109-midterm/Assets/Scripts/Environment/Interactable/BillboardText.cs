using UnityEngine;

public class BillboardText : MonoBehaviour
{
    public Camera mainCamera;

    void LateUpdate()
    {
        transform.LookAt(mainCamera.transform.position);
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
    }
}
