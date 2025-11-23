using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;

    private void LateUpdate() // 👈 ใช้ LateUpdate ด้วย
    {
        transform.position = cameraPosition.position;
    }
}
