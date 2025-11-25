using UnityEngine;

public class BillboardText : MonoBehaviour
{
    public Camera mainCamera;

    private void Awake()
    {
        mainCamera = FindAnyObjectByType<Camera>();
    }

    void LateUpdate()
    {
        transform.LookAt(mainCamera.transform.position);
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
    }
}
