using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float senX = 200f;
    public float senY = 200f;
    public Transform OrienRotation;

    private float xRotation;
    private float yRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * senX;
        float mouseY = Input.GetAxis("Mouse Y") * senY;

        yRotation += mouseX * Time.deltaTime;
        xRotation -= mouseY * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        OrienRotation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
